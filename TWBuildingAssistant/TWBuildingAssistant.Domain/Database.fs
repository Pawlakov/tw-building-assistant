module TWBuildingAssistant.Domain.Database

open FSharp.Data.Sql

[<Literal>]
let dbVendor = 
    Common.DatabaseProviderTypes.MSSQLSERVER

[<Literal>]
let connString = 
    "Data Source=.;Initial Catalog=twa;Integrated Security=true;TrustServerCertificate=True;"
    
[<Literal>]
let useOptTypes =
    Common.NullableColumnType.OPTION
    
type sql =
    SqlDataProvider<
        DatabaseVendor = dbVendor,
        ConnectionString = connString,
        UseOptionTypes = useOptTypes>