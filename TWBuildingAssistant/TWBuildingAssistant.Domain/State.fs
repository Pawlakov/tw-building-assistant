module TWBuildingAssistant.Domain.State

open FSharp.Data.Sql
open Buildings
open Effects
open Settings

type RegionState = 
    { Sanitation:int
      Food:int
      Wealth:float
      Maintenance:float }

type ProvinceState = 
    { Regions:RegionState[]
      TotalFood:int
      TotalWealth:float
      TotalIncome:float
      PublicOrder:int
      ReligiousOsmosis:int
      ResearchRate:int
      Growth:int }

let gatherRegionLocalEffects regionBuildings =
    let localEffect =
        regionBuildings 
        |> Seq.map (fun x -> x.LocalEffectSet.LocalEffect)
        |> collectLocalEffectsSeq
    let incomes =
        regionBuildings
        |> Seq.collect (fun x -> x.LocalEffectSet.Incomes)
        |> Seq.toList
    { LocalEffect = localEffect; Incomes = incomes }

let gatherEffects buildings predefinedEffect =
    let provinceBuildings =
        buildings
        |> Seq.collect (fun x -> x)
    
    let effect = 
        provinceBuildings 
        |> Seq.map (fun x -> x.EffectSet.Effect)
        |> Seq.append (Seq.singleton predefinedEffect.Effect)
        |> collectEffectsSeq

    let bonuses = 
        provinceBuildings 
        |> Seq.collect (fun x -> x.EffectSet.Bonuses) 
        |> Seq.append predefinedEffect.Bonuses
        |> Seq.toList

    let influences = 
        provinceBuildings 
        |> Seq.collect (fun x -> x.EffectSet.Influences) 
        |> Seq.append predefinedEffect.Influences
        |> Seq.toList

    { Effect = effect; Bonuses = bonuses; Influences = influences }

let getRegionState effectSet localEffectSet =
    let sanitation = 
        localEffectSet.LocalEffect.Sanitation + effectSet.Effect.Sanitation

    let fertility =
        match effectSet.Effect.Fertility with
        | fertility when fertility < 0 -> 0
        | fertility when fertility > 5 -> 5
        | fertility -> fertility

    let food =
        localEffectSet.LocalEffect.Food + (localEffectSet.LocalEffect.FoodFromFertility * fertility)

    let wealth =
        collectIncomes fertility effectSet.Bonuses localEffectSet.Incomes

    let maintenance =
        localEffectSet.LocalEffect.Maintenance |> float

    { Sanitation = sanitation
      Food = food
      Wealth = wealth
      Maintenance = maintenance }

let getState buildings settings predefinedEffectSet =
    let provinceEffectSet = 
        gatherEffects buildings predefinedEffectSet

    let regionStates =
        buildings
        |> Seq.map (gatherRegionLocalEffects >> (getRegionState provinceEffectSet))
        |> Seq.toArray

    let totalFood = 
        provinceEffectSet.Effect.Food + Array.sumBy (fun x -> x.Food) regionStates

    let publicOrder = 
        provinceEffectSet.Effect.PublicOrder + collectInfluencesSeq settings.ReligionId provinceEffectSet.Influences

    let totalWealth =
        Array.sumBy (fun x -> x.Wealth) regionStates

    let totalIncome =
        totalWealth + Array.sumBy (fun x -> x.Maintenance) regionStates

    { Regions = regionStates
      TotalFood = totalFood
      TotalWealth = totalWealth
      TotalIncome = totalIncome
      PublicOrder = publicOrder
      ReligiousOsmosis = provinceEffectSet.Effect.ReligiousOsmosis
      ResearchRate = provinceEffectSet.Effect.ResearchRate
      Growth = provinceEffectSet.Effect.Growth }