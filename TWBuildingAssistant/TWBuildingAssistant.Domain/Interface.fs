module TWBuildingAssistant.Domain.Interface

open TWBuildingAssistant.Data.Sqlite
open TWBuildingAssistant.Domain.Factions
open DTOs
open Data
open System.IO

let internal mapNamedIdToDTO (model: Settings.NamedId) = { Id = model.Id; Name = model.Name }

let internal mapNamedStringIdToDTO (model: Settings.NamedStringId) = { StringId = model.StringId; Name = model.Name }

let internal mapOptionSetToDTO (model: Settings.OptionSet) =
    { Provinces =
        model.Provinces
        |> List.map mapNamedIdToDTO
        |> List.toArray
      Weathers =
        model.Weathers
        |> List.map mapNamedIdToDTO
        |> List.toArray
      Seasons =
        model.Seasons
        |> List.map mapNamedIdToDTO
        |> List.toArray
      Religions =
        model.Religions
        |> List.map mapNamedIdToDTO
        |> List.toArray
      Factions =
        model.Factions
        |> List.map mapNamedStringIdToDTO
        |> List.toArray
      Difficulties =
        model.Difficulties
        |> List.map mapNamedIdToDTO
        |> List.toArray
      Taxes =
        model.Taxes
        |> List.map mapNamedIdToDTO
        |> List.toArray
      PowerLevels =
        model.PowerLevels
        |> List.map mapNamedIdToDTO
        |> List.toArray }

let internal mapSlotToDTO (model: Province.SlotDescriptor) =
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

let internal mapRegionToDTO (model: Province.Region) =
    { Id = model.Id
      Name = model.Name
      ResourceId = model.ResourceId
      ResourceName = model.ResourceName
      Slots = model.Slots |> Array.map mapSlotToDTO }

let internal mapProvinceToDTO (model: Province.Province) =
    { Id = model.Id
      Name = model.Name
      Regions = model.Regions |> Array.map mapRegionToDTO }

let internal mapBuildingLevelToDTO (model: Buildings.BuildingLevel) = { StringId = model.Id; Name = model.Name }

let internal mapBuildingBranchToDTO (model: Buildings.BuildingBranch) =
    { StringId = model.Id
      Name = model.Name
      Items = model.Levels |> Array.map mapBuildingLevelToDTO }

let internal mapBuildingLibraryEntryToDTO (model: Buildings.BuildingLibraryEntry) =
    { Descriptor = model.Descriptor |> mapSlotToDTO
      BuildingBranches =
        model.BuildingBranches
        |> Array.map mapBuildingBranchToDTO }

let internal mapRegionStateToDTO (model: State.RegionState) =
    { Sanitation = model.Sanitation
      Food = model.Food
      Wealth = model.Wealth
      Maintenance = model.Maintenance
      CapitalTier = model.CapitalTier }

let internal mapProvinceStateToDTO (model: State.ProvinceState) =
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

let internal mapSeekerResultToDTO (seekerResult: Seeker.SeekerResult) =
    { BranchId = seekerResult.Branch.Id
      LevelId = seekerResult.Level.Id
      RegionId = seekerResult.RegionId
      SlotIndex = seekerResult.SlotIndex }

let internal mapSettingsFromDTO dto : Settings.Settings =
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
      PiracyRate = dto.PiracyRate }

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
      ResourceId = dto.ResourceId }: Province.SlotDescriptor

let internal mapSeekerSettingsSlotFromDTO (buildingLibrary: Buildings.BuildingLibraryEntry []) seekerSettingsSlot =
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
            |> Array.find (fun x ->
                x.Id = branchId
                && (x.Levels |> Array.exists (fun y -> y.Id = levelId)))
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
      SlotIndex = seekerSettingsSlot.SlotIndex }: Seeker.SeekerSettingsSlot

let internal getWeathersData () =
    "Data/Weathers.json"
    |> File.ReadAllText
    |> WeathersData.ParseList

let internal getSeasonsData () =
    "Data/Seasons.json"
    |> File.ReadAllText
    |> SeasonsData.ParseList

let internal getClimatesData () =
    "Data/Climates.json"
    |> File.ReadAllText
    |> ClimatesData.ParseList

let internal getResourcesData () =
    "Data/Resources.json"
    |> File.ReadAllText
    |> ResourcesData.ParseList

let internal getProvincesData () =
    "Data/Provinces.json"
    |> File.ReadAllText
    |> ProvincesData.ParseList

let internal getReligionsData () =
    "Data/Religions.json"
    |> File.ReadAllText
    |> ReligionsData.ParseList

let internal getDifficultiesData () =
    "Data/Difficulties.json"
    |> File.ReadAllText
    |> DifficultiesData.ParseList

let internal getTaxesData () =
    "Data/Taxes.json"
    |> File.ReadAllText
    |> TaxesData.ParseList

let internal getPowerLevelsData () =
    "Data/PowerLevels.json"
    |> File.ReadAllText
    |> PowerLevelsData.ParseList

