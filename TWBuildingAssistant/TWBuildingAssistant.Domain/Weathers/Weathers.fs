module TWBuildingAssistant.Domain.Weathers

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Weathers\Weathers-sample.json", SampleIsList=false>

    type internal JsonWeather = JsonData.Weather

    let internal getWeathersData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Weathers.Weathers.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Weathers

let internal getWeatherPairSeq (weathersData: Data.JsonWeather []) =
    query { for weather in weathersData do select (weather.Id, weather.Name) }