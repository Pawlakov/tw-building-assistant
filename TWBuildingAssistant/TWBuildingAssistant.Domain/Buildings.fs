module TWBuildingAssistant.Domain.Buildings

open FSharp.Data.Sql
open Database
open Effects
open Province
open Settings

type BuildingLevel =
    { Id:int
      Name:string
      Incomes:Income[]
      EffectSet:EffectSet }

type BuildingBranch =
    { Id:int
      Name:string
      Interesting:bool
      Levels:BuildingLevel[] }

type BuildingLibraryEntry =
    { Descriptor:SlotDescriptor
      BuildingBranches:BuildingBranch[] }

let emptyBuildingLevel =
    { Id = 0; Name = "Empty"; Incomes = [||]; EffectSet = emptyEffectSet }

let emptyBuildingBranch =
    { Id = 0; Name = "Empty"; Interesting = true; Levels = [|emptyBuildingLevel|]}

let getUnlockedBuildingLevelIds (ctx:sql.dataContext) settings =
    let techIds =
        query {
            for tech in ctx.Dbo.TechnologyLevels do
            where (tech.FactionId = settings.FactionId && tech.Order <= settings.TechnologyTier)
            select (tech.Id) }

    let locks =
        query {
            for lock in ctx.Dbo.BuildingLevelLocks do
            where ((lock.TechnologyLevelId |=| techIds) && (settings.UseAntilegacyTechnologies || not lock.Antilegacy)) }
        |> Seq.toList

    let loop (lockedIds, unlockedIds) (lock:sql.dataContext.``dbo.BuildingLevelLocksEntity``) =
        match lock.Lock with
        | true ->
            (lock.BuildingLevelId::lockedIds, unlockedIds)
        | false ->
            (lockedIds, lock.BuildingLevelId::unlockedIds)

    let (lockedIds, unlockedIds) = 
        locks 
        |> List.fold loop ([], [])

    unlockedIds |> List.except lockedIds

let getBuildingLibraryEntry (ctx:sql.dataContext) settings descriptor =
    let usedBranchIds =
        query {
            for branchUse in ctx.Dbo.BuildingBranchUses do
            where (branchUse.FactionId = settings.FactionId)
            select (branchUse.BuildingBranchId) }
        |> Seq.toList

    let slotTypeInt =
        match descriptor.SlotType with
        | Main -> 0
        | Coastal -> 1
        | General -> 2

    let regionTypeInt =
        match descriptor.RegionType with
        | City -> 0
        | Town -> 1

    let usedBranches =
        query {
            for branch in ctx.Dbo.BuildingBranches do
            where (branch.Id |=| usedBranchIds)
            where (branch.SlotType = slotTypeInt)
            where (branch.RegionType = None || branch.RegionType = Some regionTypeInt)
            where (branch.ReligionId = None || branch.ReligionId = Some settings.ReligionId)
            where (branch.ResourceId = None || branch.ResourceId = descriptor.ResourceId) }

    let unlockedLevelIds = getUnlockedBuildingLevelIds ctx settings
    let unlockedLevels = 
        query {
            for level in ctx.Dbo.BuildingLevels do
            where (level.Id |=| unlockedLevelIds) }
        |> Seq.toList

    let firstLoop strainPairs (branch:sql.dataContext.``dbo.BuildingBranchesEntity``) =
        let rec traverseBranch ancestors (current:sql.dataContext.``dbo.BuildingLevelsEntity``) =
            let branchLevels = 
                current::ancestors
            let children =
                unlockedLevels
                |> List.filter (fun x -> x.ParentBuildingLevelId = Some current.Id)
            match children with
            | [] -> 
                [ branchLevels ]
            | _ -> 
                children 
                |> List.map (traverseBranch branchLevels)
                |> List.collect (fun x -> x)
        let strains =
            unlockedLevels
            |> List.find (fun x -> x.Id = branch.RootBuildingLevelId)
            |> traverseBranch []
        match branch.AllowParallel with
        | true ->
            strainPairs 
            |> List.append (strains |> List.map (fun x -> (branch, x |> List.rev)))
        | false ->
            let levels =
                strains
                |> List.collect (fun x -> x |> List.rev)
                |> List.distinct
            (branch, levels)::strainPairs

    let strainPairs =
        usedBranches 
        |> Seq.fold firstLoop []

    let secondLoop finalDictionary ((branch:sql.dataContext.``dbo.BuildingBranchesEntity``), (levels:sql.dataContext.``dbo.BuildingLevelsEntity`` list)) =
        let levelsOther =
            levels
            |> List.map (fun level -> { Id = level.Id; Name = level.Name; Incomes = (level.Id |> (getIncomes ctx) |> List.toArray); EffectSet = (level.EffectId |> getEffectOption ctx) }:BuildingLevel)
            |> List.toArray
        match levelsOther with
        | [||] ->
            finalDictionary
        | _ ->
            { Id = branch.Id; Name = branch.Name; Interesting = branch.Interesting; Levels = levelsOther }::finalDictionary

    let finalDictionary =
        strainPairs
        |> List.fold secondLoop []
    let finalFinalDictionary =
        match descriptor.SlotType with
        | General -> emptyBuildingBranch::finalDictionary
        | _ -> finalDictionary  

    { Descriptor = descriptor; BuildingBranches = (finalFinalDictionary |> List.toArray) }

let getBuildingLibrary settings =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let slotTypes = [ Main; Coastal; General ]
    let regionTypes = [ City; Town ]
    let resourceIdsInProvince = 
        query {
        for region in ctx.Dbo.Regions do
        where (region.ProvinceId = settings.ProvinceId)
        select (region.ResourceId)
        distinct }
        |> Seq.toList

    let descriptors =
        slotTypes 
        |> List.collect (fun slotType -> regionTypes |> List.collect (fun regionType -> resourceIdsInProvince |> List.map (fun resourceId -> { SlotType = slotType; RegionType = regionType; ResourceId = resourceId })))

    let results =
        descriptors 
        |> List.map (getBuildingLibraryEntry ctx settings)
        |> List.toArray

    results