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

type FactionsData =
    JsonProvider<Sample="""
    { 
        "Id":0, 
        "Name":"a", 
        "EffectId":0, 
        "TechnologyLevels":[
            {
                "Id":0, 
                "Order":0, 
                "UniversalEffectId":0, 
                "AntilegacyEffectId":0,
                "UniversalUnlockedBuildingLevelIds": [
                  0
                ],
                "AntilegacyUnlockedBuildingLevelIds": [
                  0
                ],
                "AntilegacyLockedBuildingLevelIds": [
                  0
                ]
            }
        ]
    }
    { 
        "Id":0, 
        "Name":"a",
        "TechnologyLevels":[
            {
                "Id":0, 
                "Order":0
            }
        ]
    }
    """, SampleIsList=true>