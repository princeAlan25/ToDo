using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoEntityModels.DataContexts;

public static class ToDoContextExtension
{
    public static IServiceCollection AddToDoContextService(
        this IServiceCollection services,
        string databaseFile = "ToDo.db"
        )
    {
        string dbFile = databaseFile;
        string dbPath = "Database";
        string fullDbPath = Path.Combine(AppContext.BaseDirectory,"..", "..", "..", "..", dbPath, dbFile);
        services.AddDbContext<ToDoContext>(options =>
        {
            options.UseSqlite($"Data Source={fullDbPath}");

            options.LogTo(
                ContextLogger.LoggContext,
                [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]
                );
        }, contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);
        return services;
    }
}
