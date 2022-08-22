module TWBuildingAssistant.Data.FSharp.Library

open FSharp.Data.Sql
open Models

[<Literal>]
let dbVendor = 
    Common.DatabaseProviderTypes.MSSQLSERVER

[<Literal>]
let connString = 
    "Data Source=.;Initial Catalog=twa;Integrated Security=true;TrustServerCertificate=True;"
    
[<Literal>]
let useOptTypes =
    Common.NullableColumnType.VALUE_OPTION
    
type sql =
    SqlDataProvider<
        DatabaseVendor = dbVendor,
        ConnectionString = connString,
        UseOptionTypes = useOptTypes>

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

//let getEffect effectId =
//    match effectId with
//    | Some effectId ->
//        let effectQueryResult = 
//            let sql = "
//            select *
//            from Effects 
//            where Id = @id"

//            use conn = getConnection ()
//            conn
//                |> Db.newCommand sql
//                |> Db.setParams ["id", SqlType.Int effectId]
//                |> Db.querySingle Effect.ofDataReader

//        let incomeQueryResult = 
//            let sql = "
//            select *
//            from Bonuses 
//            where EffectId = @id"

//            use conn = getConnection ()
//            conn
//                |> Db.newCommand sql
//                |> Db.setParams ["id", SqlType.Int effectId]
//                |> Db.query Income.ofDataReader

//        let influenceQueryResult = 
//            let sql = "
//            select *
//            from Influences 
//            where EffectId = @id"

//            use conn = getConnection ()
//            conn
//                |> Db.newCommand sql
//                |> Db.setParams ["id", SqlType.Int effectId]
//                |> Db.query Influence.ofDataReader

//        let effect = 
//            match effectQueryResult with
//            | Error error -> 
//                failwith "Database error"
//            | Ok element -> 
//                match element with
//                | None ->
//                    failwith "Database error"
//                | Some element ->
//                    element
        
//        let incomes = 
//            match incomeQueryResult with
//            | Error error -> 
//                failwith "Database error"
//            | Ok elements -> 
//                elements
        
//        let influences = 
//            match influenceQueryResult with
//            | Error error -> 
//                failwith "Database error"
//            | Ok elements -> 
//                elements

//        Some { Effect = effect; Incomes = incomes; Influences = influences }
//    | None ->
//        None