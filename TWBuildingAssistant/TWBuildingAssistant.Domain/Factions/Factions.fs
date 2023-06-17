module TWBuildingAssistant.Domain.Factions

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Factions\Factions-sample.json", SampleIsList=true>

    type internal JsonFaction = JsonData.Faction
    type internal JsonFactionEffect = JsonData.Effect
    type internal JsonFactionBonus = JsonData.Bonus
    type internal JsonFactionInfluence = JsonData.Influencis
    type internal JsonTechnologyGroup = JsonData.TechnologyGroup
    type internal JsonTechnology = JsonData.Technology

    let internal getFactionsData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Factions.Factions.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Factions

let private getBonusTupleFromJson (jsonBonus: Data.JsonFactionBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category

    (value, category)

let private getInfluenceTupleFromJson (jsonInfluence: Data.JsonFactionInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    (value, religionId)

let private getEffectSetTupleFromJson (jsonEffect: Data.JsonFactionEffect) =
    let publicOrder = jsonEffect.PublicOrder |> Option.defaultValue 0
    let food = jsonEffect.Food |> Option.defaultValue 0
    let sanitation = jsonEffect.Sanitation |> Option.defaultValue 0
    let researchRate = jsonEffect.ResearchRate |> Option.defaultValue 0
    let growth = jsonEffect.Growth |> Option.defaultValue 0
    let fertility = jsonEffect.Fertility |> Option.defaultValue 0
    let religiousOsmosis = jsonEffect.ReligiousOsmosis |> Option.defaultValue 0
    let taxRate = jsonEffect.TaxRate |> Option.defaultValue 0
    let corruptionRate = jsonEffect.CorruptionRate |> Option.defaultValue 0

    let effectTuple = (publicOrder, food, sanitation, researchRate, growth, fertility, religiousOsmosis, taxRate, corruptionRate)

    let bonusTupleSeq = query { for jsonBonus in jsonEffect.Bonuses do select (getBonusTupleFromJson jsonBonus) }

    let influenceTupleSeq = query { for jsonBonus in jsonEffect.Influences do select (getInfluenceTupleFromJson jsonBonus) }

    ( effectTuple, bonusTupleSeq, influenceTupleSeq )

let private isAntilegacy (jsonTechnology: Data.JsonTechnology) =
    (Array.length jsonTechnology.NegatedIds) > 0

let private getDesiredTechnologyIdSeq useAntilegacyTechnologies (jsonTechnologySeq: seq<Data.JsonTechnology>) =
    query { 
        for technology in jsonTechnologySeq do 
            where (useAntilegacyTechnologies || technology |> isAntilegacy |> not)
            select (technology.Id, technology.RequiredIds, technology.NegatedIds)
    }

let private narrowDownTechnologyIdSeq (jsonTechnologySeq: seq<string * string array * string array>) =
    let isPossible (id, requiredIds, _) =
        let notNegated = jsonTechnologySeq |> Seq.exists (fun (_, _, otherNegatedIds) -> otherNegatedIds |> Seq.contains id) |> not
        let researchable = requiredIds |> Seq.forall (fun x -> jsonTechnologySeq |> Seq.exists (fun (otherId, _, _) -> otherId = x))
        notNegated && researchable

    jsonTechnologySeq
    |> Seq.filter isPossible

let private getEffectSetsFromTechnologyGroupSeq (jsonTechnologyIdSeq: seq<string>) (jsonTechnologyGroup: Data.JsonTechnologyGroup) =
    let allInGroupIdEffectPairs = jsonTechnologyGroup.Technologies |> Seq.map (fun x -> x.Id, x.Effect)
    let unlockedEffectPairs = allInGroupIdEffectPairs |> Seq.map (fun (x, y) -> (jsonTechnologyIdSeq |> Seq.contains x, y))

    let technologyEffecs = unlockedEffectPairs |> Seq.map snd |> Seq.choose id |> Seq.map getEffectSetTupleFromJson
    match unlockedEffectPairs |> Seq.forall (fun (x, _) -> x) with
    | true ->
        Seq.concat [technologyEffecs; (jsonTechnologyGroup.CompletionEffect |> Option.map getEffectSetTupleFromJson |> Option.toList |> List.toSeq)]
    | false ->
        technologyEffecs

let private getBuildingIdsFromTechnologyGroupSeq (jsonTechnologyIdSeq: seq<string>) (jsonTechnologyGroup: Data.JsonTechnologyGroup) =
    let allInGroupIdEffectPairs = jsonTechnologyGroup.Technologies |> Seq.map (fun technology -> technology.Id, technology.UnlockedBuildingLevelIds)
    let unlockedEffectPairs = allInGroupIdEffectPairs |> Seq.map (fun (id, buildingIds) -> (jsonTechnologyIdSeq |> Seq.contains id, buildingIds))

    unlockedEffectPairs |> Seq.map snd |> Seq.concat


let internal getFactionEffects (factionsData: Data.JsonFaction []) (settings: Settings.Settings) =
    let jsonEffect = query {
            for jsonFaction in factionsData do
                where (jsonFaction.Id = settings.FactionId)
                select jsonFaction.Effect
                exactlyOne
        }

    jsonEffect 
    |> Option.map getEffectSetTupleFromJson
    |> Option.toList
    |> List.toSeq

let internal getTechnologyEffects (factionsData: Data.JsonFaction []) (settings: Settings.Settings) =
    let faction =
        query {
            for faction in factionsData do
                where (faction.Id = settings.FactionId)
                select faction
                exactlyOne
        }

    let desiredTechnologyGroupSeq =
        query {
            for technologyGroups in faction.TechnologyGroups do
                where (technologyGroups.Order <= settings.TechnologyTier)
                select technologyGroups
        }

    let desiredTechnolgyIdSeq =
        query {
            for technologyGroup in desiredTechnologyGroupSeq do
                select (technologyGroup.Technologies |> getDesiredTechnologyIdSeq settings.UseAntilegacyTechnologies)
        } |> Seq.collect id |> narrowDownTechnologyIdSeq |> Seq.map (fun (id, _, _) -> id)

    query {
        for technologyGroup in desiredTechnologyGroupSeq do
            select (getEffectSetsFromTechnologyGroupSeq desiredTechnolgyIdSeq technologyGroup)
    } |> Seq.collect id

let internal getUnlockedBuildingLevelIds (factionsData: Data.JsonFaction []) (settings: Settings.Settings) =
    let faction =
        query {
            for faction in factionsData do
                where (faction.Id = settings.FactionId)
                select faction
                exactlyOne
        }

    let desiredTechnologyGroupSeq =
        query {
            for technologyGroups in faction.TechnologyGroups do
                where (technologyGroups.Order <= settings.TechnologyTier)
                select technologyGroups
        }

    let desiredTechnolgyIdSeq =
        query {
            for technologyGroup in desiredTechnologyGroupSeq do
                select (technologyGroup.Technologies |> getDesiredTechnologyIdSeq settings.UseAntilegacyTechnologies)
        } |> Seq.collect id |> narrowDownTechnologyIdSeq |> Seq.map (fun (id, _, _) -> id)

    let unlockedFromTechnologiesIds = 
        query {
            for technologyGroup in desiredTechnologyGroupSeq do
                select (getBuildingIdsFromTechnologyGroupSeq desiredTechnolgyIdSeq technologyGroup)
        } |> Seq.collect id

    Seq.concat [unlockedFromTechnologiesIds; faction.StartingBuildingLevelIds]

let internal getUsedBuildingBranchIds (factionsData: Data.JsonFaction []) (settings: Settings.Settings) =
    query {
            for jsonFaction in factionsData do
                where (jsonFaction.Id = settings.FactionId)
                select jsonFaction.UsedBuildingBranchIds
                exactlyOne
        } |> Seq.ofArray

let internal getFactionPairs (factionsData: Data.JsonFaction []) =
    query { for faction in factionsData do select (faction.Id, faction.Name) }