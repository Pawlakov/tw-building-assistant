module TWBuildingAssistant.Data.FSharp.Models

type NamedId = 
    { Id:int
      Name:string }

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

let emptyEffect =
    { PublicOrder = 0; RegularFood = 0; FertilityDependentFood = 0; ProvincialSanitation = 0; ResearchRate = 0; Growth = 0; Fertility = 0; ReligiousOsmosis = 0; RegionalSanitation = 0 }

let emptyEffectSet =
    { Effect = emptyEffect; Incomes = []; Influences = [] }