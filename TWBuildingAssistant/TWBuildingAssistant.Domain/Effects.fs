module TWBuildingAssistant.Domain.Effects

open FSharp.Data.Sql
open Database
open Settings

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

type IncomeValue =
    | Simple of int
    | Percentage of int
    | FertilityDependent of int

type CategoryIncome =
    { Category:IncomeCategory
      Value:IncomeValue }

type Income =
    | CategoryIncome of CategoryIncome
    | AllPercentage of int

type StateReligionInfluence =
    { Value:int }

type SpecificReligionInfluence =
    { ReligionId:int
      Value:int }

type Influence =
    | StateReligion of StateReligionInfluence
    | SpecificReligion of SpecificReligionInfluence

type EffectSet =
    { Effect:Effect
      Incomes:Income list
      Influences:Influence list }

let emptyEffect =
    { PublicOrder = 0; RegularFood = 0; FertilityDependentFood = 0; ProvincialSanitation = 0; ResearchRate = 0; Growth = 0; Fertility = 0; ReligiousOsmosis = 0; RegionalSanitation = 0 }

let emptyEffectSet =
    { Effect = emptyEffect; Incomes = []; Influences = [] }

let getIncomeCategory intValue =
    match intValue with
    | Some 1 -> Some Agriculture
    | Some 2 -> Some Husbandry
    | Some 3 -> Some Culture
    | Some 4 -> Some Industry
    | Some 5 -> Some LocalCommerce
    | Some 6 -> Some MaritimeCommerce
    | Some 7 -> Some Subsistence
    | Some 8 -> Some Maintenance
    | Some _ -> failwith "Invalid value"
    | None -> None

let createIncome (rd:sql.dataContext.``dbo.BonusesEntity``) =  
    let value = rd.Value 
    let category = rd.Category |> getIncomeCategory
    let bonusType = rd.Type

    match bonusType with
    | 0 -> // Simple
        match (value, category) with
        | (value, Some Maintenance) when value > 0 -> failwith "Positive 'Maintenance' income."
        | (_, Some category) -> CategoryIncome { Category = category; Value = Simple value }
        | (_, _) -> failwith "Invalid 'All' income."
    | 1 -> // Percentahe
        match (value, category) with
        | (_, Some Maintenance) -> failwith "Invalid 'Maintenance' income."
        | (_, Some category) -> CategoryIncome { Category = category; Value = Percentage value }
        | (_, _) -> AllPercentage value
    | 2 -> // Fertility Dependent
        match (value, category) with
        | (_, Some Agriculture) -> CategoryIncome { Category = Agriculture; Value = FertilityDependent value }
        | (_, Some Husbandry) -> CategoryIncome { Category = Husbandry; Value = FertilityDependent value }
        | (_, _) -> failwith "Invalid fertility-based income."
    | _ ->
        failwith "Invalid value"

let createInfluence (rd:sql.dataContext.``dbo.InfluencesEntity``) =    
    let religionId = rd.ReligionId
    let value = rd.Value
    match (religionId, value) with
    | (_, value) when value < 1 ->
        failwith "Negative influence."
    | (Some religionId, _) ->
        SpecificReligion { ReligionId = religionId; Value = value }
    | (None, _) ->
        StateReligion { Value = value }

let getEffect (ctx:sql.dataContext) effectId =
    let effect =
        query {
            for effect in ctx.Dbo.Effects do
            where (effect.Id = effectId)
            select 
                { PublicOrder = effect.PublicOrder;
                  RegularFood = effect.RegularFood; 
                  FertilityDependentFood = effect.FertilityDependentFood;
                  ProvincialSanitation = effect.ProvincialSanitation;
                  ResearchRate = effect.ResearchRate;
                  Growth = effect.Growth
                  Fertility = effect.Fertility
                  ReligiousOsmosis = effect.ReligiousOsmosis
                  RegionalSanitation = effect.RegionalSanitation }
            head }

    let incomes =
        query {
            for income in ctx.Dbo.Bonuses do
            where (income.EffectId = Some effectId) }
        |> Seq.map createIncome
        |> Seq.toList

    let influences =
        query {
            for influence in ctx.Dbo.Influences do
            where (influence.EffectId = Some effectId) }
        |> Seq.map createInfluence
        |> Seq.toList

    { Effect = effect; Incomes = incomes; Influences = influences }

