module TWBuildingAssistant.Domain.Settings

open TWBuildingAssistant.Data.Sqlite
open Data

type NamedId = { Id: int; Name: string }

type OptionSet =
    { Provinces: NamedId list
      Weathers: NamedId list
      Seasons: NamedId list
      Religions: NamedId list
      Factions: NamedId list
      Difficulties: NamedId list
      Taxes: NamedId list
      PowerLevels: NamedId list }

type internal Settings =
    { ProvinceId: int
      FertilityDrop: int
      TechnologyTier: int
      UseAntilegacyTechnologies: bool
      ReligionId: int
      FactionId: int
      WeatherId: int
      SeasonId: int
      DifficultyId: int
      TaxId: int
      PowerLevelId: int
      CorruptionRate: int
      PiracyRate: int }

let internal getProvinceOptions (provincesData: ProvincesData.Root []) =
    let query =
        query {
            for province in provincesData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let internal getWeatherOptions (weathersData: WeathersData.Root []) =
    let query =
        query {
            for province in weathersData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let internal getSeasonOptions (seasonsData: SeasonsData.Root []) =
    let query =
        query {
            for province in seasonsData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let internal getReligionOptions (religionsData: ReligionsData.Root []) =
    let query =
        query {
            for religion in religionsData do
                select
                    { Id = religion.Id
                      Name = religion.Name }
        }

    let result = query |> Seq.toList

    result

let internal getFactionOptions (ctx: DatabaseContext) =
    let query =
        query {
            for province in ctx.Factions do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let internal getDifficultyOptions (difficultiesData: DifficultiesData.Root []) =
    let query =
        query {
            for province in difficultiesData do
                select
                    { Id = province.Id
                      Name = province.Name }
        }

    let result = query |> Seq.toList

    result

let internal getTaxOptions (taxesData: TaxesData.Root []) =
    let query =
        query {
            for tax in taxesData do
                select { Id = tax.Id; Name = tax.Name }
        }

    let result = query |> Seq.toList

    result

let internal getPowerLevelOptions (powerLevelsData: PowerLevelsData.Root []) =
    let query =
        query {
            for powerLevel in powerLevelsData do
                select
                    { Id = powerLevel.Id
                      Name = powerLevel.Name }
        }

    let result = query |> Seq.toList

    result

let getOptions (ctx: DatabaseContext) weathersData seasonsData provincesData religionsData difficultiesData taxesData powerLevelsData =
    { Provinces = getProvinceOptions provincesData
      Weathers = getWeatherOptions weathersData
      Seasons = getSeasonOptions seasonsData
      Religions = getReligionOptions religionsData
      Factions = getFactionOptions ctx
      Difficulties = getDifficultyOptions difficultiesData
      Taxes = getTaxOptions taxesData
      PowerLevels = getPowerLevelOptions powerLevelsData }
