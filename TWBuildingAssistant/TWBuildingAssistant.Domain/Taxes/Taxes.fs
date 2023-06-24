module TWBuildingAssistant.Domain.Taxes

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Taxes\Taxes-sample.json", SampleIsList=false>

    type internal JsonTax = JsonData.Taxis
    type internal JsonTaxEffect = JsonData.Effect
    type internal JsonTaxBonus = JsonData.Bonus
    type internal JsonTaxInfluence = JsonData.Influencis

    let internal getTaxesData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Taxes.Taxes.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Taxes

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonTaxBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonTaxInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonTaxEffect) =
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

let internal getTaxEffect (taxesData: Data.JsonTax []) (settings: Settings.Settings) =
    let effectJson =
        query {
            for tax in taxesData do
                where (tax.Id = settings.TaxId)
                select tax.Effect
                head
        }

    effectJson |> createEffectSetFromJsonOption

let internal getTaxPairSeq (taxesData: Data.JsonTax []) =
    query { for tax in taxesData do select (tax.Id, tax.Name) }