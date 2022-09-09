namespace TWBuildingAssistant.Data.Sqlite;

using System;
using Microsoft.EntityFrameworkCore;

public class DatabaseContextFactory
{
    private readonly Action<DbContextOptionsBuilder> configureDbContext;

    public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        this.configureDbContext = configureDbContext;
    }

    public DatabaseContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>();

        this.configureDbContext(options);

        return new DatabaseContext(options.Options);
    }
}