module TWBuildingAssistant.Domain.Seasons

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Seasons\Seasons-sample.json", SampleIsList=false>

    type internal JsonSeason = JsonData.Season

    let internal getSeasonsData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Seasons.Seasons.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Seasons

let internal getSeasonPairSeq (seasonsData: Data.JsonSeason []) =
    query { for season in seasonsData do select (season.Id, season.Name) }