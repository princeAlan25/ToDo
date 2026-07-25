using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoEntityModels.Models;

namespace ToDoEntityModels.DataContexts;

public class ToDoContext(): DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbFile = "todo.db";
        string dbPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Database"));
        string fullDbPath = Path.GetFullPath(Path.Combine(dbPath, dbFile));
        if (!optionsBuilder.IsConfigured)
        {
            if (!Directory.Exists(fullDbPath))
            {
                Console.WriteLine($"Creating a Missing database file {dbFile}");
                Directory.CreateDirectory(dbPath);
                File.Create(fullDbPath);
            }
        }
        optionsBuilder.UseSqlite($"Data Source={fullDbPath}");
        optionsBuilder.LogTo(
            ContextLogger.LoggContext,
            [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(e => e.Role).WithMany(e => e.Users).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasOne(e => e.User).WithMany(e => e.Categories).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasOne(e => e.Category).WithMany(e => e.Tasks).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
