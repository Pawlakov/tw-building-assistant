namespace TWBuildingAssistant.Domain;

using System.Collections.Generic;

public record class BuildingLevel(int Id, string Name, Data.FSharp.Models.Effect Effect, IEnumerable<Data.FSharp.Models.Income> Incomes, IEnumerable<Data.FSharp.Models.Influence> Influences);