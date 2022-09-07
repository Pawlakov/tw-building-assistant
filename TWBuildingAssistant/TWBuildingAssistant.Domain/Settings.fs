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

let internal getProvinceOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Provinces do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getWeatherOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Weathers do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getSeasonOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Seasons do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getReligionOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Religions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getFactionOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Factions do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getDifficultyOptions (ctx:sql.dataContext) =
    let query =
        query {
            for province in ctx.Dbo.Difficulties do
            select { Id = province.Id; Name = province.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getTaxOptions (ctx:sql.dataContext) =
    let query =
        query {
            for tax in ctx.Dbo.Taxes do
            select { Id = tax.Id; Name = tax.Name }
        }

    let result =
        query |> Seq.toList

    result

let internal getPowerLevelOptions (ctx:sql.dataContext) =
    let query =
        query {
            for powerLevel in ctx.Dbo.PowerLevels do
            select { Id = powerLevel.Id; Name = powerLevel.Name }
        }

    let result =
        query |> Seq.toList

    result

let getOptions (connString:string option) =
    let ctx =
        match connString with
        | None ->
            sql.GetDataContext SelectOperations.DatabaseSide
        | Some someConnString ->
            sql.GetDataContext (someConnString, SelectOperations.DatabaseSide)

    { Provinces = getProvinceOptions ctx
      Weathers = getWeatherOptions ctx
      Seasons = getSeasonOptions ctx
      Religions = getReligionOptions ctx
      Factions = getFactionOptions ctx
      Difficulties = getDifficultyOptions ctx
      Taxes = getTaxOptions ctx
      PowerLevels = getPowerLevelOptions ctx }