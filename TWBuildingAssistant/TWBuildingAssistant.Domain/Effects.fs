module TWBuildingAssistant.Domain.Effects

open TWBuildingAssistant.Data.Sqlite

open Data
open Settings

type internal Effect =
    { PublicOrder: int
      Food: int
      Sanitation: int
      ResearchRate: int
      Growth: int
      Fertility: int
      ReligiousOsmosis: int
      TaxRate: int
      CorruptionRate: int }

type internal LocalEffect =
    { Maintenance: int
      Food: int
      FoodFromFertility: int
      Sanitation: int
      CapitalTier: int }

type internal IncomeCategory =
    | Agriculture
    | Husbandry
    | Culture
    | Industry
    | LocalCommerce
    | MaritimeCommerce
    | Subsistence

type internal IncomeValue =
    | Simple of int
    | FertilityDependent of int

type internal Income =
    { Category: IncomeCategory
      Value: IncomeValue }

type internal CategoryBonus =
    { Category: IncomeCategory
      Value: int }

type internal Bonus =
    | CategoryBonus of CategoryBonus
    | AllBonus of int

type internal StateReligionInfluence = { Value: int }

type internal SpecificReligionInfluence = { ReligionId: int; Value: int }

type internal Influence =
    | StateReligion of StateReligionInfluence
    | SpecificReligion of SpecificReligionInfluence

type internal EffectSet =
    { Effect: Effect
      Bonuses: Bonus list
      Influences: Influence list }

type internal LocalEffectSet =
    { LocalEffect: LocalEffect
      Incomes: Income list }


let internal emptyEffect =
    { PublicOrder = 0
      Food = 0
      Sanitation = 0
      ResearchRate = 0
      Growth = 0
      Fertility = 0
      ReligiousOsmosis = 0
      TaxRate = 0
      CorruptionRate = 0 }

let internal emptyLocalEffect =
    { Maintenance = 0
      Food = 0
      FoodFromFertility = 0
      Sanitation = 0
      CapitalTier = 0 }

let internal emptyEffectSet =
    { Effect = emptyEffect
      Bonuses = []
      Influences = [] }

let internal emptyLocalEffectSet =
    { LocalEffect = emptyLocalEffect
      Incomes = [] }


let private getIncomeCategory intValue =
    match intValue with
    | 1 -> Agriculture
    | 2 -> Husbandry
    | 3 -> Culture
    | 4 -> Industry
    | 5 -> LocalCommerce
    | 6 -> MaritimeCommerce
    | 7 -> Subsistence
    | _ -> failwith "Invalid value"

let private createEffect (publicOrder, food, sanitation, researchRate, growth, fertility, religiousOsmosis, taxRate, corruptionRate) =
    { PublicOrder = publicOrder
      Food = food
      Sanitation = sanitation
      ResearchRate = researchRate
      Growth = growth
      Fertility = fertility
      ReligiousOsmosis = religiousOsmosis
      TaxRate = taxRate
      CorruptionRate = corruptionRate }

let private createBonus (value, categoryInt) =
    match (value, categoryInt) with
    | (_, Some categoryInt) -> CategoryBonus { Category = (categoryInt |> getIncomeCategory); Value = value }
    | (_, _) -> AllBonus value

let private createInfluence (value, religionId) =
    match (religionId, value) with
    | (_, value) when value < 1 -> failwith "Negative influence."
    | (Some religionId, _) -> SpecificReligion { ReligionId = religionId; Value = value }
    | (None, _) -> StateReligion { Value = value }

let private createEffectSet (effectTuple, bonusTupleSeq: seq<int*int option>, influenceTupleSeq: seq<int*int option>) =
    { Effect = effectTuple |> createEffect
      Bonuses = query { for tuple in bonusTupleSeq do select (createBonus tuple) } |> Seq.toList
      Influences = query { for tuple in influenceTupleSeq do select (createInfluence tuple) } |> Seq.toList }

let private createEffectSets (tupleSeq: seq<(int*int*int*int*int*int*int*int*int)*seq<int*int option>*seq<int*int option>>) =
    query { for tuple in tupleSeq do select (createEffectSet tuple) } |> Seq.toList

let private getIncomeCategoryOption intValue =
    intValue |> Option.map getIncomeCategory

