module TWBuildingAssistant.Domain.Models

type NamedId = 
    { Id:int
      Name:string }

type OptionSet =
    { Provinces:NamedId list
      Weathers:NamedId list
      Seasons:NamedId list
      Religions:NamedId list
      Factions:NamedId list
      Difficulties:NamedId list
      Taxes:NamedId list }

type Effect =
    { PublicOrder:int
      RegularFood:int
      FertilityDependentFood:int
      ProvincialSanitation:int
      ResearchRate:int
      Growth:int
      Fertility:int
      ReligiousOsmosis:int
      RegionalSanitation:int }

type IncomeCategory =
    | Agriculture
    | Husbandry
    | Culture
    | Industry
    | LocalCommerce
    | MaritimeCommerce
    | Subsistence
    | Maintenance

type IncomeType =
    | Simple
    | Percentage
    | FertilityDependent

type Income =
    { Category:IncomeCategory option
      Simple:int
      Percentage:int
      FertilityDependent:int }

type Influence =
    { ReligionId:int option
      Value:int }

type EffectSet =
    { Effect:Effect
      Incomes:Income list
      Influences:Influence list }

type Settings =
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
      CorruptionRate:int
      PiracyRate:int }

type RegionType =
    | City
    | Town

type SlotType =
    | Main
    | Coastal
    | General

type SlotDescriptor =
    { SlotType:SlotType
      RegionType:RegionType
      ResourceId:int option }

type Region =
    { Id:int
      Name:string
      RegionType:RegionType
      ResourceId:int option
      ResourceName: string option
      Slots:SlotDescriptor[] }

type Province =
    { Id:int 
      Name:string
      Regions:Region[] }

type BuildingLevel =
    { Id:int
      Name:string
      EffectSet:EffectSet }

type BuildingBranch =
    { Id:int
      Name:string
      Interesting:bool
      Levels:BuildingLevel[] }

type BuildingLibraryEntry =
    { Descriptor:SlotDescriptor
      BuildingBranches:BuildingBranch[] }

type RegionState = 
    { Sanitation:int }

type ProvinceState = 
    { Regions:RegionState[]
      Food:int
      PublicOrder:int
      ReligiousOsmosis:int
      ResearchRate:int
      Growth:int
      Wealth:float }

type CalculationSlot =
    { Descriptor:SlotDescriptor
      Branch:BuildingBranch option
      Level:BuildingLevel option
      RegionId:int
      SlotIndex:int }

type CombinationTaskRegion = 
    { Slots:CalculationSlot[] }

type CombinationTask = 
    { Regions:CombinationTaskRegion[] }

type SeekerSettingsSlot = 
    { Branch:BuildingBranch option
      Level:BuildingLevel option
      Descriptor:SlotDescriptor
      RegionId:int
      SlotIndex:int }

type SeekerSettingsRegion = 
    { Slots:SeekerSettingsSlot[] }

type SeekerResult = 
    { Branch:BuildingBranch
      Level:BuildingLevel
      RegionId:int
      SlotIndex:int }

type SeekerResultWithWealth =
    { Wealth:double
      Result:SeekerResult[] }

let emptyEffect =
    { PublicOrder = 0; RegularFood = 0; FertilityDependentFood = 0; ProvincialSanitation = 0; ResearchRate = 0; Growth = 0; Fertility = 0; ReligiousOsmosis = 0; RegionalSanitation = 0 }

let emptyEffectSet =
    { Effect = emptyEffect; Incomes = []; Influences = [] }

let emptyBuildingLevel =
    { Id = 0; Name = "Empty"; EffectSet = emptyEffectSet }

let emptyBuildingBranch =
    { Id = 0; Name = "Empty"; Interesting = true; Levels = [|emptyBuildingLevel|]}