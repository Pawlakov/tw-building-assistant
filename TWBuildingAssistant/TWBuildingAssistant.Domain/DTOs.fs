module TWBuildingAssistant.Domain.DTOs

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

type BuildingLibraryEntryDto =
    { Descriptor:SlotDescriptorDto
      BuildingBranches:NamedIdWithItemsDto[] }

type RegionStateDto =
    { Sanitation:int
      Food:int
      Wealth:float
      Maintenance:float
      CapitalTier:int }

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

type ResetProgressDelegate = 
    delegate of int -> unit

type IncrementProgressDelegate = 
    delegate of unit -> unit