let private createIncomeFromJson (jsonBuildingIncome: JsonBuildingIncome) =
    let value = jsonBuildingIncome.Value
    let category = jsonBuildingIncome.Category |> getIncomeCategory
    let isFertilityDependent = jsonBuildingIncome.IsFertilityDependent |> Option.defaultValue false

    match isFertilityDependent with
    | false ->
        { Category = category
          Value = Simple value }: Income
    | true ->
        match (value, category) with
        | (_, Agriculture) ->
            { Category = Agriculture
              Value = FertilityDependent value }
        | (_, Husbandry) ->
            { Category = Husbandry
              Value = FertilityDependent value }
        | (_, _) -> failwith "Invalid fertility-based income."

let private createBonusFromEntity (rd: Entities.Bonus) =
    let value = rd.Value

    let category =
        (if rd.Category.HasValue then
             Some rd.Category.Value
         else
             None)
        |> getIncomeCategoryOption

    match (value, category) with
    | (_, Some category) -> CategoryBonus { Category = category; Value = value }
    | (_, _) -> AllBonus value

let private createBonusFromJson_1 (jsonBuildingBonus: JsonBuildingBonus) =
    let value = jsonBuildingBonus.Value
    let category = jsonBuildingBonus.Category |> getIncomeCategoryOption

    match (value, category) with
    | (_, Some category) -> CategoryBonus { Category = category; Value = value }
    | (_, _) -> AllBonus value

let private createInfluenceFromEntity (rd: Entities.Influence) =
    let religionId =
        (if rd.ReligionId.HasValue then
             Some rd.ReligionId.Value
         else
             None)

    let value = rd.Value

    match (religionId, value) with
    | (_, value) when value < 1 -> failwith "Negative influence."
    | (Some religionId, _) ->
        SpecificReligion
            { ReligionId = religionId
              Value = value }
    | (None, _) -> StateReligion { Value = value }

let private createInfluenceFromJson_1 (jsonBuildingInfluence: JsonBuildingInfluence) =
    let religionId = jsonBuildingInfluence.ReligionId
    let value = jsonBuildingInfluence.Value

    match (religionId, value) with
    | (_, value) when value < 1 -> failwith "Negative influence."
    | (Some religionId, _) ->
        SpecificReligion
            { ReligionId = religionId
              Value = value }
    | (None, _) -> StateReligion { Value = value }


let internal getEffect (ctx: DatabaseContext) effectId =
    let effect =
        query {
            for effect in ctx.Effects do
                where (effect.Id = effectId)

                select
                    { PublicOrder = effect.PublicOrder
                      Food = effect.Food
                      Sanitation = effect.Sanitation
                      ResearchRate = effect.ResearchRate
                      Growth = effect.Growth
                      Fertility = effect.Fertility
                      ReligiousOsmosis = effect.ReligiousOsmosis
                      TaxRate = effect.TaxRate
                      CorruptionRate = effect.CorruptionRate }

                head
        }

    let bonuses =
        query {
            for bonus in ctx.Bonuses do
                where (bonus.EffectId = effectId)
        }
        |> Seq.map createBonusFromEntity
        |> Seq.toList

    let influences =
        query {
            for influence in ctx.Influences do
                where (influence.EffectId = effectId)
        }
        |> Seq.map createInfluenceFromEntity
        |> Seq.toList

    { Effect = effect
      Bonuses = bonuses
      Influences = influences }

let internal getEffectFromJson_1 (jsonBuildingEffect: JsonBuildingEffect) =
    let effect =
        { PublicOrder = jsonBuildingEffect.PublicOrder |> Option.defaultValue 0
          Food = jsonBuildingEffect.Food |> Option.defaultValue 0
          Sanitation = jsonBuildingEffect.Sanitation |> Option.defaultValue 0
          ResearchRate = jsonBuildingEffect.ResearchRate |> Option.defaultValue 0
          Growth = jsonBuildingEffect.Growth |> Option.defaultValue 0
          Fertility = jsonBuildingEffect.Fertility |> Option.defaultValue 0
          ReligiousOsmosis = jsonBuildingEffect.ReligiousOsmosis |> Option.defaultValue 0
          TaxRate = jsonBuildingEffect.TaxRate |> Option.defaultValue 0
          CorruptionRate = jsonBuildingEffect.CorruptionRate |> Option.defaultValue 0 }

    let bonuses =
        jsonBuildingEffect.Bonuses
        |> Seq.map createBonusFromJson_1
        |> Seq.toList

    let influences =
        jsonBuildingEffect.Influences
        |> Seq.map createInfluenceFromJson_1
        |> Seq.toList

    { Effect = effect
      Bonuses = bonuses
      Influences = influences }

let internal getEffectOption (ctx: DatabaseContext) effectId =
    match effectId with
    | Some effectId -> effectId |> getEffect ctx
    | None -> emptyEffectSet

