using Microsoft.EntityFrameworkCore;
using ToDoEntityModels.Models;

namespace ToDoEntityModels.DataContexts;

public class ToDoContext: DbContext
{
    public ToDoContext() { }
    public ToDoContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users {  get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use an absolute path inside the app's base directory so EF Core can always find the DB file
        string dbFile = "ToDo.db";
        string dbPath = "Database";
        string fullDbPath = Path.Combine(AppContext.BaseDirectory, "..","..", "..", "..", dbPath, dbFile);
        if (!optionsBuilder.IsConfigured)
        {
            string fullDbFolder = Path.GetDirectoryName(fullDbPath) ?? Path.Combine(AppContext.BaseDirectory, dbPath);
            if (!Directory.Exists(fullDbFolder))
            {
                Console.WriteLine($"Creating missing database folder: {fullDbFolder}");
                Directory.CreateDirectory(fullDbFolder);
            }

            if (!File.Exists(fullDbPath))
            {
                Console.WriteLine($"Creating missing database file: {fullDbPath}");
                // Create and dispose the file stream immediately
                using (File.Create(fullDbPath)) { }
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

        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, Name = "Business", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new Role { RoleId = 2, Name = "Sport", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new Role { RoleId = 3, Name = "Learning", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new Role { RoleId = 4, Name = "Teaching", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new Role { RoleId = 5, Name = "Daily", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new Role { RoleId = 6, Name = "Travel", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() },
            new Role { RoleId = 7, Name = "Work", Users = [], CreatedAt = new DateTime(), UpdatedAt = new DateTime() }
            );
    }
}
