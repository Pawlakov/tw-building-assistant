module TWBuildingAssistant.Domain.Factions.FactionsData

open FSharp.Data

open System.IO
open System.Reflection

type private JsonData = JsonProvider<Sample="Factions\Factions-sample.json", SampleIsList=true>

type internal JsonFaction = JsonData.Faction
type internal JsonFactionEffect = JsonData.Effect
type internal JsonTechnologyLevel = JsonData.TechnologyLevel
type internal JsonTechnologyPath = JsonData.UniversalPath
type internal JsonTechnologyEffect = JsonData.Effect2
type internal JsonTechnologyBonus = JsonData.Bonus
type internal JsonTechnologyInfluence = JsonData.Influencis

let internal getFactionsData () =
    let assembly = Assembly.GetExecutingAssembly ()
    use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Factions.Factions.json")
    use reader = new StreamReader (stream)
    (() |> reader.ReadToEnd |> JsonData.Parse).Factions