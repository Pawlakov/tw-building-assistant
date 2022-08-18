module TWBuildingAssistant.Data.FSharp.Library

open Donald
open Microsoft.Data.Sqlite
open Models

let getIdNameOptions tableName =
    let result =
        let sql = tableName |> sprintf "select * from %s"
        use conn = new SqliteConnection "Data Source=twa_data.db"
        conn
            |> Db.newCommand sql
            |> Db.query NamedId.ofDataReader

    match result with
    | Error error -> 
        failwith "Database error"
    | Ok elements -> 
        elements |> List.toSeq

let getProvinceOptions () =
    getIdNameOptions "Provinces"

let getWeatherOptions () =
    getIdNameOptions "Weathers"

let getSeasonOptions () =
    getIdNameOptions "Seasons"

let getReligionOptions () =
    getIdNameOptions "Religions"

let getFactionOptions () =
    getIdNameOptions "Factions"

let getDifficultyOptions () =
    getIdNameOptions "Difficulties"

let getTaxOptions () =
    getIdNameOptions "Taxes"