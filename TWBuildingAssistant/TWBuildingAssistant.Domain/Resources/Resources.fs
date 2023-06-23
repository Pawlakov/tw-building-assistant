module TWBuildingAssistant.Domain.Resources

module Data =
    open FSharp.Data

    open System.IO
    open System.Reflection

    type private JsonData = JsonProvider<Sample="Resources\Resources-sample.json", SampleIsList=false>

    type internal JsonResource = JsonData.Resourcis

    let internal getResourcesData () =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = assembly.GetManifestResourceStream ("TWBuildingAssistant.Domain.Resources.Resources.json")
        use reader = new StreamReader (stream)
        (() |> reader.ReadToEnd |> JsonData.Parse).Resources

type internal Resource =
    { Id: string
      Name: string }

// Constructors
let private createResource id name =
    match (id, name) with
    | "", _ ->
        failwith "Resource without id."
    | _, "" ->
        failwith "Resource without name."
    | _ ->
        { Id = id; Name = name }

let private createResourceFromJson (jsonResource: Data.JsonResource) =
    let id = jsonResource.Id
    let name = jsonResource.Name

    createResource id name
//

let internal getResourcesByIds (resourcesData: Data.JsonResource []) ids =
    resourcesData 
    |> Seq.filter (fun resource -> ids |> Seq.contains resource.Id)
    |> Seq.map (fun resource -> resource |> createResourceFromJson)