module TWBuildingAssistant.Domain.Effects

open FSharp.Data.Sql
open Database
open Settings

type Effect =
    { PublicOrder:int
      Food:int
      Sanitation:int
      ResearchRate:int
      Growth:int
      Fertility:int
      ReligiousOsmosis:int }

type LocalEffect =
    { Maintenance:int
      Food:int
      FoodFromFertility:int
      Sanitation:int }

type IncomeCategory =
    | Agriculture
    | Husbandry
    | Culture
    | Industry
    | LocalCommerce
    | MaritimeCommerce
    | Subsistence

type IncomeValue =
    | Simple of int
    | FertilityDependent of int

type Income =
    { Category:IncomeCategory
      Value:IncomeValue }

type CategoryBonus =
    { Category:IncomeCategory
      Value:int }

type Bonus =
    | CategoryBonus of CategoryBonus
    | AllBonus of int

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
      Bonuses:Bonus list
      Influences:Influence list }

type LocalEffectSet =
    { LocalEffect:LocalEffect
      Incomes:Income list }

let emptyEffect =
    { PublicOrder = 0; Food = 0; Sanitation = 0; ResearchRate = 0; Growth = 0; Fertility = 0; ReligiousOsmosis = 0 }

let emptyLocalEffect =
    { Maintenance = 0; Food = 0; FoodFromFertility = 0; Sanitation = 0 }

let emptyEffectSet =
    { Effect = emptyEffect; Bonuses = []; Influences = [] }

let emptyLocalEffectSet =
    { LocalEffect = emptyLocalEffect; Incomes = [] }

let getIncomeCategory intValue =
    match intValue with
    | 1 -> Agriculture
    | 2 -> Husbandry
    | 3 -> Culture
    | 4 -> Industry
    | 5 -> LocalCommerce
    | 6 -> MaritimeCommerce
    | 7 -> Subsistence
    | _ -> failwith "Invalid value"

let getIncomeCategoryOption intValue =
    intValue 
    |> Option.map getIncomeCategory

let createIncome (rd:sql.dataContext.``dbo.IncomesEntity``) =  
    let value = rd.Value 
    let category = rd.Category |> getIncomeCategory
    let isFertilityDependent = rd.IsFertilityDependent

    match isFertilityDependent with
    | false ->
        { Category = category; Value = Simple value }:Income
    | true -> 
        match (value, category) with
        | (_, Agriculture) -> { Category = Agriculture; Value = FertilityDependent value }
        | (_, Husbandry) -> { Category = Husbandry; Value = FertilityDependent value }
        | (_, _) -> failwith "Invalid fertility-based income."

let createBonus (rd:sql.dataContext.``dbo.BonusesEntity``) =  
    let value = rd.Value 
    let category = rd.Category |> getIncomeCategoryOption

    match (value, category) with
    | (_, Some category) -> CategoryBonus { Category = category; Value = value }
    | (_, _) -> AllBonus value

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
                  Food = effect.Food; 
                  Sanitation = effect.Sanitation;
                  ResearchRate = effect.ResearchRate;
                  Growth = effect.Growth
                  Fertility = effect.Fertility
                  ReligiousOsmosis = effect.ReligiousOsmosis }
            head }

    let bonuses =
        query {
            for bonus in ctx.Dbo.Bonuses do
            where (bonus.EffectId = Some effectId) }
        |> Seq.map createBonus
        |> Seq.toList

    let influences =
        query {
            for influence in ctx.Dbo.Influences do
            where (influence.EffectId = Some effectId) }
        |> Seq.map createInfluence
        |> Seq.toList

    { Effect = effect; Bonuses = bonuses; Influences = influences }

let getEffectOption (ctx:sql.dataContext) effectId =
    match effectId with
    | Some effectId ->
        effectId |> getEffect ctx
    | None ->
        emptyEffectSet

