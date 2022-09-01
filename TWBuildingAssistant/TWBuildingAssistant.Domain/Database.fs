module TWBuildingAssistant.Domain.Database

open FSharp.Data.Sql

[<Literal>]
let internal dbVendor = 
    Common.DatabaseProviderTypes.MSSQLSERVER

[<Literal>]
let internal connString = 
    "Data Source=.;Initial Catalog=twa;Integrated Security=true;TrustServerCertificate=True;"
    
[<Literal>]
let internal useOptTypes =
    Common.NullableColumnType.OPTION
    
type internal sql =
    SqlDataProvider<
        DatabaseVendor = dbVendor,
        ConnectionString = connString,
        UseOptionTypes = useOptTypes>