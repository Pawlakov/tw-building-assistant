module TWBuildingAssistant.Data.FSharp.Models

open System.Data
open Donald

type NamedId = 
    { Id:int
      Name:string }

module NamedId =
  let ofDataReader (rd : IDataReader) : NamedId =      
      { Id = rd.ReadInt32 "Id"
        Name = rd.ReadString "Name" }