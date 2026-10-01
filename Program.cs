using Microsoft.EntityFrameworkCore;
using EfCoreTest.Models;
using EfCoreTest.Data;

using var db = new AppDbContext();

if (!await db.Courses.AnyAsync()) //Kontrollerar om det finns något i databasen
{
    var defaultCourse = new Course
    {
        Code = "DT071G",
        Title = "Webbutveckling med .NET",
        Modules =
        [
            new Module { Title = "C# och OOP", OrderIndex = 1, IsPublished = true },
            new Module { Title = "Databashantering med EF Core", OrderIndex = 2, IsPublished = false }
        ]
    };

    db.Courses.Add(defaultCourse); //insert into table i minnet
    await db.SaveChangesAsync();  //Här sker själva kommandot
}

while (true)
{
    Console.Clear();
    Console.WriteLine("=== KURSER I DATABASEN ===");

    // Hämta alla kurser med tillhörande moduler via en enda enkel fråga
    var courses = await db.Courses
        .Include(c => c.Modules)
        .ToListAsync();

    if (courses.Count == 0)
    {
        Console.WriteLine("(Databasen är tom)");
    }
    else
    {
        foreach (var c in courses)
        {
            Console.WriteLine($"\n[Id: {c.CourseId}] {c.Code} - {c.Title}");
            foreach (var m in c.Modules)
            {
                Console.WriteLine($"   └── Modul {m.OrderIndex}: {m.Title}");
            }
        }
    }

    Console.WriteLine("\n----------------------------");
    Console.WriteLine("1. Lägg till kurs");
    Console.WriteLine("2. Lägg till modul till kurs");
    Console.WriteLine("3. Avsluta");
    Console.Write("\nVälj (1-3): ");

    var val = Console.ReadLine();

    if (val == "1")
    {
        Console.Write("Kurskod (t.ex. DT071G): ");
        var code = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Kurstitel: ");
        var title = Console.ReadLine()?.Trim() ?? string.Empty;

        // Skapa C#-objektet och spara – EF Core sköter SQL INSERT
        var course = new Course { Code = code, Title = title }; //Objektinitierare 
        db.Courses.Add(course);
        await db.SaveChangesAsync();

        Console.WriteLine($"\nSparad! Kursen fick automatiskt Id {course.CourseId}.");
        Console.WriteLine("Tryck valfri tangent...");
        Console.ReadKey();
    }
    else if (val == "2")
    {
        Console.Write("Ange Kurs-Id som modulen ska tillhöra: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ogiltigt Id.");
            Console.ReadKey();
            continue;
        }

        Console.Write("Modultitel: ");
        var moduleTitle = Console.ReadLine() ?? string.Empty;

        Console.Write("Ordningsnummer (1, 2, 3...): ");
        _ = int.TryParse(Console.ReadLine(), out int orderIndex);

        // Skapa modulen och koppla med foreign key
        var module = new Module
        {
            CourseId = courseId,
            Title = moduleTitle,
            OrderIndex = orderIndex,
            IsPublished = true
        };

        db.Modules.Add(module);
        await db.SaveChangesAsync();

        Console.WriteLine("\nModulen sparad!");
        Console.WriteLine("Tryck valfri tangent...");
        Console.ReadKey();
    }
    else if (val == "3")
    {
        break;
    }
}