let getEffectOption (ctx:sql.dataContext) effectId =
    match effectId with
    | Some effectId ->
        effectId |> getEffect ctx
    | None ->
        emptyEffectSet

let collectEffects (effects:Effect list) =
    { PublicOrder = effects |> List.map (fun x -> x.PublicOrder) |> List.sum;
      RegularFood = effects |> List.map (fun x -> x.RegularFood) |> List.sum; 
      FertilityDependentFood = effects |> List.map (fun x -> x.FertilityDependentFood) |> List.sum;
      ProvincialSanitation = effects |> List.map (fun x -> x.ProvincialSanitation) |> List.sum;
      ResearchRate = effects |> List.map (fun x -> x.ResearchRate) |> List.sum;
      Growth = effects |> List.map (fun x -> x.Growth) |> List.sum;
      Fertility = effects |> List.map (fun x -> x.Fertility) |> List.sum;
      ReligiousOsmosis = effects |> List.map (fun x -> x.ReligiousOsmosis) |> List.sum;
      RegionalSanitation = effects |> List.map (fun x -> x.RegionalSanitation) |> List.sum }

let collectIncomes fertilityLevel incomes =
    let firstLoop (records, allBonus) (income:Income) =
        match income with
        | AllPercentage value ->
            (records, allBonus + value)
        | CategoryIncome income ->
            (income::records, allBonus)

    let (categoryIncomes, allBonus) = 
        incomes 
        |> List.fold firstLoop ([], 0)

    let secondLoop total (category, categoryIncomes) =
        let thirdLoop (sum, percentageFromIncomes) (income:CategoryIncome) =
            match income.Value with
            | Simple value ->
                (sum + value, percentageFromIncomes)
            | Percentage value ->
                (sum, percentageFromIncomes + value)
            | FertilityDependent value ->
                (sum + (value * fertilityLevel), percentageFromIncomes)

        let basePercentage = 
            match category with
            | Maintenance ->
                100
            | _ ->
                100 + allBonus

        let (sum, percentage) = 
            categoryIncomes 
            |> List.fold thirdLoop (0, basePercentage)

        total + (float(sum * percentage) * 0.01)

    let total =
        categoryIncomes 
        |> List.groupBy (fun x -> x.Category) 
        |> List.fold secondLoop 0

    total

let collectInfluences stateReligionId influences =
    let loop (state, all) (influence:Influence) =
        match influence with
        | StateReligion influence ->
            (state + influence.Value, all + influence.Value)
        | SpecificReligion influence when influence.ReligionId = stateReligionId ->
            (state + influence.Value, all + influence.Value)
        | SpecificReligion influence ->
            (state, all + influence.Value)

    let (state, all) =
        influences
        |> List.fold loop (0, 0)

    let percentage =
        match all with
        | 0 ->
            100.0
        | _ ->
            100.0 * float(state) / float(all)

    let result = 0 - int(System.Math.Floor((750.0 - (percentage * 7.0)) * 0.01))
    result

let getFactionwideEffects (ctx:sql.dataContext) factionId =
    let effectId = 
        query {
            for faction in ctx.Dbo.Factions do
            where (faction.Id = factionId)
            select faction.EffectId
            head }

    effectId |> Option.map (getEffect ctx)

let getTechnologyEffect (ctx:sql.dataContext) factionId technologyTier useAntilegacyTechnologies =
    let getEffectOption =
        Option.map (getEffect ctx)

    let getEffectSeq =
        Seq.choose getEffectOption

    let universalEffectIds = 
        query {
            for technologyLevel in ctx.Dbo.TechnologyLevels do
            where (technologyLevel.FactionId = factionId && technologyLevel.Order <= technologyTier)
            select technologyLevel.UniversalEffectId }

    let effects = 
        if useAntilegacyTechnologies then
            let antilegacyEffectIds = 
                query {
                    for technologyLevel in ctx.Dbo.TechnologyLevels do
                    where (technologyLevel.FactionId = factionId && technologyLevel.Order <= technologyTier)
                    select technologyLevel.AntilegacyEffectId }

            universalEffectIds |> Seq.append antilegacyEffectIds |> getEffectSeq
        else
            universalEffectIds |> getEffectSeq
        |> Seq.toList

    { Effect = effects |> List.map (fun x -> x.Effect) |> collectEffects; Incomes = effects |> List.collect (fun x -> x.Incomes); Influences = effects |> List.collect (fun x -> x.Influences) }

