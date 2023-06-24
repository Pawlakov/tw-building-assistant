module TWBuildingAssistant.Domain.Settings

type NamedId = { Id: string; Name: string }

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
    { ProvinceId: string
      FertilityDrop: int
      TechnologyTier: int
      UseAntilegacyTechnologies: bool
      ReligionId: string
      FactionId: string
      WeatherId: string
      SeasonId: string
      DifficultyId: string
      TaxId: string
      PowerLevelId: string
      CorruptionRate: int
      PiracyRate: int }

let private createOptions (tuples: seq<string*string>) =
    query { for (id, name) in tuples do select { Id = id; Name = name } } |> Seq.toList

let getOptions getWeatherTupleSeq getSeasonTupleSeq getProvinceTupleSeq getReligionTupleSeq getDifficultyTupleSeq getTaxTupleSeq getPowerLevelTupleSeq getFactionTupleSeq =
    { Provinces = () |> getProvinceTupleSeq |> createOptions
      Weathers = () |> getWeatherTupleSeq |> createOptions
      Seasons = () |> getSeasonTupleSeq |> createOptions
      Religions = () |> getReligionTupleSeq |> createOptions
      Factions = () |> getFactionTupleSeq |> createOptions
      Difficulties = () |> getDifficultyTupleSeq |> createOptions
      Taxes = () |> getTaxTupleSeq |> createOptions
      PowerLevels = () |> getPowerLevelTupleSeq |> createOptions }
