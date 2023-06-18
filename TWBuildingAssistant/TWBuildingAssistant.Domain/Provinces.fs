module TWBuildingAssistant.Domain.Provinces

open Data

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
      ResourceId:int option }

type internal Region =
    { Id:int
      Name:string
      RegionType:RegionType
      ResourceId:int option
      ResourceName: string option
      Slots:SlotDescriptor[] }

type internal Province =
    { Id:int 
      Name:string
      Regions:Region[] }

type internal RegionData =
    | CityData of ProvincesData.City
    | TownFirstData of ProvincesData.City
    | TownSecondData of ProvincesData.City

let internal createRegion id name regionType isCoastal resourceId resourceName missingSlot =
    match (id, name) with
    | 0, _ -> failwith "Region without id."
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
    | 0, _ ->
        failwith "Province without id."
    | _, "" ->
        failwith "Province without name."
    | _ ->
        { Id = id; Name = name; Regions = [| city; townFirst; townSecond |] }

let internal getProvince (provincesData: ProvincesData.Root[]) (resourcesData: ResourcesData.Root[]) provinceId =
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

    let resources =
        query {
            for resource in resourcesData do
            where (List.contains resource.Id resourceIds)
            select resource }
        |> Seq.toList

    let regionMap region =
        let (id, name, regionType, isCoastal, resourceId, slotsCountOffset) =
            match region with
            | CityData region ->
                (region.Id, region.Name, City, region.IsCoastal, region.ResourceId, region.SlotsCountOffset)
            | TownFirstData region ->
                (region.Id, region.Name, Town, region.IsCoastal, region.ResourceId, region.SlotsCountOffset)
            | TownSecondData region ->
                (region.Id, region.Name, Town, region.IsCoastal, region.ResourceId, region.SlotsCountOffset)

        let resourceName =
            resourceId |> Option.map (fun x -> (resources |> List.find (fun y -> y.Id = x)).Name)
        let missingSlot =
            match slotsCountOffset with
                | Some 0 -> false
                | None -> false
                | _ -> true

        createRegion id name regionType (Option.defaultValue false isCoastal) resourceId resourceName missingSlot

    let city = regionMap (CityData province.City)
    let townFirst = regionMap (TownFirstData province.TownFirst)
    let townSecond = regionMap (TownSecondData province.TownSecond)

    createProvince province.Id province.Name city townFirst townSecond

let internal getDescriptors (provincesData: ProvincesData.Root[]) (settings: Settings.Settings) =
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