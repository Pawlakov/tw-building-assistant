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
        "Factions":[{
            "Id":0, 
            "Name":"a", 
            "EffectId":0, 
            "UsedBuildingBranchIds":["a"],
            "TechnologyLevels":[
                {
                    "Id":0, 
                    "Order":0, 
                    "UniversalEffectId":0, 
                    "AntilegacyEffectId":0,
                    "UniversalUnlockedBuildingLevelIds": [
                      "a"
                    ],
                    "AntilegacyUnlockedBuildingLevelIds": [
                      "a"
                    ],
                    "AntilegacyLockedBuildingLevelIds": [
                      "a"
                    ]
                }
            ]
        }]
    }
    { 
        "Factions":[{
            "Id":0, 
            "Name":"a",
            "UsedBuildingBranchIds":["a"],
            "TechnologyLevels":[
                {
                    "Id":0, 
                    "Order":0
                }
            ]
        }]
    }
    """, SampleIsList=true>

type internal JsonFaction = FactionsData.Faction
type internal JsonTechnologyLevel = FactionsData.TechnologyLevel

type BuildingsData =
    JsonProvider<Sample="""
    {
        "Branches":[{
            "Id":"a",
            "Name":"a",
            "SlotType":0,
            "RegionType":0,
            "ReligionId":0,
            "ResourceId":0,
            "AllowParallel":true,
            "Interesting":true,
            "Levels":[{
                "Id":"a",
                "ParentId": "a",
                "Name":"a",
                "EffectId":0,
                "Maintenance":0,
                "LocalFood": 0,
                "LocalFoodFromFertility": 0,
                "LocalSanitation": 0,
                "CapitalTier": 0,
                "Incomes": [{
                    "Value":0,
                    "Category":0,
                    "IsFertilityDependent":true
                }]
            }]
        }]
    }
    {
        "Branches":[{
            "Id":"a",
            "Name":"a",
            "SlotType":0,
            "Levels":[{
                "Id":"a",
                "Name":"a",
                "Incomes": [{
                    "Value":0,
                    "Category":0
                }]
            }]
        }]
    }
    """, SampleIsList=true>

type internal JsonBuildingBranch = BuildingsData.Branch
type internal JsonBuildingLevel = BuildingsData.Level
type internal JsonBuildingIncome = BuildingsData.Income