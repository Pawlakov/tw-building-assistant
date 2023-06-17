module TWBuildingAssistant.Domain.Buildings

open Data
open Effects
open Province
open Settings

type internal BuildingLevel =
    { Id: string
      Name: string
      LocalEffectSet: LocalEffectSet
      EffectSet: EffectSet }

type internal BuildingBranch =
    { Id: string
      Name: string
      Interesting: bool
      Levels: BuildingLevel [] }

type internal BuildingLibraryEntry =
    { Descriptor: SlotDescriptor
      BuildingBranches: BuildingBranch [] }


let internal emptyBuildingLevel =
    { Id = ""
      Name = "Empty"
      LocalEffectSet = emptyLocalEffectSet
      EffectSet = emptyEffectSet }

let internal emptyBuildingBranch =
    { Id = ""
      Name = "Empty"
      Interesting = true
      Levels = [| emptyBuildingLevel |] }


let private getBuildingLibraryEntry (buildingsData: JsonBuildingBranch []) (usedBuildingBranchIdSeq: seq<string>) (unlockedBuildingLevelIdSeq: seq<string>) settings descriptor =
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
            for branch in buildingsData do
                where (Seq.contains branch.Id usedBuildingBranchIdSeq)
                where (branch.SlotType = slotTypeInt)
                where (branch.RegionType = None || branch.RegionType = Some regionTypeInt)
                where (branch.ReligionId = None || branch.ReligionId = Some settings.ReligionId)
                where (branch.ResourceId = None || branch.ResourceId = descriptor.ResourceId)
        }

    let firstLoop strainPairs (branch: JsonBuildingBranch) =
        let rec traverseStrain (current: JsonBuildingLevel option) (accumulated: JsonBuildingLevel list) =
            match current with
            | None -> accumulated
            | Some current ->
                let parent = 
                    match current.ParentId with
                    | None -> None
                    | Some parentId -> 
                        branch.Levels 
                        |> Array.filter (fun x -> x.Id = parentId) 
                        |> Array.exactlyOne
                        |> Some
                traverseStrain parent (current::accumulated)

        let isCrownLevel (level: JsonBuildingLevel) =
            branch.Levels 
            |> Array.forall (fun x -> (Some level.Id) <> x.ParentId)

        match branch.AllowParallel with
        | Some true ->
            let crownLevels = 
                branch.Levels |> Array.filter isCrownLevel
                |> Array.toList

            let strains = 
                crownLevels 
                |> List.map (fun x -> traverseStrain (Some x) [])

            (strains |> List.map (fun x -> (branch, x))) @ strainPairs
        | _ -> 
            (branch, branch.Levels |> Array.toList) :: strainPairs

    let strainPairs = usedBranches |> Seq.fold firstLoop []

    let secondLoop finalDictionary ((branch: JsonBuildingBranch), (levels:JsonBuildingLevel list)) =
        let levelsOther =
            levels
            |> List.filter (fun x -> 
                Seq.contains x.Id unlockedBuildingLevelIdSeq)
            |> List.map (fun level ->
                { Id = level.Id
                  Name = level.Name
                  LocalEffectSet = (level |> getLocalEffectFromJson)
                  EffectSet = ( level.Effect |> getEffectFromJsonOption_1) }: BuildingLevel)
            |> List.toArray

        match levelsOther with
        | [||] -> finalDictionary
        | _ ->
            { Id = branch.Id
              Name = branch.Name
              Interesting = branch.Interesting = Some true
              Levels = levelsOther }
            :: finalDictionary

    let finalDictionary = strainPairs |> List.fold secondLoop []

    let finalFinalDictionary =
        match descriptor.SlotType with
        | General -> emptyBuildingBranch :: finalDictionary
        | _ -> finalDictionary

    { Descriptor = descriptor
      BuildingBranches = (finalFinalDictionary |> List.toArray) }


let internal getBuildingLibrary buildingsData getUsedBuildingBranchIdSeq getUnlockedBuildingLevelIdSeq (provincesData: ProvincesData.Root []) settings =
    let slotTypes = [ Main; Coastal; General ]
    let regionTypes = [ City; Town ]

    let province =
        query {
            for province in provincesData do
                where (province.Id = settings.ProvinceId)
                head
        }

    let resourceIdsInProvince =
        [province.City.ResourceId; province.TownFirst.ResourceId; province.TownSecond.ResourceId]
        |> List.distinct

    let descriptors =
        slotTypes
        |> List.collect (fun slotType ->
            regionTypes
            |> List.collect (fun regionType ->
                resourceIdsInProvince
                |> List.map (fun resourceId ->
                    { SlotType = slotType
                      RegionType = regionType
                      ResourceId = resourceId })))

    let usedBuildingBranchIdSeq = settings |> getUsedBuildingBranchIdSeq

    let unlockedBuildingLevelIdSeq = settings |> getUnlockedBuildingLevelIdSeq

    let results =
        descriptors
        |> List.map (getBuildingLibraryEntry buildingsData usedBuildingBranchIdSeq unlockedBuildingLevelIdSeq settings)
        |> List.toArray

    results

let internal getBuildingLevel (buildingsData: JsonBuildingBranch []) id =
    let levels = buildingsData |> Seq.collect (fun x -> x.Levels)
    let level =
        query {
            for level in levels do
                where (level.Id = id)
                head
        }

    { Id = level.Id
      Name = level.Name
      LocalEffectSet = (level |> getLocalEffectFromJson)
      EffectSet = (level.Effect |> getEffectFromJsonOption_1) }: BuildingLevel
