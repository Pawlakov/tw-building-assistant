module TWBuildingAssistant.Data.FSharp.Settings

open FSharp.Data.Sql
open Database
open Models

let getProvinceOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Provinces do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getWeatherOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Weathers do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getSeasonOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Seasons do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getReligionOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Religions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getFactionOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Factions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getDifficultyOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Difficulties do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getTaxOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let query =
        query {
            for province in ctx.Dbo.Taxes do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result