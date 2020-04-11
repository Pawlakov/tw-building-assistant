namespace TWBuildingAssistant.Data.Tool
{
    using System;
    using TWBuildingAssistant.Data;

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("You are about to run the JSON to SQLite utility.");
            Console.Write("Press any key to continue.");
            Console.ReadKey();

            var tool = new JsonToSqliteMigrator();
            tool.Run();

            Console.WriteLine("Done.");
            Console.Write("Press any key to quit.");
            Console.ReadKey();
        }
    }
}