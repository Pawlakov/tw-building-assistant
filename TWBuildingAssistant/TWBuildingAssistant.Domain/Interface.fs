module TWBuildingAssistant.Domain.Interface

let getProvinceOptions () =
    let getProvinceTupleSeq = Provinces.Data.getProvincesData >> Provinces.getProvincePairSeq

    let options =
        Settings.getProvinceOptions getProvinceTupleSeq

    options |> DTOs.mapProvinceOptionSetToDTO

let getSettingOptions () =
    let getWeatherTupleSeq = Weathers.Data.getWeathersData >> Weathers.getWeatherPairSeq
    let getSeasonTupleSeq = Seasons.Data.getSeasonsData >> Seasons.getSeasonPairSeq
    let getReligionTupleSeq = Religions.Data.getReligionsData >> Religions.getReligionPairSeq
    let getDifficultyTupleSeq = Difficulties.Data.getDifficultiesData >> Difficulties.getDifficultyPairSeq
    let getTaxTupleSeq = Taxes.Data.getTaxesData >> Taxes.getTaxPairSeq
    let getPowerLevelTupleSeqa = PowerLevels.Data.getPowerLevelsData >> PowerLevels.getPowerLevelPairSeq
    let getFactionTupleSeq = Factions.Data.getFactionsData >> Factions.getFactionPairSeq

    let options =
        Settings.getOptions
            getWeatherTupleSeq
            getSeasonTupleSeq
            getReligionTupleSeq
            getDifficultyTupleSeq
            getTaxTupleSeq
            getPowerLevelTupleSeqa
            getFactionTupleSeq

    options |> DTOs.mapOptionSetToDTO

let getProvince provinceId =
    let resourcesData = Resources.Data.getResourcesData ()
    let provincesData = Provinces.Data.getProvincesData ()

    let province = 
        Provinces.getProvince
            provincesData 
            (resourcesData |> Resources.getResourcesByIds)
            provinceId

    province |> DTOs.mapProvinceToDTO

let getBuildingLibrary settings =
    let provincesData = Provinces.Data.getProvincesData ()
    let factionsData = Factions.Data.getFactionsData ()
    let buildingsData = Buildings.Data.getBuildingsData ()

    let settingsModel = settings |> DTOs.mapSettingsFromDTO

    let buildingsModels = 
        Buildings.getBuildingLibrary 
            buildingsData 
            (factionsData |> Factions.getUsedBuildingBranchIds) 
            (factionsData |> Factions.getUnlockedBuildingLevelIds)  
            (provincesData |> Provinces.getDescriptors)
            settingsModel

    buildingsModels
    |> Array.map DTOs.mapBuildingLibraryEntryToDTO

let getState buildingLevelIds settings =
    let climatesData = Climates.Data.getClimatesData ()
    let provincesData = Provinces.Data.getProvincesData ()
    let wondersData = Wonders.Data.getWondersData ()
    let religionsData = Religions.Data.getReligionsData ()
    let difficultiesData = Difficulties.Data.getDifficultiesData ()
    let taxesData = Taxes.Data.getTaxesData ()
    let powerLevelsData = PowerLevels.Data.getPowerLevelsData ()
    let factionsData = Factions.Data.getFactionsData ()
    let buildingsData = Buildings.Data.getBuildingsData ()

    let settingsModel = settings |> DTOs.mapSettingsFromDTO

    let predefinedEffectSet =
        Effects.getStateFromSettings
            (Climates.getClimateEffect climatesData (Provinces.getProvinceClimateId provincesData))
            (Provinces.getProvinceEffect provincesData)
            (Wonders.getWonderEffectSeq wondersData (Provinces.getProvinceRegionIdSeq provincesData))
            (Religions.getReligionEffect religionsData)
            (Difficulties.getDifficultyEffect difficultiesData)
            (Taxes.getTaxEffect taxesData)
            (PowerLevels.getPowerLevelEffect powerLevelsData)
            (Factions.getFactionEffect factionsData)
            (Factions.getTechnologyEffects factionsData)
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

    state |> DTOs.mapProvinceStateToDTO

let seek settings (seekerSettings: DTOs.SeekerSettingsRegionDTO []) (minimalCondition: DTOs.MinimalConditionDTO) (resetProgress: DTOs.ResetProgressDelegate) (incrementProgress: DTOs.IncrementProgressDelegate) =
    let climatesData = Climates.Data.getClimatesData ()
    let provincesData = Provinces.Data.getProvincesData ()
    let wondersData = Wonders.Data.getWondersData ()
    let religionsData = Religions.Data.getReligionsData ()
    let difficultiesData = Difficulties.Data.getDifficultiesData ()
    let taxesData = Taxes.Data.getTaxesData ()
    let powerLevelsData = PowerLevels.Data.getPowerLevelsData ()
    let factionsData = Factions.Data.getFactionsData ()
    let buildingsData = Buildings.Data.getBuildingsData ()

    let settingsModel = settings |> DTOs.mapSettingsFromDTO

    let predefinedEffectSet =
        Effects.getStateFromSettings
            (Climates.getClimateEffect climatesData (Provinces.getProvinceClimateId provincesData))
            (Provinces.getProvinceEffect provincesData)
            (Wonders.getWonderEffectSeq wondersData (Provinces.getProvinceRegionIdSeq provincesData))
            (Religions.getReligionEffect religionsData)
            (Difficulties.getDifficultyEffect difficultiesData)
            (Taxes.getTaxEffect taxesData)
            (PowerLevels.getPowerLevelEffect powerLevelsData)
            (Factions.getFactionEffect factionsData)
            (Factions.getTechnologyEffects factionsData)
            settingsModel

    let buildingLibrary = 
        Buildings.getBuildingLibrary 
            buildingsData 
            (factionsData |> Factions.getUsedBuildingBranchIds) 
            (factionsData |> Factions.getUnlockedBuildingLevelIds) 
            (provincesData |> Provinces.getDescriptors)
            settingsModel

    let seekerSettingsModel =
        seekerSettings
        |> Array.map (fun x -> { Slots = x.Slots |> (Array.map (DTOs.mapSeekerSettingsSlotFromDTO buildingLibrary)) }: Seeker.SeekerSettingsRegion)

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

    seekerResults |> Array.map DTOs.mapSeekerResultToDTO
