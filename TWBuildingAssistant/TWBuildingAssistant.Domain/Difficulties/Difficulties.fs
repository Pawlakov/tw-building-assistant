module TWBuildingAssistant.Domain.Difficulties

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Difficulties\Difficulties-sample.json", SampleIsList=false>

    type internal JsonDifficult = JsonData.Difficulty
    type internal JsonDifficultEffect = JsonData.Effect
    type internal JsonDifficultBonus = JsonData.Bonus
    type internal JsonDifficultInfluence = JsonData.Influencis

    let internal getDifficultiesData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Difficulties.Difficulties.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Difficulties

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonDifficultBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonDifficultInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonDifficultEffect) =
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

let internal getDifficultyEffect (difficultiesData: Data.JsonDifficult []) (settings: Settings.Settings) =
    let effectJson =
        query {
            for difficulty in difficultiesData do
                where (difficulty.Id = settings.DifficultyId)
                select difficulty.Effect
                head
        }

    effectJson |> createEffectSetFromJsonOption

let internal getDifficultyPairSeq (difficultiesData: Data.JsonDifficult []) =
    query { for difficulty in difficultiesData do select (difficulty.Id, difficulty.Name) }