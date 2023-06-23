module TWBuildingAssistant.Domain.Settings

open Data

type NamedId = { Id: int; Name: string }

type NamedStringId = { StringId: string; Name: string }

type OptionSet =
    { Provinces: NamedStringId list
      Weathers: NamedId list
      Seasons: NamedId list
      Religions: NamedStringId list
      Factions: NamedStringId list
      Difficulties: NamedId list
      Taxes: NamedId list
      PowerLevels: NamedId list }

type internal Settings =
    { ProvinceId: string
      FertilityDrop: int
      TechnologyTier: int
      UseAntilegacyTechnologies: bool
      ReligionId: string
      FactionId: string
      WeatherId: int
      SeasonId: int
      DifficultyId: int
      TaxId: int
      PowerLevelId: int
      CorruptionRate: int
      PiracyRate: int }

let private createOptions (tuples: seq<string*string>) =
    query { for (id, name) in tuples do select { StringId = id; Name = name } } |> Seq.toList

let private getWeatherOptions (weathersData: WeathersData.Root []) =
    let query =
        query {
            for province in weathersData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let private getSeasonOptions (seasonsData: SeasonsData.Root []) =
    let query =
        query {
            for province in seasonsData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let private getDifficultyOptions (difficultiesData: DifficultiesData.Root []) =
    let query =
        query {
            for province in difficultiesData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let private getTaxOptions (taxesData: TaxesData.Root []) =
    let query =
        query {
            for tax in taxesData do
                select { Id = tax.Id; Name = tax.Name }
        }

    let result = query |> Seq.toList

    result

let private getPowerLevelOptions (powerLevelsData: PowerLevelsData.Root []) =
    let query =
        query {
            for powerLevel in powerLevelsData do
                select
                    { Id = powerLevel.Id
                      Name = powerLevel.Name }
        }

    let result = query |> Seq.toList

    result

let getOptions weathersData seasonsData getProvinceTupleSeq getReligionTupleSeq difficultiesData taxesData powerLevelsData getFactionTupleSeq =
    { Provinces = () |> getProvinceTupleSeq |> createOptions
      Weathers = getWeatherOptions weathersData
      Seasons = getSeasonOptions seasonsData
      Religions = () |> getReligionTupleSeq |> createOptions
      Factions = () |> getFactionTupleSeq |> createOptions
      Difficulties = getDifficultyOptions difficultiesData
      Taxes = getTaxOptions taxesData
      PowerLevels = getPowerLevelOptions powerLevelsData }
