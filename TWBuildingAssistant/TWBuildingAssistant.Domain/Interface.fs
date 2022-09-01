module TWBuildingAssistant.Domain.Interface

type NamedIdDto =
    { Id:int
      Name:string }

type NamedIdWithItemsDto =
    { Id:int
      Name:string
      Items:NamedIdDto[] }

type SettingOptions =
    { Provinces:NamedIdDto[]
      Weathers:NamedIdDto[]
      Seasons:NamedIdDto[]
      Religions:NamedIdDto[]
      Factions:NamedIdDto[]
      Difficulties:NamedIdDto[]
      Taxes:NamedIdDto[]
      PowerLevels:NamedIdDto[] }

let getSettingOptions () =
    let mapNamedId (model:Settings.NamedId) =
        { Id = model.Id; Name = model.Name}

    let options =
        Settings.getOptions ()

    { Provinces = options.Provinces |> List.map mapNamedId |> List.toArray
      Weathers = options.Weathers |> List.map mapNamedId |> List.toArray
      Seasons = options.Seasons |> List.map mapNamedId |> List.toArray
      Religions = options.Religions |> List.map mapNamedId |> List.toArray
      Factions = options.Factions |> List.map mapNamedId |> List.toArray
      Difficulties = options.Difficulties |> List.map mapNamedId |> List.toArray
      Taxes = options.Taxes |> List.map mapNamedId |> List.toArray
      PowerLevels = options.PowerLevels |> List.map mapNamedId |> List.toArray }

type SettingsDto =
    { ProvinceId:int
      FertilityDrop:int 
      TechnologyTier:int 
      UseAntilegacyTechnologies:bool 
      ReligionId:int
      FactionId:int
      WeatherId:int
      SeasonId:int
      DifficultyId:int
      TaxId:int
      PowerLevelId:int
      CorruptionRate:int
      PiracyRate:int }

type SlotDescriptorDto =
    { SlotType:int
      RegionType:int
      ResourceId:int option }

type RegionDto =
    { Id:int
      Name:string
      ResourceId:int option
      ResourceName: string option
      Slots:SlotDescriptorDto[] }

type ProvinceDto =
    { Id:int 
      Name:string
      Regions:RegionDto[] }

let internal mapSlot (model:Province.SlotDescriptor) =
    let mapSlotType slotType =
        match slotType with
        | Province.Main -> 0
        | Province.Coastal -> 1
        | Province.General -> 2

    let mapRegionType regionType =
        match regionType with
        | Province.City -> 0
        | Province.Town -> 1

    { SlotType = model.SlotType |> mapSlotType
      RegionType = model.RegionType |> mapRegionType
      ResourceId = model.ResourceId }

let getProvince provinceId =
    let mapRegion (model:Province.Region) =
        { Id = model.Id
          Name = model.Name
          ResourceId = model.ResourceId
          ResourceName = model.ResourceName
          Slots = model.Slots |> Array.map mapSlot }

    let province = Province.getProvince provinceId

    { Id = province.Id
      Name = province.Name 
      Regions = province.Regions |> Array.map mapRegion }

type BuildingLibraryEntryDto =
    { Descriptor:SlotDescriptorDto
      BuildingBranches:NamedIdWithItemsDto[] }

let internal mapSettings settings =
    { ProvinceId = settings.ProvinceId
      FertilityDrop = settings.FertilityDrop
      TechnologyTier = settings.TechnologyTier
      UseAntilegacyTechnologies = settings.UseAntilegacyTechnologies 
      ReligionId = settings.ReligionId
      FactionId = settings.FactionId
      WeatherId = settings.WeatherId
      SeasonId = settings.SeasonId
      DifficultyId = settings.DifficultyId
      TaxId = settings.TaxId
      PowerLevelId = settings.PowerLevelId
      CorruptionRate = settings.CorruptionRate
      PiracyRate = settings.PiracyRate }:Settings.Settings

let getBuildingLibrary settings =
    let mapBuildingLibraryEntry (buildingLibraryEntry:Buildings.BuildingLibraryEntry) =
        let mapBuildingBranch (buildingBranch:Buildings.BuildingBranch) =
            let mapBuildingLevel (buildingLevel:Buildings.BuildingLevel) =
                { Id = buildingLevel.Id
                  Name = buildingLevel.Name }

            { Id = buildingBranch.Id
              Name = buildingBranch.Name 
              Items = buildingBranch.Levels |> Array.map mapBuildingLevel}

        { Descriptor = buildingLibraryEntry.Descriptor |> mapSlot
          BuildingBranches = buildingLibraryEntry.BuildingBranches |> Array.map mapBuildingBranch }

    let settingsModel =
        mapSettings settings

    let buildingsModels = 
        Buildings.getBuildingLibrary settingsModel

    buildingsModels |> Array.map mapBuildingLibraryEntry

type RegionStateDto =
    { Sanitation:int
      Food:int
      Wealth:float
      Maintenance:float }

type ProvinceStateDto =
    { Regions:RegionStateDto[]
      TotalFood:int
      TotalWealth:float
      TaxRate:int
      CorruptionRate:int
      TotalIncome:float
      PublicOrder:int
      ReligiousOsmosis:int
      ResearchRate:int
      Growth:int }