let internal getBuildingsData () =
    let all = ("Data/BuildingsAll.json" |> File.ReadAllText |> BuildingsData.Parse).Branches
    let resource = ("Data/BuildingsResource.json" |> File.ReadAllText |> BuildingsData.Parse).Branches
    let religion = ("Data/BuildingsReligion.json" |> File.ReadAllText |> BuildingsData.Parse).Branches
    let roman = ("Data/BuildingsRoman.json" |> File.ReadAllText |> BuildingsData.Parse).Branches
    let romanWest = ("Data/BuildingsRomanWest.json" |> File.ReadAllText |> BuildingsData.Parse).Branches

    Array.concat [all; resource; religion; roman; romanWest]

let getSettingOptions () =
    let weathersData = getWeathersData ()
    let seasonsData = getSeasonsData ()
    let provincesData = getProvincesData ()
    let religionsData = getReligionsData ()
    let difficultiesData = getDifficultiesData ()
    let taxesData = getTaxesData ()
    let powerLevelsData = getPowerLevelsData ()
    let getFactionTupleSeq = FactionsData.getFactionsData >> Factions.getFactionPairs

    let options =
        Settings.getOptions
            weathersData
            seasonsData
            provincesData
            religionsData
            difficultiesData
            taxesData
            powerLevelsData
            getFactionTupleSeq

    options |> mapOptionSetToDTO

let getProvince provinceId =
    let resourcesData = getResourcesData ()
    let provincesData = getProvincesData ()

    let province = Province.getProvince provincesData resourcesData provinceId

    province |> mapProvinceToDTO

let getBuildingLibrary settings =
    let provincesData = getProvincesData ()
    let factionsData = FactionsData.getFactionsData ()
    let buildingsData = getBuildingsData ()

    let settingsModel = settings |> mapSettingsFromDTO

    let buildingsModels = Buildings.getBuildingLibrary buildingsData factionsData provincesData settingsModel

    buildingsModels
    |> Array.map mapBuildingLibraryEntryToDTO

let getState ctx buildingLevelIds settings =
    let climatesData = getClimatesData ()
    let provincesData = getProvincesData ()
    let religionsData = getReligionsData ()
    let difficultiesData = getDifficultiesData ()
    let taxesData = getTaxesData ()
    let powerLevelsData = getPowerLevelsData ()
    let factionsData = FactionsData.getFactionsData ()
    let buildingsData = getBuildingsData ()

    let settingsModel = settings |> mapSettingsFromDTO

    let predefinedEffectSet =
        Effects.getStateFromSettings
            ctx
            climatesData
            provincesData
            religionsData
            difficultiesData
            taxesData
            powerLevelsData
            (factionsData |> Factions.getFactionEffects)
            (factionsData |> Factions.getTechnologyEffects)
            settingsModel

    let buildings =
        buildingLevelIds
        |> Array.map (fun region ->
            region
            |> Array.filter (fun x -> x <> "")
            |> Array.map (Buildings.getBuildingLevel buildingsData)
            |> Array.toSeq)
        |> Array.toSeq

    let state = State.getState buildings settingsModel predefinedEffectSet

    state |> mapProvinceStateToDTO

let seek
    (ctx: DatabaseContext)
    settings
    seekerSettings
    minimalCondition
    (resetProgress: ResetProgressDelegate)
    (incrementProgress: IncrementProgressDelegate)
    =
    let climatesData = getClimatesData ()
    let provincesData = getProvincesData ()
    let religionsData = getReligionsData ()
    let difficultiesData = getDifficultiesData ()
    let taxesData = getTaxesData ()
    let powerLevelsData = getPowerLevelsData ()
    let factionsData = FactionsData.getFactionsData ()
    let buildingsData = getBuildingsData ()

    let settingsModel = settings |> mapSettingsFromDTO

    let predefinedEffectSet =
        Effects.getStateFromSettings
            ctx
            climatesData
            provincesData
            religionsData
            difficultiesData
            taxesData
            powerLevelsData
            (factionsData |> Factions.getFactionEffects)
            (factionsData |> Factions.getTechnologyEffects)
            settingsModel

    let buildingLibrary = Buildings.getBuildingLibrary buildingsData factionsData provincesData settingsModel

    let seekerSettingsModel =
        seekerSettings
        |> Array.map (fun x ->
            { Slots =
                x.Slots
                |> (Array.map (mapSeekerSettingsSlotFromDTO buildingLibrary)) }: Seeker.SeekerSettingsRegion)

    let minimalConditionFun (state: State.ProvinceState) =
        if minimalCondition.RequireFood
           && state.TotalFood < 0 then
            false
        elif minimalCondition.RequireSanitation
             && (state.Regions
                 |> Array.exists (fun x -> x.Sanitation < 0)) then
            false
        elif minimalCondition.MinimalPublicOrder > state.PublicOrder then
            false
        else
            true

    let seekerResults =
        Seeker.seek
            settingsModel
            predefinedEffectSet
            buildingLibrary
            seekerSettingsModel
            minimalConditionFun
            (fun x -> resetProgress.Invoke x)
            (fun () -> incrementProgress.Invoke())

    seekerResults |> Array.map mapSeekerResultToDTO
