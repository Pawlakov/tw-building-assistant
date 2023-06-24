module TWBuildingAssistant.Domain.PowerLevels

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="PowerLevels\PowerLevels-sample.json", SampleIsList=false>

    type internal JsonPowerLevel = JsonData.PowerLevel
    type internal JsonPowerLevelEffect = JsonData.Effect
    type internal JsonPowerLevelBonus = JsonData.Bonus
    type internal JsonPowerLevelInfluence = JsonData.Influencis

    let internal getPowerLevelsData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.PowerLevels.PowerLevels.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).PowerLevels

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonPowerLevelBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonPowerLevelInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonPowerLevelEffect) =
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

let internal getPowerLevelEffect (powerLevelsData: Data.JsonPowerLevel []) (settings: Settings.Settings) =
    let effectJson =
        query {
            for powerLevel in powerLevelsData do
                where (powerLevel.Id = settings.PowerLevelId)
                select powerLevel.Effect
                head
        }

    effectJson |> createEffectSetFromJsonOption

let internal getPowerLevelPairSeq (powerLevelsData: Data.JsonPowerLevel []) =
    query { for powerLevel in powerLevelsData do select (powerLevel.Id, powerLevel.Name) }