module TWBuildingAssistant.Domain.State

open FSharp.Data.Sql
open Buildings
open Effects
open Settings

type RegionState = 
    { Sanitation:int
      Wealth:float }

type ProvinceState = 
    { Regions:RegionState[]
      Food:int
      PublicOrder:int
      ReligiousOsmosis:int
      ResearchRate:int
      Growth:int }

let getStateFromBuildings buildings =
    let getSetFromRegion regionBuildings =
        { Effect = collectEffectsSeq (regionBuildings |> Seq.map (fun x -> x.EffectSet.Effect))
          RegionIncomes = (regionBuildings |> Seq.collect (fun x -> x.EffectSet.RegionIncomes) |> Seq.toList)
          ProvinceIncomes = (regionBuildings |> Seq.collect (fun x -> x.EffectSet.ProvinceIncomes) |> Seq.toList)
          Influences = (regionBuildings |> Seq.collect (fun x -> x.EffectSet.Influences) |> Seq.toList) }

    buildings
    |> Seq.map getSetFromRegion
    |> Seq.toList

let getState buildings settings predefinedEffect =
    let regionalEffectSets = 
        getStateFromBuildings buildings

    let effect = 
        collectEffectsSeq (predefinedEffect.Effect::(regionalEffectSets |> List.map (fun x -> x.Effect)))
    let provinceIncomes =
        regionalEffectSets
        |> List.collect (fun x -> x.ProvinceIncomes)
        |> List.append predefinedEffect.ProvinceIncomes
    let influences = 
        regionalEffectSets
        |> List.collect (fun x -> x.Influences)
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
        |> Seq.map (fun x -> { Sanitation = (x.Effect.RegionalSanitation + effect.ProvincialSanitation); Wealth = (collectIncomes fertility ([x.RegionIncomes; provinceIncomes] |> List.collect (fun x -> x))) })
        |> Seq.toArray

    { Regions = regionStates
      Food = food
      PublicOrder = publicOrder
      ReligiousOsmosis = effect.ReligiousOsmosis
      ResearchRate = effect.ResearchRate
      Growth = effect.Growth }