let internal getEffectFromJsonOption_1 (jsonBuildingEffect:JsonBuildingEffect option) =
    match jsonBuildingEffect with
    | Some jsonBuildingEffect -> jsonBuildingEffect |> getEffectFromJson_1
    | None -> emptyEffectSet

let internal getLocalEffectFromJson (jsonBuildingLevel:JsonBuildingLevel) =
    let localEffect =
        { Maintenance = jsonBuildingLevel.Maintenance |> Option.defaultValue 0
          Food = jsonBuildingLevel.LocalFood |> Option.defaultValue 0
          FoodFromFertility = jsonBuildingLevel.LocalFoodFromFertility |> Option.defaultValue 0
          Sanitation = jsonBuildingLevel.LocalSanitation |> Option.defaultValue 0
          CapitalTier = jsonBuildingLevel.CapitalTier |> Option.defaultValue 0 }

    let incomes =
        jsonBuildingLevel.Incomes
        |> Seq.map createIncomeFromJson
        |> Seq.toList

    { LocalEffect = localEffect
      Incomes = incomes }

let internal collectEffects (effects: Effect list) =
    { PublicOrder = effects |> List.sumBy (fun x -> x.PublicOrder)
      Food = effects |> List.sumBy (fun x -> x.Food)
      Sanitation = effects |> List.sumBy (fun x -> x.Sanitation)
      ResearchRate = effects |> List.sumBy (fun x -> x.ResearchRate)
      Growth = effects |> List.sumBy (fun x -> x.Growth)
      Fertility = effects |> List.sumBy (fun x -> x.Fertility)
      ReligiousOsmosis =
        effects
        |> List.sumBy (fun x -> x.ReligiousOsmosis)
      TaxRate = effects |> List.sumBy (fun x -> x.TaxRate)
      CorruptionRate = effects |> List.sumBy (fun x -> x.CorruptionRate) }

let internal collectIncomes fertilityLevel bonuses (incomes: Income list) =
    let firstLoop (records, allBonus) (bonus: Bonus) =
        match bonus with
        | AllBonus value -> (records, allBonus + value)
        | CategoryBonus income -> (income :: records, allBonus)

    let (categoryBonuses, allBonus) = bonuses |> List.fold firstLoop ([], 0)

    let bonusesGrouped =
        categoryBonuses
        |> List.groupBy (fun x -> x.Category)

    let incomesGrouped = incomes |> List.groupBy (fun x -> x.Category)

    let secondLoop total (category, categoryIncomes) =
        let thirdLoop sum (income: Income) =
            match income.Value with
            | Simple value -> sum + value
            | FertilityDependent value -> sum + (value * fertilityLevel)

        let basePercentage = 100 + allBonus

        let percentage =
            bonusesGrouped
            |> List.tryPick (fun (x, y) -> if x = category then Some y else None)
            |> Option.map (fun x -> x |> List.sumBy (fun y -> y.Value))
            |> Option.fold (fun x y -> x + y) basePercentage

        let sum = categoryIncomes |> List.fold thirdLoop 0

        total + (float (sum * percentage) * 0.01)

    let total = incomesGrouped |> List.fold secondLoop 0

    total

let internal collectInfluences stateReligionId influences =
    let loop (state, all) (influence: Influence) =
        match influence with
        | StateReligion influence -> (state + influence.Value, all + influence.Value)
        | SpecificReligion influence when influence.ReligionId = stateReligionId ->
            (state + influence.Value, all + influence.Value)
        | SpecificReligion influence -> (state, all + influence.Value)

    let (state, all) = influences |> List.fold loop (0, 0)

    let percentage =
        match all with
        | 0 -> 100.0
        | _ -> 100.0 * float (state) / float (all)

    let result =
        0
        - int (System.Math.Floor((750.0 - (percentage * 7.0)) * 0.01))

    result

let internal collectLocalEffects (localEffects: LocalEffect list) =
    { Maintenance =
        localEffects
        |> List.sumBy (fun x -> x.Maintenance)
      Food = 
        localEffects 
        |> List.sumBy (fun x -> x.Food)
      FoodFromFertility =
        localEffects
        |> List.sumBy (fun x -> x.FoodFromFertility)
      Sanitation = 
        localEffects 
        |> List.sumBy (fun x -> x.Sanitation)
      CapitalTier =
        localEffects
        |> List.map (fun x -> x.CapitalTier)
        |> List.max }

let internal getReligionEffect (ctx: DatabaseContext) (religionsData: ReligionsData.Root []) religionId =
    let effectId =
        query {
            for religion in religionsData do
                where (religion.Id = religionId)
                select religion.EffectId
                head
        }

    effectId |> (getEffectOption ctx)

