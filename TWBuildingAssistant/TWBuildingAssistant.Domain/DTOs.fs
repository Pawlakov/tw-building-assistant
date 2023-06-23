module TWBuildingAssistant.Domain.DTOs

type NamedIdDto =
    { Id:int
      Name:string }

type NamedStringIdDto =
    { StringId:string
      Name:string }

type NamedStringIdWithItemsDto =
    { StringId:string
      Name:string
      Items:NamedStringIdDto[] }

type SettingOptions =
    { Provinces:NamedStringIdDto[]
      Weathers:NamedIdDto[]
      Seasons:NamedIdDto[]
      Religions:NamedStringIdDto[]
      Factions:NamedStringIdDto[]
      Difficulties:NamedIdDto[]
      Taxes:NamedIdDto[]
      PowerLevels:NamedIdDto[] }

type SettingsDto =
    { ProvinceId:string
      FertilityDrop:int 
      TechnologyTier:int 
      UseAntilegacyTechnologies:bool 
      ReligionId:string
      FactionId:string
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
      ResourceId:string option }

type RegionDto =
    { Id:string
      Name:string
      ResourceId:string option
      ResourceName:string option
      Slots:SlotDescriptorDto[] }

type ProvinceDto =
    { Id:string 
      Name:string
      Regions:RegionDto[] }

type BuildingLibraryEntryDto =
    { Descriptor:SlotDescriptorDto
      BuildingBranches:NamedStringIdWithItemsDto[] }

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
    { BranchId:string option
      LevelId:string option
      Descriptor:SlotDescriptorDto
      RegionId:string
      SlotIndex:int }

type SeekerSettingsRegionDto = 
    { Slots:SeekerSettingsSlotDto[] }

type MinimalConditionDto =
    { RequireSanitation:bool 
      RequireFood:bool 
      MinimalPublicOrder:int }

type SeekerResultDto = 
    { BranchId:string
      LevelId:string
      RegionId:string
      SlotIndex:int }

type ResetProgressDelegate = 
    delegate of int -> unit

type IncrementProgressDelegate = 
    delegate of unit -> unit