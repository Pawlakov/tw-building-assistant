module TWBuildingAssistant.Domain.Factions.Factions

let private getBonusTupleFromJson (jsonBonus: FactionsData.JsonTechnologyBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category

    (value, category)

let private getInfluenceTupleFromJson (jsonInfluence: FactionsData.JsonTechnologyInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    (value, religionId)

let private getTechnologyEffectSetTupleFromJson (jsonEffect: FactionsData.JsonTechnologyEffect) =
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

let private getFactionEffectSetTupleFromJson (jsonEffect: FactionsData.JsonFactionEffect) =
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


let internal getFactionEffects (factionsData: FactionsData.JsonFaction []) factionId =
    let jsonEffect = query {
            for jsonFaction in factionsData do
                where (jsonFaction.Id = factionId)
                select jsonFaction.Effect
                exactlyOne
        }

    jsonEffect 
    |> Option.map getFactionEffectSetTupleFromJson
    |> Option.toList
    |> List.toSeq

let internal getTechnologyEffects (factionsData: FactionsData.JsonFaction []) (factionId, technologyTier, useAntilegacyTechnologies) =
    
    let faction =
        query {
            for faction in factionsData do
                where (faction.Id = factionId)
                select faction
                exactlyOne
        }

    let universalEffects =
        query {
            for technologyLevel in faction.TechnologyLevels do
                where (technologyLevel.Order <= technologyTier)
                select technologyLevel.UniversalPath.Effect
        }

    let getEffectSeq = Seq.choose id >> Seq.map getTechnologyEffectSetTupleFromJson

    if useAntilegacyTechnologies then
        let antilegacyEffects =
            query {
                for technologyLevel in faction.TechnologyLevels do
                    where (technologyLevel.Order <= technologyTier)
                    select (Option.map (fun (x:FactionsData.JsonTechnologyPath) -> x.Effect) technologyLevel.AntilegacyPath)
            }

        antilegacyEffects
        |> Seq.choose id
        |> Seq.append universalEffects
        |> getEffectSeq
    else
        universalEffects
        |> getEffectSeq

let internal getFactionPairs (factionsData: FactionsData.JsonFaction []) =
    query { for faction in factionsData do select (faction.Id, faction.Name) }