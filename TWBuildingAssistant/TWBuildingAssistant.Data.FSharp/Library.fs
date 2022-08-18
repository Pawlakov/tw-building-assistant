module TWBuildingAssistant.Data.FSharp.Library

open Donald
open Microsoft.Data.Sqlite
open Models

let getConnection () =
    new SqliteConnection "Data Source=twa_data.db"

let getProvinceOptions () =
    let result =
        let sql = "select * from Provinces"
        use conn = getConnection ()
        conn
            |> Db.newCommand sql
            |> Db.query NamedId.ofDataReader

    match result with
    | Error error -> 
        failwith "Database error"
    | Ok elements -> 
        elements |> List.toSeq