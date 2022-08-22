module TWBuildingAssistant.Data.FSharp.Models

open System.Data

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

let getIncomeType intValue =
    match intValue with
    | 0 -> Simple
    | 1 -> Percentage
    | 2 -> FertilityDependent
    | _ -> failwith "Invalid value"

//module Income =
//    let ofDataReader (rd : IDataReader) : Income =   
//        let category = rd.ReadInt32Option "Category" |> getIncomeCategory
//        let value = rd.ReadInt32 "Value"
//        let bonusType = rd.ReadInt32 "Type" |> getIncomeType
//        match (value, category, bonusType) with
//        | (0, _, _) ->
//            failwith "'0' income."
//        | (value, Some Maintenance, _) when value > 0 ->
//            failwith "Positive 'Maintenance' income."
//        | (_, Some Maintenance, bonusType) when bonusType <> Simple ->
//            failwith "Invalid 'Maintenance' income."
//        | (_, None, bonusType) when bonusType <> Percentage ->
//            failwith "Invalid 'All' income."
//        | (_, category, FertilityDependent) when category <> Some Husbandry && category <> Some Agriculture ->
//            failwith "Invalid fertility-based income."
//        | (_, _, Simple) ->
//            { Category = category; Simple = value; Percentage = 0; FertilityDependent = 0 }
//        | (_, _, Percentage) ->
//            { Category = category; Simple = 0; Percentage = value; FertilityDependent = 0 }
//        | (_, _, FertilityDependent) ->
//            { Category = category; Simple = 0; Percentage = 0; FertilityDependent = value }

type Influence =
    { ReligionId:int option
      Value:int }

//module Influence =
//    let ofDataReader (rd : IDataReader) : Influence =    
//        let religionId = rd.ReadInt32Option "ReligionId"
//        let value = rd.ReadInt32 "Value"
//        match (religionId, value) with
//        | (_, value) when value < 1 ->
//            failwith "Negative influence."
//        | (_, _) ->
//            { ReligionId = religionId; Value = value }

type EffectSet =
    { Effect:Effect
      Incomes:Income list
      Influences:Influence list }