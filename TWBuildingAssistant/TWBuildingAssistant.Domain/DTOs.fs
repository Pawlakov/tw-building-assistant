module TWBuildingAssistant.Domain.DTOs

type NamedIdDTO =
    { Id:string
      Name:string }

type NamedIdWithItemsDTO =
    { Id:string
      Name:string
      Items:NamedIdDTO[] }

type SettingOptions =
    { Weathers:NamedIdDTO[]
      Seasons:NamedIdDTO[]
      Religions:NamedIdDTO[]
      Factions:NamedIdDTO[]
      Difficulties:NamedIdDTO[]
      Taxes:NamedIdDTO[]
      PowerLevels:NamedIdDTO[] }

type ProvinceOptions =
    { Provinces:NamedIdDTO[] }

type SettingsDTO =
    { ProvinceId:string
      FertilityDrop:int 
      TechnologyTier:int 
      UseAntilegacyTechnologies:bool 
      ReligionId:string
      FactionId:string
      WeatherId:string
      SeasonId:string
      DifficultyId:string
      TaxId:string
      PowerLevelId:string
      CorruptionRate:int
      PiracyRate:int }

type SlotDescriptorDTO =
    { SlotType:int
      RegionType:int
      ResourceId:string option }

type RegionDTO =
    { Id:string
      Name:string
      ResourceId:string option
      ResourceName:string option
      Slots:SlotDescriptorDTO[] }

type ProvinceDTO =
    { Id:string 
      Name:string
      Regions:RegionDTO[] }

type BuildingLibraryEntryDTO =
    { Descriptor:SlotDescriptorDTO
      BuildingBranches:NamedIdWithItemsDTO[] }

type RegionStateDTO =
    { Sanitation:int
      Food:int
      Wealth:float
      Maintenance:float
      CapitalTier:int }

type ProvinceStateDTO =
    { Regions:RegionStateDTO[]
      TotalFood:int
      TotalWealth:float
      TaxRate:int
      CorruptionRate:int
      TotalIncome:float
      PublicOrder:int
      ReligiousOsmosis:int
      ResearchRate:int
      Growth:int }

type SeekerSettingsSlotDTO = 
    { BranchId:string option
      LevelId:string option
      Descriptor:SlotDescriptorDTO
      RegionId:string
      SlotIndex:int }

type SeekerSettingsRegionDTO = 
    { Slots:SeekerSettingsSlotDTO[] }

type MinimalConditionDTO =
    { RequireSanitation:bool 
      RequireFood:bool 
      MinimalPublicOrder:int }

type SeekerResultDTO = 
    { BranchId:string
      LevelId:string
      RegionId:string
      SlotIndex:int }

type ResetProgressDelegate = 
    delegate of int -> unit

type IncrementProgressDelegate = 
    delegate of unit -> unit

let internal mapNamedIdToDTO (model: Settings.NamedId) = { Id = model.Id; Name = model.Name }

let internal mapOptionSetToDTO (model: Settings.OptionSet) =
    { Weathers =
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
        |> List.map mapNamedIdToDTO
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

let internal mapProvinceOptionSetToDTO (model: Settings.ProvinceOptionSet) =
    { Provinces =
        model.Provinces
        |> List.map mapNamedIdToDTO
        |> List.toArray }

let internal mapSlotToDTO (model: Provinces.SlotDescriptor) =
    let mapSlotType slotType =
        match slotType with
        | Provinces.Main -> 0
        | Provinces.Coastal -> 1
        | Provinces.General -> 2

    let mapRegionType regionType =
        match regionType with
        | Provinces.City -> 0
        | Provinces.Town -> 1

    { SlotType = model.SlotType |> mapSlotType
      RegionType = model.RegionType |> mapRegionType
      ResourceId = model.ResourceId }

let internal mapRegionToDTO (model: Provinces.Region) =
    { Id = model.Id
      Name = model.Name
      ResourceId = model.ResourceId
      ResourceName = model.ResourceName
      Slots = model.Slots |> Array.map mapSlotToDTO }

let internal mapProvinceToDTO (model: Provinces.Province) =
    { Id = model.Id
      Name = model.Name
      Regions = model.Regions |> Array.map mapRegionToDTO }

let internal mapBuildingLevelToDTO (model: Buildings.BuildingLevel) = { Id = model.Id; Name = model.Name }

let internal mapBuildingBranchToDTO (model: Buildings.BuildingBranch) =
    { Id = model.Id
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
        | 0 -> Provinces.Main
        | 1 -> Provinces.Coastal
        | 2 -> Provinces.General
        | _ -> failwith ""

    let mapRegionType regionType =
        match regionType with
        | 0 -> Provinces.City
        | 1 -> Provinces.Town
        | _ -> failwith ""

    { SlotType = dto.SlotType |> mapSlotType
      RegionType = dto.RegionType |> mapRegionType
      ResourceId = dto.ResourceId }: Provinces.SlotDescriptor

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