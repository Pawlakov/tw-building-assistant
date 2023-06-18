module TWBuildingAssistant.Domain.Buildings

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Buildings\Buildings-sample.json", SampleIsList=false>

    type internal JsonBuildingBranch = JsonData.Branch
    type internal JsonBuildingLevel = JsonData.Level
    type internal JsonBuildingEffect = JsonData.Effect
    type internal JsonBuildingBonus = JsonData.Bonus
    type internal JsonBuildingInfluence = JsonData.Influencis
    type internal JsonBuildingIncome = JsonData.Income

    let private getBuildingsDataSingleFile fileName =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream (fileName)
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Branches

    let internal getBuildingsData () =
        let all = "TWBuildingAssistant.Domain.Buildings.BuildingsAll.json" |> getBuildingsDataSingleFile
        let resource = "TWBuildingAssistant.Domain.Buildings.BuildingsResource.json" |> getBuildingsDataSingleFile
        let religion = "TWBuildingAssistant.Domain.Buildings.BuildingsReligion.json" |> getBuildingsDataSingleFile
        let roman = "TWBuildingAssistant.Domain.Buildings.BuildingsRoman.json" |> getBuildingsDataSingleFile
        let romanWest = "TWBuildingAssistant.Domain.Buildings.BuildingsRomanWest.json" |> getBuildingsDataSingleFile

        Array.concat [all; resource; religion; roman; romanWest]

type internal BuildingLevel =
    { Id: string
      Name: string
      LocalEffectSet: Effects.LocalEffectSet
      EffectSet: Effects.EffectSet }

type internal BuildingBranch =
    { Id: string
      Name: string
      Interesting: bool
      Levels: BuildingLevel [] }

type internal BuildingLibraryEntry =
    { Descriptor: Provinces.SlotDescriptor
      BuildingBranches: BuildingBranch [] }


let internal emptyBuildingLevel =
    { Id = ""
      Name = "Empty"
      LocalEffectSet = Effects.emptyLocalEffectSet
      EffectSet = Effects.emptyEffectSet }

let internal emptyBuildingBranch =
    { Id = ""
      Name = "Empty"
      Interesting = true
      Levels = [| emptyBuildingLevel |] }

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonBuildingBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonBuildingInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonBuildingEffect) =
    let publicOrder = jsonEffect.PublicOrder |> Option.defaultValue 0
    let food = jsonEffect.Food |> Option.defaultValue 0
    let sanitation = jsonEffect.Sanitation |> Option.defaultValue 0
    let researchRate = jsonEffect.ResearchRate |> Option.defaultValue 0
    let growth = jsonEffect.Growth |> Option.defaultValue 0
    let fertility = jsonEffect.Fertility |> Option.defaultValue 0
    let religiousOsmosis = jsonEffect.ReligiousOsmosis |> Option.defaultValue 0
    let taxRate = jsonEffect.TaxRate |> Option.defaultValue 0
    let corruptionRate = jsonEffect.CorruptionRate |> Option.defaultValue 0

    let effect = Effects.createEffect publicOrder food sanitation researchRate growth fertility religiousOsmosis taxRate corruptionRate

    let bonusSeq = jsonEffect.Bonuses |> Seq.map createBonusFromJson

    let influenceSeq = jsonEffect.Influences |> Seq.map createInfluenceFromJson

    Effects.createEffectSet effect bonusSeq influenceSeq

let private createEffectSetFromJsonOption = Option.map createEffectSetFromJson >> Option.defaultValue Effects.emptyEffectSet

let private createIncomeFromJson (jsonIncome: Data.JsonBuildingIncome) =
    let value = jsonIncome.Value
    let category = jsonIncome.Category |> Effects.createIncomeCategory
    let isFertilityDependent = jsonIncome.IsFertilityDependent |> Option.defaultValue false

    Effects.createIncome value category isFertilityDependent

let private createLocalEffectSetFromJson (jsonBuildingLevel: Data.JsonBuildingLevel) =
    let maintenance = jsonBuildingLevel.Maintenance |> Option.defaultValue 0
    let food = jsonBuildingLevel.LocalFood |> Option.defaultValue 0
    let foodFromFertility = jsonBuildingLevel.LocalFoodFromFertility |> Option.defaultValue 0
    let sanitation = jsonBuildingLevel.LocalSanitation |> Option.defaultValue 0
    let capitalTier = jsonBuildingLevel.CapitalTier |> Option.defaultValue 0
    
    let effect = Effects.createLocalEffect maintenance food foodFromFertility sanitation capitalTier

    let incomeSeq = jsonBuildingLevel.Incomes |> Seq.map createIncomeFromJson

    Effects.createLocalEffectSet effect incomeSeq
