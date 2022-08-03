namespace TWBuildingAssistant.Data.Tool;

using System;
using System.Linq;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Data.Sqlite.Entities;

public class Program
{
    public static void Main()
    {
        var context = new DatabaseContext();

        DisplayTechs(context);

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Press any key to quit");
        Console.ReadKey();
    }

    private static void DisplayTechs(DatabaseContext context)
    {
        var techs = context.TechnologyLevels.OrderBy(x => x.Order).ToList();
        foreach (var tech in techs)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("{0}", tech.Order);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" (Id: {0})", tech.Id);

            Console.WriteLine();

            // var root = context.BuildingLevels.Find(branch.RootBuildingLevelId);
            // DisplayLevel(context, root, 1);
        }
    }

    private static void DisplayBranches(DatabaseContext context)
    {
        var branches = context.BuildingBranches.OrderBy(x => x.Name).ToList();
        foreach (var branch in branches)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("{0}", branch.Name);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" (Id: {0})", branch.Id);

            Console.WriteLine();

            var root = context.BuildingLevels.Find(branch.RootBuildingLevelId);
            DisplayLevel(context, root, 1);
        }
    }

    private static void DisplayLevel(DatabaseContext context, BuildingLevel level, int depth)
    {
        Console.Write(new string(' ', depth * 2));

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("{0}", level.Name);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" (Id: {0})", level.Id);

        Console.WriteLine();

        var children = context.BuildingLevels.Where(x => x.ParentBuildingLevelId == level.Id).ToList();
        foreach (var child in children)
        {
            DisplayLevel(context, child, depth + 1);
        }
    }
}