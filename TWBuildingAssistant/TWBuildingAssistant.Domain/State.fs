module TWBuildingAssistant.Domain.State

open FSharp.Data.Sql
open Effects
open Models

let getStateFromBuildings buildings =
    let regionalEffects = 
        buildings
        |> Seq.map (fun x -> collectEffectsSeq(x |> Seq.map (fun x -> x.EffectSet.Effect)))

    let regionalIncomes = 
        buildings
        |> Seq.collect (fun x -> x)
        |> Seq.collect (fun x -> x.EffectSet.Incomes)

    let regionalInfluences = 
        buildings
        |> Seq.collect (fun x -> x)
        |> Seq.collect (fun x -> x.EffectSet.Influences)

    (regionalEffects |> Seq.toList, regionalIncomes |> Seq.toList, regionalInfluences |> Seq.toList)

let getState buildings settings predefinedEffect =
    let (regionalEffects, regionalIncomes, regionalInfluences) = 
        getStateFromBuildings buildings

    let effect = 
        collectEffectsSeq (predefinedEffect.Effect::regionalEffects)
    let incomes = 
        regionalIncomes
        |> Seq.append predefinedEffect.Incomes
    let influences = 
        regionalInfluences
        |> Seq.append predefinedEffect.Influences

    let fertility = 
        match effect.Fertility with
        | fertility when fertility < 0 -> 0
        | fertility when fertility > 5 -> 5
        | fertility -> fertility
    let sanitation = 
        regionalEffects
        |> Seq.map (fun x -> x.RegionalSanitation + effect.ProvincialSanitation)
    let food = 
        effect.RegularFood + (fertility * effect.FertilityDependentFood)
    let publicOrder = 
        effect.PublicOrder + collectInfluencesSeq settings.ReligionId influences
    let income = 
        collectIncomesSeq fertility incomes

    let regionStates = 
        sanitation
        |> Seq.map (fun x -> { Sanitation = x })
        |> Seq.toArray

    { Regions = regionStates; Food = food; PublicOrder = publicOrder; ReligiousOsmosis = effect.ReligiousOsmosis; ResearchRate = effect.ResearchRate; Growth = effect.Growth; Wealth = income }