let internal getProvinceEffect (ctx: DatabaseContext) (provincesData: ProvincesData.Root []) provinceId =
    let effectId =
        query {
            for province in provincesData do
                where (province.Id = provinceId)
                select province.EffectId
                head
        }

    effectId |> (getEffect ctx)

let internal getClimateEffect
    (ctx: DatabaseContext)
    (provincesData: ProvincesData.Root [])
    (climatesData: ClimatesData.Root [])
    provinceId
    seasonId
    weatherId
    =
    let climateId =
        query {
            for province in provincesData do
                where (province.Id = provinceId)
                select province.ClimateId
                head
        }

    let climate =
        query {
            for climate in climatesData do
                where (climate.Id = climateId)
                select climate
                head
        }

    let effectId =
        climate.Effects
        |> Seq.tryFind (fun x -> x.SeasonId = seasonId)
        |> Option.bind (fun x ->
            x.Effects
            |> Seq.tryFind (fun y -> y.WeatherId = weatherId)
            |> Option.map (fun z -> z.EffectId))

    effectId |> (getEffectOption ctx)

let internal getDifficultyEffect (ctx: DatabaseContext) (difficultiesData: DifficultiesData.Root []) difficultyId =
    let effectId =
        query {
            for difficulty in difficultiesData do
                where (difficulty.Id = difficultyId)
                select difficulty.EffectId
                head
        }

    effectId |> (getEffectOption ctx)

let internal getTaxEffect (ctx: DatabaseContext) (taxesData: TaxesData.Root []) taxId =
    let effectId =
        query {
            for tax in taxesData do
                where (tax.Id = taxId)
                select tax.EffectId
                head
        }

    effectId |> (getEffect ctx)

let internal getPowerLevelEffect (ctx: DatabaseContext) (powerLevelsData: PowerLevelsData.Root []) powerLevelId =
    let effectId =
        query {
            for powerLevel in powerLevelsData do
                where (powerLevel.Id = powerLevelId)
                select powerLevel.EffectId
                head
        }

    effectId |> (getEffect ctx)

let internal getStateFromSettings
    (ctx: DatabaseContext)
    climatesData
    provincesData
    religionsData
    difficultiesData
    taxesData
    powerLevelsData
    getFactionEffectTupleSeq
    getTechnologyEffectTupleSeq
    (settings: Settings)
    =
    let religionEffects = getReligionEffect ctx religionsData settings.ReligionId
    let provinceEffects = getProvinceEffect ctx provincesData settings.ProvinceId

    let climateEffects =
        getClimateEffect ctx provincesData climatesData settings.ProvinceId settings.SeasonId settings.WeatherId

    let difficultyEffects =
        getDifficultyEffect ctx difficultiesData settings.DifficultyId

    let taxEffects = getTaxEffect ctx taxesData settings.TaxId

    let powerLevelEffects =
        getPowerLevelEffect ctx powerLevelsData settings.PowerLevelId

    let fertilityDropAndCorruptionEffect =
        { emptyEffect with
            Fertility = settings.FertilityDrop
            CorruptionRate = settings.CorruptionRate }

    let piracyBonus =
        CategoryBonus
            { Category = MaritimeCommerce
              Value = -settings.PiracyRate }

    let factionEffects = settings |> getFactionEffectTupleSeq |> createEffectSets

    let technologyEffects = settings |> getTechnologyEffectTupleSeq |> createEffectSets

    let effectSets =
        factionEffects@
        technologyEffects@
        [ religionEffects
          provinceEffects
          climateEffects
          difficultyEffects
          taxEffects
          powerLevelEffects
          { Effect = fertilityDropAndCorruptionEffect
            Bonuses = [ piracyBonus ]
            Influences = [] } ]

    let effect =
        effectSets
        |> List.map (fun x -> x.Effect)
        |> collectEffects

    let bonuses =
        effectSets
        |> List.map (fun x -> x.Bonuses)
        |> List.concat

    let influences =
        effectSets
        |> List.map (fun x -> x.Influences)
        |> List.concat

    { Effect = effect
      Bonuses = bonuses
      Influences = influences }

let internal collectEffectsSeq effects = effects |> Seq.toList |> collectEffects

let internal collectLocalEffectsSeq localEffects =
    localEffects |> Seq.toList |> collectLocalEffects

let internal collectInfluencesSeq stateReligionId influences =
    influences
    |> Seq.toList
    |> collectInfluences stateReligionId
