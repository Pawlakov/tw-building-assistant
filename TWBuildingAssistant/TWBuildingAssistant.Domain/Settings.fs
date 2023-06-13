module TWBuildingAssistant.Domain.Settings

open Data

type NamedId = { Id: int; Name: string }

type NamedStringId = { StringId: string; Name: string }

type OptionSet =
    { Provinces: NamedId list
      Weathers: NamedId list
      Seasons: NamedId list
      Religions: NamedId list
      Factions: NamedStringId list
      Difficulties: NamedId list
      Taxes: NamedId list
      PowerLevels: NamedId list }

type internal Settings =
    { ProvinceId: int
      FertilityDrop: int
      TechnologyTier: int
      UseAntilegacyTechnologies: bool
      ReligionId: int
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

let private getProvinceOptions (provincesData: ProvincesData.Root []) =
    let query =
        query {
            for province in provincesData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

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

let private getReligionOptions (religionsData: ReligionsData.Root []) =
    let query =
        query {
            for religion in religionsData do
                select
                    { Id = religion.Id
                      Name = religion.Name }
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

let getOptions weathersData seasonsData provincesData religionsData difficultiesData taxesData powerLevelsData getFactionTupleSeq =
    { Provinces = getProvinceOptions provincesData
      Weathers = getWeatherOptions weathersData
      Seasons = getSeasonOptions seasonsData
      Religions = getReligionOptions religionsData
      Factions = () |> getFactionTupleSeq |> createOptions
      Difficulties = getDifficultyOptions difficultiesData
      Taxes = getTaxOptions taxesData
      PowerLevels = getPowerLevelOptions powerLevelsData }
