module TWBuildingAssistant.Domain.Settings

open TWBuildingAssistant.Data.Sqlite

type NamedId = 
    { Id:int
      Name:string }

type OptionSet =
    { Provinces:NamedId list
      Weathers:NamedId list
      Seasons:NamedId list
      Religions:NamedId list
      Factions:NamedId list
      Difficulties:NamedId list
      Taxes:NamedId list
      PowerLevels:NamedId list }

type internal Settings =
    { ProvinceId:int
      FertilityDrop:int 
      TechnologyTier:int 
      UseAntilegacyTechnologies:bool 
      ReligionId:int
      FactionId:int
      WeatherId:int
      SeasonId:int
      DifficultyId:int
      TaxId:int
      PowerLevelId:int
      CorruptionRate:int
      PiracyRate:int }

let internal getProvinceOptions (ctx:DatabaseContext) =
    let query =
        query {
            for province in ctx.Provinces do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getWeatherOptions (ctx:DatabaseContext) =
    let query =
        query {
            for province in ctx.Weathers do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getSeasonOptions (ctx:DatabaseContext) =
    let query =
        query {
            for province in ctx.Seasons do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getReligionOptions (ctx:DatabaseContext) =
    let query =
        query {
            for province in ctx.Religions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getFactionOptions (ctx:DatabaseContext) =
    let query =
        query {
            for province in ctx.Factions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getDifficultyOptions (ctx:DatabaseContext) =
    let query =
        query {
            for province in ctx.Difficulties do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getTaxOptions (ctx:DatabaseContext) =
    let query =
        query {
            for tax in ctx.Taxes do
            select { Id = tax.Id; Name = tax.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getPowerLevelOptions (ctx:DatabaseContext) =
    let query =
        query {
            for powerLevel in ctx.PowerLevels do
            select { Id = powerLevel.Id; Name = powerLevel.Name }
        }

    let result =
        query |> Seq.toList

    result

let getOptions (ctx:DatabaseContext) =
    { Provinces = getProvinceOptions ctx
      Weathers = getWeatherOptions ctx
      Seasons = getSeasonOptions ctx
      Religions = getReligionOptions ctx
      Factions = getFactionOptions ctx
      Difficulties = getDifficultyOptions ctx
      Taxes = getTaxOptions ctx
      PowerLevels = getPowerLevelOptions ctx }