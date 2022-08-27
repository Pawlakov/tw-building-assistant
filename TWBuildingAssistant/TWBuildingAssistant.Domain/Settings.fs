module TWBuildingAssistant.Domain.Settings

open FSharp.Data.Sql
open Database

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

type Settings =
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

let getProvinceOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Provinces do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getWeatherOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Weathers do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getSeasonOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Seasons do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getReligionOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Religions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getFactionOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Factions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getDifficultyOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Difficulties do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let getTaxOptions (ctx:sql.dataContext) =
    let query =
        query {
            for tax in ctx.Dbo.Taxes do
            select { Id = tax.Id; Name = tax.Name }
        }

    let result =
        query |> Seq.toList

    result

let getPowerLevelOptions (ctx:sql.dataContext) =
    let query =
        query {
            for powerLevel in ctx.Dbo.PowerLevels do
            select { Id = powerLevel.Id; Name = powerLevel.Name }
        }

    let result =
        query |> Seq.toList

    result

let getOptions () =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    { Provinces = getProvinceOptions ctx
      Weathers = getWeatherOptions ctx
      Seasons = getSeasonOptions ctx
      Religions = getReligionOptions ctx
      Factions = getFactionOptions ctx
      Difficulties = getDifficultyOptions ctx
      Taxes = getTaxOptions ctx
      PowerLevels = getPowerLevelOptions ctx }