let getState buildingLevelIds settings =
    let mapRegionState (regionState:State.RegionState) =
        { Sanitation = regionState.Sanitation
          Food = regionState.Food
          Wealth = regionState.Wealth
          Maintenance = regionState.Maintenance }

    let settingsModel =
        mapSettings settings
    let predefinedEffectSet = 
        Effects.getStateFromSettings settingsModel
    let buildings =
        buildingLevelIds
        |> Array.map (fun region -> region |> Array.filter (fun x -> x <> 0) |> Array.map Buildings.getBuildingLevel |> Array.toSeq)
        |> Array.toSeq

    let state = 
        State.getState buildings settingsModel predefinedEffectSet

    { Regions = state.Regions |> Array.map mapRegionState
      TotalFood = state.TotalFood
      TotalWealth = state.TotalWealth
      TaxRate = state.TaxRate
      CorruptionRate = state.CorruptionRate
      TotalIncome = state.TotalIncome
      PublicOrder = state.PublicOrder
      ReligiousOsmosis = state.ReligiousOsmosis
      ResearchRate = state.ResearchRate
      Growth = state.Growth }

type SeekerSettingsSlotDto = 
    { BranchId:int option
      LevelId:int option
      Descriptor:SlotDescriptorDto
      RegionId:int
      SlotIndex:int }

type SeekerSettingsRegionDto = 
    { Slots:SeekerSettingsSlotDto[] }

type MinimalConditionDto =
    { RequireSanitation:bool 
      RequireFood:bool 
      MinimalPublicOrder:int }

type SeekerResultDto = 
    { BranchId:int
      LevelId:int
      RegionId:int
      SlotIndex:int }

type ResetProgressDelegate = delegate of int -> unit
type IncrementProgressDelegate = delegate of unit -> unit
let seek settings (seekerSettings:SeekerSettingsRegionDto[]) (minimalCondition:MinimalConditionDto) (resetProgress:ResetProgressDelegate) (incrementProgress:IncrementProgressDelegate) =
    let mapSlot slotDescriptor =
        let mapSlotType slotType =
            match slotType with
            | 0 -> Province.Main
            | 1 -> Province.Coastal
            | 2 -> Province.General
            | _ -> failwith ""

        let mapRegionType regionType =
            match regionType with
            | 0 -> Province.City
            | 1 -> Province.Town
            | _ -> failwith ""

        { SlotType = slotDescriptor.SlotType |> mapSlotType
          RegionType = slotDescriptor.RegionType |> mapRegionType
          ResourceId = slotDescriptor.ResourceId }:Province.SlotDescriptor
    
    let mapSeekerSettingsSlot (buildingLibrary:Buildings.BuildingLibraryEntry[]) seekerSettingsSlot =
        let descriptor = seekerSettingsSlot.Descriptor |> mapSlot
        let libraryEntry = 
            buildingLibrary
            |> Array.find (fun x -> x.Descriptor = descriptor)
        let branch =
            match seekerSettingsSlot.BranchId, seekerSettingsSlot.LevelId with
            | None, _ -> None
            | Some branchId, None -> 
                libraryEntry.BuildingBranches
                |> Array.find (fun x -> x.Id = branchId)
                |> Some
            | Some branchId, Some levelId ->
                libraryEntry.BuildingBranches
                |> Array.find (fun x -> x.Id = branchId && (x.Levels |> Array.exists (fun y -> y.Id = levelId)))
                |> Some
        let level =
            match branch, seekerSettingsSlot.LevelId with
            | None, _ -> None
            | _, None -> None
            | Some branch, Some levelId ->
                branch.Levels 
                |> Array.find (fun x -> x.Id = levelId)
                |> Some

        { Branch = branch
          Level = level
          Descriptor = descriptor
          RegionId = seekerSettingsSlot.RegionId
          SlotIndex = seekerSettingsSlot.SlotIndex }:Seeker.SeekerSettingsSlot
    
    let settingsModel =
        mapSettings settings
    let predefinedEffectSet = 
        Effects.getStateFromSettings settingsModel
    let buildingLibrary = 
        Buildings.getBuildingLibrary settingsModel
    let seekerSettingsModel =
        seekerSettings |> Array.map (fun x -> { Slots = x.Slots |> (Array.map (mapSeekerSettingsSlot buildingLibrary)) }:Seeker.SeekerSettingsRegion )
    let minimalConditionFun (state:State.ProvinceState) =
        if minimalCondition.RequireFood && state.TotalFood < 0 then
            false
        elif minimalCondition.RequireSanitation && (state.Regions |> Array.exists (fun x -> x.Sanitation < 0)) then
            false
        elif minimalCondition.MinimalPublicOrder > state.PublicOrder then
            false
        else 
            true

    let seekerResults =
        Seeker.seek settingsModel predefinedEffectSet buildingLibrary seekerSettingsModel minimalConditionFun (fun x -> resetProgress.Invoke x) (fun () -> incrementProgress.Invoke ())

    let mapSeekerResult (seekerResult:Seeker.SeekerResult) =
        { BranchId = seekerResult.Branch.Id
          LevelId = seekerResult.Level.Id
          RegionId = seekerResult.RegionId
          SlotIndex = seekerResult.SlotIndex }

    seekerResults
    |> Array.map mapSeekerResult