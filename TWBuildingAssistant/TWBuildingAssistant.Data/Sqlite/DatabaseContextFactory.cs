﻿namespace TWBuildingAssistant.Data.Sqlite;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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