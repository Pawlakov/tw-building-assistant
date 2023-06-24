module TWBuildingAssistant.Domain.Data

open FSharp.Data

type TaxesData = JsonProvider<"Data/Taxes.json", SampleIsList=true>
type DifficultiesData = JsonProvider<"Data/Difficulties.json", SampleIsList=true>
type PowerLevelsData = JsonProvider<"Data/PowerLevels.json", SampleIsList=true>