let getLocalEffect (ctx:sql.dataContext) buildingLevelId =
    let localEffect =
        query {
            for buildingLevel in ctx.Dbo.BuildingLevels do
            where (buildingLevel.Id = buildingLevelId)
            select 
                { Maintenance = buildingLevel.Maintenance
                  Food = buildingLevel.LocalFood
                  FoodFromFertility = buildingLevel.LocalFoodFromFertility
                  Sanitation = buildingLevel.LocalSanitation }
            head }

    let incomes =
        query {
            for income in ctx.Dbo.Incomes do
            where (income.BuildingLevelId = buildingLevelId) }
        |> Seq.map createIncome
        |> Seq.toList

    { LocalEffect = localEffect; Incomes = incomes }

let collectEffects (effects:Effect list) =
    { PublicOrder = effects |> List.sumBy (fun x -> x.PublicOrder)
      Food = effects |> List.sumBy (fun x -> x.Food)
      Sanitation = effects |> List.sumBy (fun x -> x.Sanitation)
      ResearchRate = effects |> List.sumBy (fun x -> x.ResearchRate)
      Growth = effects |> List.sumBy (fun x -> x.Growth)
      Fertility = effects |> List.sumBy (fun x -> x.Fertility)
      ReligiousOsmosis = effects |> List.sumBy (fun x -> x.ReligiousOsmosis) }

let collectIncomes fertilityLevel bonuses (incomes:Income list) =
    let firstLoop (records, allBonus) (bonus:Bonus) =
        match bonus with
        | AllBonus value ->
            (records, allBonus + value)
        | CategoryBonus income ->
            (income::records, allBonus)

    let (categoryBonuses, allBonus) = 
        bonuses 
        |> List.fold firstLoop ([], 0)

    let bonusesGrouped =
        categoryBonuses 
        |> List.groupBy (fun x -> x.Category) 

    let incomesGrouped =
        incomes 
        |> List.groupBy (fun x -> x.Category) 

    let secondLoop total (category, categoryIncomes) =
        let thirdLoop sum (income:Income) =
            match income.Value with
            | Simple value ->
                sum + value
            | FertilityDependent value ->
                sum + (value * fertilityLevel)

        let basePercentage = 
            100 + allBonus

        let percentage =
            bonusesGrouped 
            |> List.tryPick (fun (x, y) -> if x = category then Some y else None)
            |> Option.map (fun x -> x |> List.sumBy (fun y -> y.Value))
            |> Option.fold (fun x y -> x + y) basePercentage

        let sum = 
            categoryIncomes 
            |> List.fold thirdLoop 0

        total + (float(sum * percentage) * 0.01)

    let total =
        incomesGrouped
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

let collectLocalEffects (localEffects:LocalEffect list) =
    { Maintenance = localEffects |> List.sumBy (fun x -> x.Maintenance)
      Food = localEffects |> List.sumBy (fun x -> x.Food)
      FoodFromFertility = localEffects |> List.sumBy (fun x -> x.FoodFromFertility)
      Sanitation = localEffects |> List.sumBy (fun x -> x.Sanitation) }

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

    { Effect = effects |> List.map (fun x -> x.Effect) |> collectEffects
      Bonuses = effects |> List.collect (fun x -> x.Bonuses)
      Influences = effects |> List.collect (fun x -> x.Influences) }

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
    let corruptionBonus = 
        AllBonus -settings.CorruptionRate
    let piracyBonus = 
        CategoryBonus { Category = MaritimeCommerce; Value = -settings.PiracyRate }

    let effectSets =
        [ factionEffects
          Some technologyEffects
          religionEffects
          provinceEffects
          Some climateEffects
          difficultyEffects
          taxEffects
          Some { Effect = fertilityDropEffect; Bonuses = [corruptionBonus; piracyBonus]; Influences = []} ]
        |> List.choose (fun x -> x)

    let effect =
        effectSets |> List.map (fun x -> x.Effect) |> collectEffects

    let bonuses = 
        effectSets |> List.map (fun x -> x.Bonuses) |> List.concat

    let influences = 
        effectSets |> List.map (fun x -> x.Influences) |> List.concat

    { Effect = effect; Bonuses = bonuses; Influences = influences }

let collectEffectsSeq effects =
    effects 
    |> Seq.toList 
    |> collectEffects

let collectLocalEffectsSeq localEffects =
    localEffects 
    |> Seq.toList 
    |> collectLocalEffects

let collectInfluencesSeq stateReligionId influences =
    influences 
    |> Seq.toList 
    |> collectInfluences stateReligionId