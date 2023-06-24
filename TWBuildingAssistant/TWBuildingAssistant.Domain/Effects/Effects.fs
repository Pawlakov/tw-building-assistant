module TWBuildingAssistant.Domain.Effects

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

type internal SpecificReligionInfluence = { ReligionId: string; Value: int }

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


// Constructors
let internal createIncomeCategory intValue =
    match intValue with
    | 1 -> Agriculture
    | 2 -> Husbandry
    | 3 -> Culture
    | 4 -> Industry
    | 5 -> LocalCommerce
    | 6 -> MaritimeCommerce
    | 7 -> Subsistence
    | _ -> failwith "Invalid value"

let internal createIncomeCategoryOption = Option.map createIncomeCategory

let internal createEffect publicOrder food sanitation researchRate growth fertility religiousOsmosis taxRate corruptionRate =
    { PublicOrder = publicOrder
      Food = food
      Sanitation = sanitation
      ResearchRate = researchRate
      Growth = growth
      Fertility = fertility
      ReligiousOsmosis = religiousOsmosis
      TaxRate = taxRate
      CorruptionRate = corruptionRate }

let internal createBonus value category =
    match (value, category) with
    | (_, Some category) -> CategoryBonus { Category = category; Value = value }
    | (_, _) -> AllBonus value

let internal createInfluence value religionId =
    match (religionId, value) with
    | (_, value) when value < 1 -> failwith "Negative influence."
    | (Some religionId, _) -> SpecificReligion { ReligionId = religionId; Value = value }
    | (None, _) -> StateReligion { Value = value }

let internal createEffectSet effect bonusSeq influenceSeq =
    { Effect = effect
      Bonuses = bonusSeq |> Seq.toList
      Influences = influenceSeq |> Seq.toList }

let internal createIncome value category isFertilityDependent =
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

let internal createLocalEffect maintenance food foodFromFertility sanitation capitalTier =
    { Maintenance = maintenance
      Food = food
      FoodFromFertility = foodFromFertility
      Sanitation = sanitation
      CapitalTier = capitalTier }

let internal createLocalEffectSet localEffect incomeSeq =
    { LocalEffect = localEffect
      Incomes = incomeSeq |> Seq.toList }
//

// Collectors
let internal collectEffects effectSeq =
    let effectFolder state effect =
        { PublicOrder = state.PublicOrder + effect.PublicOrder
          Food = state.Food + effect.Food
          Sanitation = state.Sanitation + effect.Sanitation
          ResearchRate = state.ResearchRate + effect.ResearchRate
          Growth = state.Growth + effect.Growth
          Fertility = state.Fertility + effect.Fertility
          ReligiousOsmosis = state.ReligiousOsmosis + effect.ReligiousOsmosis
          TaxRate = state.TaxRate + effect.TaxRate
          CorruptionRate = state.CorruptionRate + effect.CorruptionRate }

    Seq.fold effectFolder emptyEffect effectSeq

let internal collectIncomes fertilityLevel bonusSeq incomeSeq =
    let bonusFolder (records, allBonus) bonus =
        match bonus with
        | AllBonus value -> (records, allBonus + value)
        | CategoryBonus income -> (income :: records, allBonus)

    let (categoryBonuses, allBonus) = Seq.fold bonusFolder ([], 0) bonusSeq

    let bonusesGrouped =
        categoryBonuses
        |> List.groupBy (fun x -> x.Category)

    let incomesGrouped = Seq.groupBy (fun (x: Income) -> x.Category) incomeSeq

    let incomeCategoryFolder total (category, categoryIncomes) =
        let incomeFolder sum (income: Income) =
            match income.Value with
            | Simple value -> sum + value
            | FertilityDependent value -> sum + (value * fertilityLevel)

        let basePercentage = 100 + allBonus

        let percentage =
            bonusesGrouped
            |> List.tryPick (fun (x, y) -> if x = category then Some y else None)
            |> Option.map (fun x -> x |> List.sumBy (fun y -> y.Value))
            |> Option.fold (fun x y -> x + y) basePercentage

        let sum = Seq.fold incomeFolder 0 categoryIncomes

        total + (float (sum * percentage) * 0.01)

    let total = Seq.fold incomeCategoryFolder 0 incomesGrouped

    total

let internal collectInfluences stateReligionId influenceSeq =
    let influenceFolder (state, all) influence =
        match influence with
        | StateReligion influence ->
            (state + influence.Value, all + influence.Value)
        | SpecificReligion influence when influence.ReligionId = stateReligionId ->
            (state + influence.Value, all + influence.Value)
        | SpecificReligion influence ->
            (state, all + influence.Value)

    let (state, all) = Seq.fold influenceFolder (0, 0) influenceSeq

    let percentage =
        match all with
        | 0 -> 100.0
        | _ -> 100.0 * float (state) / float (all)

    let result = 0 - int (System.Math.Floor((750.0 - (percentage * 7.0)) * 0.01))

    result

let internal collectLocalEffects localEffectSeq =
    let localEffectFolder state effect =
        { Maintenance = state.Maintenance + effect.Maintenance
          Food = state.Food + effect.Food
          FoodFromFertility = state.FoodFromFertility + effect.FoodFromFertility
          Sanitation = state.Sanitation + effect.Sanitation
          CapitalTier = System.Math.Max (state.CapitalTier, effect.CapitalTier) }

    Seq.fold localEffectFolder emptyLocalEffect localEffectSeq
//

let internal getStateFromSettings
    getClimateEffectSet
    getProvinceEffectSet
    getWonderEffectSetSeq
    getReligionEffectSet
    getDifficultyEffectSet
    getTaxEffectSet
    getPowerLevelEffectSet
    getFactionEffectSet
    getTechnologyEffectSetSeq
    (settings: Settings.Settings)
    =
    let religionEffectSet = settings |> getReligionEffectSet

    let wonderEffectSetSeq = settings |> getWonderEffectSetSeq |> Seq.toList

    let provinceEffectSet = settings |> getProvinceEffectSet

    let climateEffectSet = settings |> getClimateEffectSet

    let difficultyEffectSet = settings |> getDifficultyEffectSet

    let taxEffectSet = settings |> getTaxEffectSet

    let powerLevelEffectSet = settings |> getPowerLevelEffectSet

    let fertilityDropAndCorruptionEffect =
        { emptyEffect with
            Fertility = settings.FertilityDrop
            CorruptionRate = settings.CorruptionRate }

    let piracyBonus =
        CategoryBonus
            { Category = MaritimeCommerce
              Value = -settings.PiracyRate }

    let factionEffectSet = settings |> getFactionEffectSet

    let technologyEffectSetSeq = settings |> getTechnologyEffectSetSeq |> Seq.toList

    let effectSets =
        wonderEffectSetSeq@
        technologyEffectSetSeq@
        [ factionEffectSet
          religionEffectSet
          provinceEffectSet
          climateEffectSet
          difficultyEffectSet
          taxEffectSet
          powerLevelEffectSet
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