using EfCoreTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreTest.Data;

public class AppDbContext : DbContext
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Module> Modules => Set<Module>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=kurser.db")
        .LogTo(
                action: message =>
                {
                    // Filtrera bort EF:s interna migreringstabeller så bara verklig SQL syns
                    if (message.Contains("__EFMigrations") || message.Contains("PRAGMA"))
                        return;

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("\n┌── [SQL TILL DATABASEN] ───────────────────────────");
                    Console.WriteLine(message.TrimEnd());
                    Console.WriteLine("└───────────────────────────────────────────────────");
                    Console.ResetColor();
                },
                categories: [DbLoggerCategory.Database.Command.Name],
                minimumLevel: LogLevel.Information
            )
            .EnableSensitiveDataLogging();
    }
}