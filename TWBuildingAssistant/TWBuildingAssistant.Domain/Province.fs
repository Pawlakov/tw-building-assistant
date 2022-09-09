module TWBuildingAssistant.Domain.Province

open TWBuildingAssistant.Data.Sqlite

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

let internal getRegionType intValue =
    match intValue with
    | 0 -> City
    | 1 -> Town
    | _ -> failwith "Invalid value"

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

let internal createProvince id name regions =
    match (id, name, regions) with
    | 0, _, _ ->
        failwith "Province without id."
    | _, "", _ ->
        failwith "Province without name."
    | _, _, regions when regions |> Array.length <> 3 ->
        failwith "Invalid region count."
    | _ ->
        { Id = id; Name = name; Regions = regions }

let internal getProvince (ctx:DatabaseContext) provinceId =
    let regions =
        query {
            for region in ctx.Regions do
            where (region.ProvinceId = provinceId)
            select region }
        |> Seq.toList

    let resourceIds =
        regions 
        |> List.choose (fun x -> (if x.ResourceId.HasValue then Some x.ResourceId.Value else None)) 
        |> List.distinct

    let resources =
        query {
            for resource in ctx.Resources do
            where (List.contains resource.Id resourceIds)
            select resource }
        |> Seq.toList

    let province =
        query {
            for province in ctx.Provinces do
            where (province.Id = provinceId)
            select province
            head }

    let regionMap (region:Entities.Region) =
        let regionType =
            region.RegionType |> getRegionType
        let resourceId = 
            if region.ResourceId.HasValue then Some region.ResourceId.Value else None
        let resourceName =
            resourceId |> Option.map (fun x -> (resources |> List.find (fun y -> y.Id = x)).Name)
        let missingSlot =
            region.SlotsCountOffset <> 0

        createRegion region.Id region.Name regionType region.IsCoastal resourceId resourceName missingSlot

    let regions =
        regions
        |> List.map regionMap
        |> List.toArray

    createProvince province.Id province.Name regions