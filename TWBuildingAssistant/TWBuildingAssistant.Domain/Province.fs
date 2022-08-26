module TWBuildingAssistant.Domain.Province

open FSharp.Data.Sql
open Database

type RegionType =
    | City
    | Town

type SlotType =
    | Main
    | Coastal
    | General

type SlotDescriptor =
    { SlotType:SlotType
      RegionType:RegionType
      ResourceId:int option }

type Region =
    { Id:int
      Name:string
      RegionType:RegionType
      ResourceId:int option
      ResourceName: string option
      Slots:SlotDescriptor[] }

type Province =
    { Id:int 
      Name:string
      Regions:Region[] }

let getRegionType intValue =
    match intValue with
    | 0 -> City
    | 1 -> Town
    | _ -> failwith "Invalid value"

let createRegion id name regionType isCoastal resourceId resourceName missingSlot =
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

let createProvince id name regions =
    match (id, name, regions) with
    | 0, _, _ ->
        failwith "Province without id."
    | _, "", _ ->
        failwith "Province without name."
    | _, _, regions when regions |> Array.length <> 3 ->
        failwith "Invalid region count."
    | _ ->
        { Id = id; Name = name; Regions = regions }

let getProvince provinceId =
    let ctx =
        sql.GetDataContext SelectOperations.DatabaseSide

    let regions =
        query {
            for region in ctx.Dbo.Regions do
            where (region.ProvinceId = provinceId)
            select region }
        |> Seq.toList

    let resourceIds =
        regions 
        |> List.choose (fun x -> x.ResourceId) 
        |> List.distinct

    let resources =
        query {
            for resource in ctx.Dbo.Resources do
            where (resource.Id |=| resourceIds)
            select resource }
        |> Seq.toList

    let province =
        query {
            for province in ctx.Dbo.Provinces do
            where (province.Id = provinceId)
            select province
            head }

    let regions =
        regions
        |> List.map (fun region -> createRegion region.Id region.Name (region.RegionType |> getRegionType) region.IsCoastal region.ResourceId (region.ResourceId |> Option.map (fun x -> (resources |> List.find (fun y -> y.Id = x)).Name)) (region.SlotsCountOffset <> 0))
        |> List.toArray

    createProvince province.Id province.Name regions