let getReligionEffect (ctx:sql.dataContext) religionId =
    let effectId = 
        query {
            for religion in ctx.Dbo.Religions do
            where (religion.Id = religionId)
            select religion.EffectId
            head }

    effectId |> Option.map (getEffect ctx)

let getProvinceEffect (ctx:sql.dataContext) provinceId =
    let effectId = 
        query {
            for province in ctx.Dbo.Provinces do
            where (province.Id = provinceId)
            select province.EffectId
            head }

    effectId |> Option.map (getEffect ctx)

let getClimateEffect (ctx:sql.dataContext) provinceId seasonId weatherId =
    let effectId = 
        query {
            for province in ctx.Dbo.Provinces do
            where (province.Id = provinceId)
            join effect in ctx.Dbo.WeatherEffects on (province.ClimateId = effect.ClimateId)
            where (effect.WeatherId = weatherId && effect.SeasonId = seasonId)
            select effect.EffectId
            head }

    effectId |> (getEffect ctx)

let getDifficultyEffect (ctx:sql.dataContext) difficultyId =
    let effectId = 
        query {
            for difficulty in ctx.Dbo.Difficulties do
            where (difficulty.Id = difficultyId)
            select difficulty.EffectId
            head }

    effectId |> Option.map (getEffect ctx)

let getTaxEffect (ctx:sql.dataContext) taxId =
    let effectId = 
        query {
            for tax in ctx.Dbo.Taxes do
            where (tax.Id = taxId)
            select tax.EffectId
            head }

    effectId |> Option.map (getEffect ctx)

let getStateFromSettings settings =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let factionEffects = getFactionwideEffects ctx settings.FactionId
    let technologyEffects = getTechnologyEffect ctx settings.FactionId settings.TechnologyTier settings.UseAntilegacyTechnologies
    let religionEffects = getReligionEffect ctx settings.ReligionId
    let provinceEffects = getProvinceEffect ctx settings.ProvinceId
    let climateEffects = getClimateEffect ctx settings.ProvinceId settings.SeasonId settings.WeatherId
    let difficultyEffects = getDifficultyEffect ctx settings.DifficultyId
    let taxEffects = getTaxEffect ctx settings.TaxId
    let fertilityDropEffect = 
        { emptyEffect with Fertility = settings.FertilityDrop }
    let corruptionIncome = 
        AllPercentage -settings.CorruptionRate
    let piracyIncome = 
        CategoryIncome { Category = MaritimeCommerce; Value = Percentage -settings.PiracyRate }

    let effectSets =
        [ factionEffects
          Some technologyEffects
          religionEffects
          provinceEffects
          Some climateEffects
          difficultyEffects
          taxEffects
          Some { Effect = fertilityDropEffect; Incomes = [corruptionIncome; piracyIncome]; Influences = []} ]
        |> List.choose (fun x -> x)

    let effect =
        effectSets |> List.map (fun x -> x.Effect) |> collectEffects

    let incomes = 
        effectSets |> List.map (fun x -> x.Incomes) |> List.concat

    let influences = 
        effectSets |> List.map (fun x -> x.Influences) |> List.concat

    { Effect = effect; Incomes = incomes; Influences = influences }

let collectEffectsSeq effects =
    effects 
    |> Seq.toList 
    |> collectEffects

let collectIncomesSeq fertilityLevel incomes =
    incomes 
    |> Seq.toList 
    |> collectIncomes fertilityLevel

let collectInfluencesSeq stateReligionId influences =
    influences 
    |> Seq.toList 
    |> collectInfluences stateReligionId