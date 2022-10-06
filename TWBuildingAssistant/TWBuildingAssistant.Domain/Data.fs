module TWBuildingAssistant.Domain.Data

open FSharp.Data

type ResourcesData = JsonProvider<"Data/Resources.json", SampleIsList=true>
type WeathersData = JsonProvider<"Data/Weathers.json", SampleIsList=true>
type SeasonsData = JsonProvider<"Data/Seasons.json", SampleIsList=true>
type ClimatesData = JsonProvider<"Data/Climates.json", SampleIsList=true>

type ProvincesData =
    JsonProvider<Sample="""
    { 
        "Id":0, 
        "Name":"a", 
        "ClimateId":0, 
        "EffectId":0, 
        "City":
        { 
            "Id":0, 
            "Name":"a", 
            "IsCoastal":true, 
            "ResourceId":0,
            "SlotsCountOffset":0
        }, 
        "TownFirst":
        { 
            "Id":0, 
            "Name":"a", 
            "IsCoastal":true, 
            "ResourceId":0,
            "SlotsCountOffset":0
        }, 
        "TownSecond":
        { 
            "Id":0, 
            "Name":"a", 
            "IsCoastal":true, 
            "ResourceId":0,
            "SlotsCountOffset":0
        } 
    }
    { 
        "Id":0, 
        "Name":"a", 
        "ClimateId":0, 
        "EffectId":0, 
        "City":
        { 
            "Id":0, 
            "Name":"a"
        }, 
        "TownFirst":
        { 
            "Id":0, 
            "Name":"a"
        }, 
        "TownSecond":
        { 
            "Id":0, 
            "Name":"a"
        } 
    }
    """, SampleIsList=true>

type ReligionsData = JsonProvider<"Data/Religions.json", SampleIsList=true>
type TaxesData = JsonProvider<"Data/Taxes.json", SampleIsList=true>
type DifficultiesData = JsonProvider<"Data/Difficulties.json", SampleIsList=true>
type PowerLevelsData = JsonProvider<"Data/PowerLevels.json", SampleIsList=true>