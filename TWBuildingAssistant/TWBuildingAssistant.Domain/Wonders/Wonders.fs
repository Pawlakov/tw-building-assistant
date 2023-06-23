module TWBuildingAssistant.Domain.Wonders

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Wonders\Wonders-sample.json", SampleIsList=false>

    type internal JsonWonder = JsonData.Wonder
    type internal JsonWonderEffect = JsonData.FactionwideEffect
    type internal JsonWonderBonus = JsonData.Bonus
    type internal JsonWonderInfluence = JsonData.Influencis

    let internal getWondersData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Wonders.Wonders.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Wonders

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonWonderBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonWonderInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonWonderEffect) =
    let publicOrder = jsonEffect.PublicOrder |> Option.defaultValue 0
    let food = jsonEffect.Food |> Option.defaultValue 0
    let sanitation = jsonEffect.Sanitation |> Option.defaultValue 0
    let researchRate = jsonEffect.ResearchRate |> Option.defaultValue 0
    let growth = jsonEffect.Growth |> Option.defaultValue 0
    let fertility = jsonEffect.Fertility |> Option.defaultValue 0
    let religiousOsmosis = jsonEffect.ReligiousOsmosis |> Option.defaultValue 0
    let taxRate = jsonEffect.TaxRate |> Option.defaultValue 0
    let corruptionRate = jsonEffect.CorruptionRate |> Option.defaultValue 0

    let effect = Effects.createEffect publicOrder food sanitation researchRate growth fertility religiousOsmosis taxRate corruptionRate

    let bonusSeq = jsonEffect.Bonuses |> Seq.map createBonusFromJson

    let influenceSeq = jsonEffect.Influences |> Seq.map createInfluenceFromJson

    Effects.createEffectSet effect bonusSeq influenceSeq
//

let internal getWonderEffectSeq (wondersData: Data.JsonWonder []) getProvinceRegionIdSeq (settings: Settings.Settings) =
    let regionIdSeq:seq<string> = settings |> getProvinceRegionIdSeq
    let intersection = regionIdSeq |> Set.ofSeq |> Set.intersect

    let (jsonLocalEffect, jsonFactionWideElement) = query {
            for jsonWonder in wondersData do
                where ((jsonWonder.RegionIds |> Set.ofSeq |> intersection |> Set.count) > 0)
                select (jsonWonder.LocalEffect, jsonWonder.FactionwideEffect)
                exactlyOne
        }

    [jsonLocalEffect; jsonFactionWideElement]
    |> Seq.choose id
    |> Seq.map createEffectSetFromJson