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
