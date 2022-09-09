module TWBuildingAssistant.Domain.Interface

open DTOs
open TWBuildingAssistant.Data.Sqlite

let internal mapNamedIdToDTO (model:Settings.NamedId) =
    { Id = model.Id; Name = model.Name}

let internal mapOptionSetToDTO (model:Settings.OptionSet) =
    { Provinces = model.Provinces |> List.map mapNamedIdToDTO |> List.toArray
      Weathers = model.Weathers |> List.map mapNamedIdToDTO |> List.toArray
      Seasons = model.Seasons |> List.map mapNamedIdToDTO |> List.toArray
      Religions = model.Religions |> List.map mapNamedIdToDTO |> List.toArray
      Factions = model.Factions |> List.map mapNamedIdToDTO |> List.toArray
      Difficulties = model.Difficulties |> List.map mapNamedIdToDTO |> List.toArray
      Taxes = model.Taxes |> List.map mapNamedIdToDTO |> List.toArray
      PowerLevels = model.PowerLevels |> List.map mapNamedIdToDTO |> List.toArray }

let internal mapSlotToDTO (model:Province.SlotDescriptor) =
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

let internal mapRegionToDTO (model:Province.Region) =
    { Id = model.Id
      Name = model.Name
      ResourceId = model.ResourceId
      ResourceName = model.ResourceName
      Slots = model.Slots |> Array.map mapSlotToDTO }

let internal mapProvinceToDTO (model:Province.Province) =
    { Id = model.Id
      Name = model.Name 
      Regions = model.Regions |> Array.map mapRegionToDTO }

let internal mapBuildingLevelToDTO (model:Buildings.BuildingLevel) =
    { Id = model.Id
      Name = model.Name }

let internal mapBuildingBranchToDTO (model:Buildings.BuildingBranch) =
    { Id = model.Id
      Name = model.Name 
      Items = model.Levels |> Array.map mapBuildingLevelToDTO}

let internal mapBuildingLibraryEntryToDTO (model:Buildings.BuildingLibraryEntry) =
    { Descriptor = model.Descriptor |> mapSlotToDTO
      BuildingBranches = model.BuildingBranches |> Array.map mapBuildingBranchToDTO }

let internal mapRegionStateToDTO (model:State.RegionState) =
    { Sanitation = model.Sanitation
      Food = model.Food
      Wealth = model.Wealth
      Maintenance = model.Maintenance }

let internal mapProvinceStateToDTO (model:State.ProvinceState) =
    { Regions = model.Regions |> Array.map mapRegionStateToDTO
      TotalFood = model.TotalFood
      TotalWealth = model.TotalWealth
      TaxRate = model.TaxRate
      CorruptionRate = model.CorruptionRate
      TotalIncome = model.TotalIncome
      PublicOrder = model.PublicOrder
      ReligiousOsmosis = model.ReligiousOsmosis
      ResearchRate = model.ResearchRate
      Growth = model.Growth }

let internal mapSeekerResultToDTO (seekerResult:Seeker.SeekerResult) =
    { BranchId = seekerResult.Branch.Id
      LevelId = seekerResult.Level.Id
      RegionId = seekerResult.RegionId
      SlotIndex = seekerResult.SlotIndex }

let internal mapSettingsFromDTO dto =
    { ProvinceId = dto.ProvinceId
      FertilityDrop = dto.FertilityDrop
      TechnologyTier = dto.TechnologyTier
      UseAntilegacyTechnologies = dto.UseAntilegacyTechnologies 
      ReligionId = dto.ReligionId
      FactionId = dto.FactionId
      WeatherId = dto.WeatherId
      SeasonId = dto.SeasonId
      DifficultyId = dto.DifficultyId
      TaxId = dto.TaxId
      PowerLevelId = dto.PowerLevelId
      CorruptionRate = dto.CorruptionRate
      PiracyRate = dto.PiracyRate }:Settings.Settings

let internal mapSlotFromDTO dto =
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

    { SlotType = dto.SlotType |> mapSlotType
      RegionType = dto.RegionType |> mapRegionType
      ResourceId = dto.ResourceId }:Province.SlotDescriptor

let internal mapSeekerSettingsSlotFromDTO (buildingLibrary:Buildings.BuildingLibraryEntry[]) seekerSettingsSlot =
    let descriptor = seekerSettingsSlot.Descriptor |> mapSlotFromDTO
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

let getSettingOptions (ctx:DatabaseContext) =
    let options =
        Settings.getOptions ctx

    options |> mapOptionSetToDTO

let getProvince (ctx:DatabaseContext) provinceId =
    let province = Province.getProvince ctx provinceId

    province |> mapProvinceToDTO

let getBuildingLibrary (ctx:DatabaseContext) settings =
    let settingsModel =
        settings |> mapSettingsFromDTO

    let buildingsModels = 
        Buildings.getBuildingLibrary ctx settingsModel

    buildingsModels |> Array.map mapBuildingLibraryEntryToDTO

let getState (ctx:DatabaseContext) buildingLevelIds settings =
    let settingsModel =
        settings |> mapSettingsFromDTO
    let predefinedEffectSet = 
        Effects.getStateFromSettings ctx settingsModel
    let buildings =
        buildingLevelIds
        |> Array.map (fun region -> region |> Array.filter (fun x -> x <> 0) |> Array.map (Buildings.getBuildingLevel ctx) |> Array.toSeq)
        |> Array.toSeq

    let state = 
        State.getState buildings settingsModel predefinedEffectSet

    state |> mapProvinceStateToDTO

let seek(ctx:DatabaseContext) settings seekerSettings minimalCondition (resetProgress:ResetProgressDelegate) (incrementProgress:IncrementProgressDelegate) =
    let settingsModel =
        settings |> mapSettingsFromDTO
    let predefinedEffectSet = 
        Effects.getStateFromSettings ctx settingsModel
    let buildingLibrary = 
        Buildings.getBuildingLibrary ctx settingsModel
    let seekerSettingsModel =
        seekerSettings |> Array.map (fun x -> { Slots = x.Slots |> (Array.map (mapSeekerSettingsSlotFromDTO buildingLibrary)) }:Seeker.SeekerSettingsRegion )
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

    seekerResults
    |> Array.map mapSeekerResultToDTO