module TWBuildingAssistant.Domain.Buildings

open TWBuildingAssistant.Data.Sqlite
open Data
open Effects
open Province
open Settings

type internal BuildingLevel =
    { Id: int
      Name: string
      LocalEffectSet: LocalEffectSet
      EffectSet: EffectSet }

type internal BuildingBranch =
    { Id: int
      Name: string
      Interesting: bool
      Levels: BuildingLevel [] }

type internal BuildingLibraryEntry =
    { Descriptor: SlotDescriptor
      BuildingBranches: BuildingBranch [] }

let internal emptyBuildingLevel =
    { Id = 0
      Name = "Empty"
      LocalEffectSet = emptyLocalEffectSet
      EffectSet = emptyEffectSet }

let internal emptyBuildingBranch =
    { Id = 0
      Name = "Empty"
      Interesting = true
      Levels = [| emptyBuildingLevel |] }

let internal getUnlockedBuildingLevelIds (factionsData: FactionsData.Root []) settings =
    let faction =
        query {
            for faction in factionsData do
                where (faction.Id = settings.FactionId)
                head
        }

    let techs =
        query {
            for tech in faction.TechnologyLevels do
                where (tech.Order <= settings.TechnologyTier)
                select tech
        }

    let loopWithAntilegacy (lockedIds, unlockedIds) (tech: FactionsData.TechnologyLevel) =
        (Array.concat [lockedIds; tech.AntilegacyLockedBuildingLevelIds], Array.concat [unlockedIds; tech.AntilegacyUnlockedBuildingLevelIds; tech.UniversalUnlockedBuildingLevelIds])

    let loopWithoutAntilegacy (lockedIds, unlockedIds) (tech: FactionsData.TechnologyLevel) =
        (lockedIds, Array.concat [unlockedIds; tech.UniversalUnlockedBuildingLevelIds])

    let (lockedIds, unlockedIds) = 
        match settings.UseAntilegacyTechnologies with
        | true ->
            techs 
            |> Seq.fold loopWithAntilegacy ([||], [||])
        | false ->
            techs 
            |> Seq.fold loopWithoutAntilegacy ([||], [||])

    unlockedIds |> Array.except lockedIds

let internal getBuildingLibraryEntry (ctx: DatabaseContext) factionsData settings descriptor =
    let usedBranchIds =
        query {
            for branchUse in ctx.BuildingBranchUses do
                where (branchUse.FactionId = settings.FactionId)
                select (branchUse.BuildingBranchId)
        }
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
            for branch in ctx.BuildingBranches do
                where (List.contains branch.Id usedBranchIds)
                where (branch.SlotType = slotTypeInt)

                where (
                    not branch.RegionType.HasValue
                    || branch.RegionType.Value = regionTypeInt
                )

                where (
                    not branch.ReligionId.HasValue
                    || branch.ReligionId.Value = settings.ReligionId
                )

                where (
                    not branch.ResourceId.HasValue
                    || (match descriptor.ResourceId with
                        | Some resourceId -> branch.ResourceId.Value = resourceId
                        | None -> false)
                )
        }

    let unlockedLevelIds = getUnlockedBuildingLevelIds factionsData settings

    let unlockedLevels =
        query {
            for level in ctx.BuildingLevels do
                where (Array.contains level.Id unlockedLevelIds)
        }
        |> Seq.toList

    let firstLoop strainPairs (branch: Entities.BuildingBranch) =
        let rec traverseBranch ancestors (current: Entities.BuildingLevel) =
            let branchLevels = current :: ancestors

            let children =
                unlockedLevels
                |> List.filter (fun x -> x.ParentBuildingLevelId = current.Id)

            match children with
            | [] -> [ branchLevels ]
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
            |> List.append (
                strains
                |> List.map (fun x -> (branch, x |> List.rev))
            )
        | false ->
            let levels =
                strains
                |> List.collect (fun x -> x |> List.rev)
                |> List.distinct

            (branch, levels) :: strainPairs

    let strainPairs = usedBranches |> Seq.fold firstLoop []

    let secondLoop finalDictionary ((branch: Entities.BuildingBranch), (levels: Entities.BuildingLevel list)) =
        let levelsOther =
            levels
            |> List.map (fun level ->
                { Id = level.Id
                  Name = level.Name
                  LocalEffectSet = (level.Id |> getLocalEffect ctx)
                  EffectSet =
                    ((if level.EffectId.HasValue then
                          Some level.EffectId.Value
                      else
                          None)
                     |> getEffectOption ctx) }: BuildingLevel)
            |> List.toArray

        match levelsOther with
        | [||] -> finalDictionary
        | _ ->
            { Id = branch.Id
              Name = branch.Name
              Interesting = branch.Interesting
              Levels = levelsOther }
            :: finalDictionary

    let finalDictionary = strainPairs |> List.fold secondLoop []

    let finalFinalDictionary =
        match descriptor.SlotType with
        | General -> emptyBuildingBranch :: finalDictionary
        | _ -> finalDictionary

    { Descriptor = descriptor
      BuildingBranches = (finalFinalDictionary |> List.toArray) }

let internal getBuildingLibrary (ctx: DatabaseContext) factionsData settings =
    let slotTypes = [ Main; Coastal; General ]
    let regionTypes = [ City; Town ]

    let resourceIdsInProvince =
        query {
            for region in ctx.Regions do
                where (region.ProvinceId = settings.ProvinceId)
                select (region.ResourceId)
                distinct
        }
        |> Seq.toList

    let descriptors =
        slotTypes
        |> List.collect (fun slotType ->
            regionTypes
            |> List.collect (fun regionType ->
                resourceIdsInProvince
                |> List.map (fun resourceId ->
                    { SlotType = slotType
                      RegionType = regionType
                      ResourceId =
                        (if resourceId.HasValue then
                             Some resourceId.Value
                         else
                             None) })))

    let results =
        descriptors
        |> List.map (getBuildingLibraryEntry ctx factionsData settings)
        |> List.toArray

    results

let internal getBuildingLevel (ctx: DatabaseContext) id =
    let level =
        query {
            for level in ctx.BuildingLevels do
                where (level.Id = id)
                head
        }

    let levelEffectId =
        if level.EffectId.HasValue then
            Some level.EffectId.Value
        else
            None

    { Id = level.Id
      Name = level.Name
      LocalEffectSet = (level.Id |> getLocalEffect ctx)
      EffectSet = (levelEffectId |> getEffectOption ctx) }: BuildingLevel
