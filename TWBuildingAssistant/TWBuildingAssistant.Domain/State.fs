module TWBuildingAssistant.Domain.State

open FSharp.Data.Sql
open Buildings
open Effects
open Settings

type RegionState = 
    { Sanitation:int
      Wealth:float
      Maintenance:float }

type ProvinceState = 
    { Regions:RegionState[]
      Food:int
      PublicOrder:int
      ReligiousOsmosis:int
      ResearchRate:int
      Growth:int }

let getStateFromBuildings buildings =
    let getSetFromRegion regionBuildings =
        let incomes =
            regionBuildings
            |> Seq.collect (fun x -> x.Incomes)
            |> Seq.toList
        let maintenance =
            regionBuildings
            |> Seq.sumBy (fun x -> x.Maintenance)
        let effect = 
            regionBuildings 
            |> Seq.map (fun x -> x.EffectSet.Effect)
            |> collectEffectsSeq
        let bonuses = 
            regionBuildings 
            |> Seq.collect (fun x -> x.EffectSet.Bonuses) 
            |> Seq.toList
        let influences = 
            regionBuildings 
            |> Seq.collect (fun x -> x.EffectSet.Influences) 
            |> Seq.toList
        (incomes, { Effect = effect; Bonuses = bonuses; Influences = influences }, maintenance)

    buildings
    |> Seq.map getSetFromRegion
    |> Seq.toList

let getState buildings settings predefinedEffect =
    let regionalEffectSets = 
        getStateFromBuildings buildings

    let effect = 
        collectEffectsSeq (predefinedEffect.Effect::(regionalEffectSets |> List.map (fun (_, x, _) -> x.Effect)))
    let provinceIncomes =
        regionalEffectSets
        |> List.collect (fun (_, x, _) -> x.Bonuses)
        |> List.append predefinedEffect.Bonuses
    let influences = 
        regionalEffectSets
        |> List.collect (fun (_, x, _) -> x.Influences)
        |> List.append predefinedEffect.Influences

    let fertility = 
        match effect.Fertility with
        | fertility when fertility < 0 -> 0
        | fertility when fertility > 5 -> 5
        | fertility -> fertility
    let food = 
        effect.RegularFood + (fertility * effect.FertilityDependentFood)
    let publicOrder = 
        effect.PublicOrder + collectInfluencesSeq settings.ReligionId influences

    let regionStates = 
        regionalEffectSets
        |> Seq.map (fun (x, y, z) -> { Sanitation = (y.Effect.RegionalSanitation + effect.ProvincialSanitation); Wealth = (collectIncomes fertility provinceIncomes x); Maintenance = float(z) })
        |> Seq.toArray

    { Regions = regionStates
      Food = food
      PublicOrder = publicOrder
      ReligiousOsmosis = effect.ReligiousOsmosis
      ResearchRate = effect.ResearchRate
      Growth = effect.Growth }