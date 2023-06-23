module TWBuildingAssistant.Domain.Data

open FSharp.Data

type WeathersData = JsonProvider<"Data/Weathers.json", SampleIsList=true>
type SeasonsData = JsonProvider<"Data/Seasons.json", SampleIsList=true>
type ClimatesData = JsonProvider<"Data/Climates.json", SampleIsList=true>
type TaxesData = JsonProvider<"Data/Taxes.json", SampleIsList=true>
type DifficultiesData = JsonProvider<"Data/Difficulties.json", SampleIsList=true>
type PowerLevelsData = JsonProvider<"Data/PowerLevels.json", SampleIsList=true>