//

let private getBuildingLibraryEntry (buildingsData: Data.JsonBuildingBranch []) (usedBuildingBranchIdSeq: seq<string>) (unlockedBuildingLevelIdSeq: seq<string>) (settings: Settings.Settings) (descriptor: Provinces.SlotDescriptor) =
    let slotTypeInt =
        match descriptor.SlotType with
        | Provinces.Main -> 0
        | Provinces.Coastal -> 1
        | Provinces.General -> 2

    let regionTypeInt =
        match descriptor.RegionType with
        | Provinces.City -> 0
        | Provinces.Town -> 1

    let usedBranches =
        query {
            for branch in buildingsData do
                where (Seq.contains branch.Id usedBuildingBranchIdSeq)
                where (branch.SlotType = slotTypeInt)
                where (branch.RegionType = None || branch.RegionType = Some regionTypeInt)
                where (branch.ReligionId = None || branch.ReligionId = Some settings.ReligionId)
                where (branch.ResourceId = None || branch.ResourceId = descriptor.ResourceId)
        }

    let firstLoop strainPairs (branch: Data.JsonBuildingBranch) =
        let rec traverseStrain (current: Data.JsonBuildingLevel option) (accumulated: Data.JsonBuildingLevel list) =
            match current with
            | None -> accumulated
            | Some current ->
                let parent = 
                    match current.ParentId with
                    | None -> None
                    | Some parentId -> 
                        branch.Levels 
                        |> Array.filter (fun x -> x.Id = parentId) 
                        |> Array.exactlyOne
                        |> Some
                traverseStrain parent (current::accumulated)

        let isCrownLevel (level: Data.JsonBuildingLevel) =
            branch.Levels 
            |> Array.forall (fun x -> (Some level.Id) <> x.ParentId)

        match branch.AllowParallel with
        | Some true ->
            let crownLevels = 
                branch.Levels |> Array.filter isCrownLevel
                |> Array.toList

            let strains = 
                crownLevels 
                |> List.map (fun x -> traverseStrain (Some x) [])

            (strains |> List.map (fun x -> (branch, x))) @ strainPairs
        | _ -> 
            (branch, branch.Levels |> Array.toList) :: strainPairs

    let strainPairs = usedBranches |> Seq.fold firstLoop []

    let secondLoop finalDictionary ((branch: Data.JsonBuildingBranch), (levels:Data.JsonBuildingLevel list)) =
        let levelsOther =
            levels
            |> List.filter (fun x -> 
                Seq.contains x.Id unlockedBuildingLevelIdSeq)
            |> List.map (fun level ->
                { Id = level.Id
                  Name = level.Name
                  LocalEffectSet = (level |> createLocalEffectSetFromJson)
                  EffectSet = (level.Effect |> createEffectSetFromJsonOption) }: BuildingLevel)
            |> List.toArray

        match levelsOther with
        | [||] -> finalDictionary
        | _ ->
            { Id = branch.Id
              Name = branch.Name
              Interesting = branch.Interesting = Some true
              Levels = levelsOther }
            :: finalDictionary

    let finalDictionary = strainPairs |> List.fold secondLoop []

    let finalFinalDictionary =
        match descriptor.SlotType with
        | Provinces.General -> emptyBuildingBranch :: finalDictionary
        | _ -> finalDictionary

    { Descriptor = descriptor
      BuildingBranches = (finalFinalDictionary |> List.toArray) }


let internal getBuildingLibrary buildingsData getUsedBuildingBranchIdSeq getUnlockedBuildingLevelIdSeq getSlotDescriptorSeq settings =

    let usedBuildingBranchIdSeq = settings |> getUsedBuildingBranchIdSeq

    let unlockedBuildingLevelIdSeq = settings |> getUnlockedBuildingLevelIdSeq

    let slotDescriptors = settings |> getSlotDescriptorSeq

    let results =
        slotDescriptors
        |> Seq.map (getBuildingLibraryEntry buildingsData usedBuildingBranchIdSeq unlockedBuildingLevelIdSeq settings)
        |> Seq.toArray

    results

let internal getBuildingLevel (buildingsData: Data.JsonBuildingBranch []) id =
    let levels = buildingsData |> Seq.collect (fun x -> x.Levels)
    let level =
        query {
            for level in levels do
                where (level.Id = id)
                head
        }

    { Id = level.Id
      Name = level.Name
      LocalEffectSet = (level |> createLocalEffectSetFromJson)
      EffectSet = (level.Effect |> createEffectSetFromJsonOption) }: BuildingLevel