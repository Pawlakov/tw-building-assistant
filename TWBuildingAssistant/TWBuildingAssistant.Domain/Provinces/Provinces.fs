module TWBuildingAssistant.Domain.Provinces

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Provinces\Provinces-sample.json", SampleIsList=false>

    type internal JsonProvince = JsonData.Provincis
    type internal JsonRegion = JsonData.City
    type internal JsonProvinceEffect = JsonData.Effect
    type internal JsonProvinceBonus = JsonData.Bonus
    type internal JsonProvinceInfluence = JsonData.Influencis

    let internal getProvincesData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Provinces.Provinces.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Provinces

type internal RegionType =
    | City
    | Town

type internal SlotType =
    | Main
    | Coastal
    | General

type internal SlotDescriptor =
    { SlotType:SlotType
      RegionType:RegionType
      ResourceId:string option }

type internal Region =
    { Id:string
      Name:string
      RegionType:RegionType
      ResourceId:string option
      ResourceName: string option
      Slots:SlotDescriptor[] }

type internal Province =
    { Id:string 
      Name:string
      Regions:Region[] }

// Constructors
let private createBonusFromJson (jsonBonus: Data.JsonProvinceBonus) =
    let value = jsonBonus.Value
    let category = jsonBonus.Category |> Effects.createIncomeCategoryOption

    Effects.createBonus value category

let private createInfluenceFromJson (jsonInfluence: Data.JsonProvinceInfluence) =
    let religionId = jsonInfluence.ReligionId
    let value = jsonInfluence.Value

    Effects.createInfluence value religionId

let private createEffectSetFromJson (jsonEffect: Data.JsonProvinceEffect) =
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

let internal createRegion id name regionType isCoastal resourceId resourceName missingSlot =
    match (id, name) with
    | "", _ -> failwith "Region without id."
    | _, "" -> failwith "Region without name."
    | _ ->
        let slotCount = 
            match (regionType, missingSlot) with
            | City, false -> 6
            | City, true -> 5
            | Town, false -> 4
            | Town, true -> 3

        let createSlot index =
            match (index, isCoastal) with
            | 1, _ ->
                { SlotType = Main; RegionType = regionType; ResourceId = resourceId }
            | 2, true ->
                { SlotType = Coastal; RegionType = regionType; ResourceId = resourceId }
            | _ ->
                { SlotType = General; RegionType = regionType; ResourceId = resourceId }

        let slots = 
            seq {
                for i in 1 .. slotCount ->
                createSlot i }
            |> Seq.toArray

        { Id = id; Name = name; RegionType = regionType; ResourceId = resourceId; ResourceName = resourceName; Slots = slots }

let internal createProvince id name city townFirst townSecond =
    match (id, name) with
    | "", _ ->
        failwith "Province without id."
    | _, "" ->
        failwith "Province without name."
    | _ ->
        { Id = id; Name = name; Regions = [| city; townFirst; townSecond |] }
//

let internal getProvince (provincesData: Data.JsonProvince []) getResourceSeq provinceId =
    let province =
        query {
            for province in provincesData do
            where (province.Id = provinceId)
            select province
            head }
    
    let resourceIds =
        [ province.City.ResourceId; province.TownFirst.ResourceId; province.TownSecond.ResourceId ]
        |> List.choose id
        |> List.distinct

    let resources: seq<Resources.Resource> = resourceIds |> getResourceSeq

    let regionMap (region: Data.JsonRegion) regionType =
        let (id, name, regionType, isCoastal, resourceId, slotsCountOffset) =
            (region.Id, region.Name, regionType, region.IsCoastal, region.ResourceId, region.SlotsCountOffset)

        let resourceName =
            resourceId |> Option.map (fun x -> (resources |> Seq.find (fun y -> y.Id = x)).Name)
        let missingSlot =
            match slotsCountOffset with
                | Some 0 -> false
                | None -> false
                | _ -> true

        createRegion id name regionType (Option.defaultValue false isCoastal) resourceId resourceName missingSlot

    let city = regionMap province.City City
    let townFirst = regionMap province.TownFirst Town
    let townSecond = regionMap province.TownSecond Town

    createProvince province.Id province.Name city townFirst townSecond

let internal getDescriptors (provincesData: Data.JsonProvince []) (settings: Settings.Settings) =
    let slotTypes = [ Main; Coastal; General ]
    let regionTypes = [ City; Town ]

    let province =
        query {
            for province in provincesData do
                where (province.Id = settings.ProvinceId)
                head
        }

    let resourceIdsInProvince =
        [province.City.ResourceId; province.TownFirst.ResourceId; province.TownSecond.ResourceId]
        |> List.distinct

    slotTypes
    |> List.collect (fun slotType ->
        regionTypes
        |> List.collect (fun regionType ->
            resourceIdsInProvince
            |> List.map (fun resourceId ->
                { SlotType = slotType
                  RegionType = regionType
                  ResourceId = resourceId })))

let internal getProvinceEffect (provincesData: Data.JsonProvince []) (settings: Settings.Settings) =
    let jsonEffect = query {
            for jsonProvince in provincesData do
                where (jsonProvince.Id = settings.ProvinceId)
                select jsonProvince.Effect
                exactlyOne
        }

    jsonEffect |> createEffectSetFromJson

let internal getProvinceClimateId (provincesData: Data.JsonProvince []) (settings: Settings.Settings) =
    query {
            for jsonProvince in provincesData do
                where (jsonProvince.Id = settings.ProvinceId)
                select jsonProvince.ClimateId
                exactlyOne
        }

let internal getProvinceRegionIdSeq (provincesData: Data.JsonProvince []) (settings: Settings.Settings) =
    query {
            for jsonProvince in provincesData do
                where (jsonProvince.Id = settings.ProvinceId)
                select [jsonProvince.City.Id; jsonProvince.TownFirst.Id; jsonProvince.TownSecond.Id]
                exactlyOne
        } |> List.toSeq

let internal getProvincePairs (provincesData: Data.JsonProvince []) =
    query { for province in provincesData do select (province.Id, province.Name) }