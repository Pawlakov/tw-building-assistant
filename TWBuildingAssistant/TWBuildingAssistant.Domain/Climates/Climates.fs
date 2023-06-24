module TWBuildingAssistant.Domain.Climates

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Climates\Climates-sample.json", SampleIsList=false>

    type internal JsonClimate = JsonData.Climate
    type internal JsonSeasonEffect = JsonData.Effect
    type internal JsonWeatherEffect = JsonData.Effect2
    type internal JsonClimateEffect = JsonData.Effect3
    type internal JsonClimateBonus = JsonData.Bonus
    type internal JsonClimateInfluence = JsonData.Influencis

    let internal getClimatesData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Climates.Climates.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Climates

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonClimateBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonClimateInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonClimateEffect) =
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

let private createEffectSetFromJsonOption = Option.map createEffectSetFromJson >> Option.defaultValue Effects.emptyEffectSet
//

let internal getClimateEffect (climatesData: Data.JsonClimate []) getProvinceClimateId (settings: Settings.Settings) =
    let climateId = settings |> getProvinceClimateId
    
    let climate =
        query {
            for climate in climatesData do
                where (climate.Id = climateId)
                select climate
                head
        }

    let jsonEffect =
        climate.Effects
        |> Seq.tryFind (fun x -> x.SeasonId = settings.SeasonId)
        |> Option.bind (fun x ->
            x.Effects
            |> Seq.tryFind (fun y -> y.WeatherId = settings.WeatherId)
            |> Option.map (fun z -> z.Effect))

    jsonEffect |> createEffectSetFromJsonOption