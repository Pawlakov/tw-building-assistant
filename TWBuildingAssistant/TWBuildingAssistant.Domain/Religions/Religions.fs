module TWBuildingAssistant.Domain.Religions

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Religions\Religions-sample.json", SampleIsList=false>

    type internal JsonReligion = JsonData.Religion
    type internal JsonReligionEffect = JsonData.Effect
    type internal JsonReligionBonus = JsonData.Bonus
    type internal JsonReligionInfluence = JsonData.Influencis

    let internal getReligionsData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Religions.Religions.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Religions

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonReligionBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonReligionInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonReligionEffect) =
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

let internal getReligionEffect (religionsData: Data.JsonReligion []) (settings: Settings.Settings) =
    let effectJson =
        query {
            for religion in religionsData do
                where (religion.Id = settings.ReligionId)
                select religion.Effect
                head
        }

    effectJson |> createEffectSetFromJsonOption

let internal getReligionPairs (religionsData: Data.JsonReligion []) =
    query { for religion in religionsData do select (religion.Id, religion.Name) }