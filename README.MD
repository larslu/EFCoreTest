# DT071G – Databashantering med Entity Framework Core & SQLite

Detta kodexempel används i kursen **DT071G (Webbutveckling med .NET)** för att demonstrera hur Entity Framework Core (Code First) hanterar databasoperationer och en 1-till-många-relation (`Course` -> `Module`) mot en lokal SQLite-databas.

---

## Förutsättningar

Innan du kör igång behöver du ha följande installerat på datorn:

1. **.NET SDK**
2. **dotnet-ef CLI-verktyget** installerat globalt.  
   Har du inte installerat det tidigare kör du detta kommando i terminalen en gång:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
   *(Har du redan verktyget installerat kan du uppdatera det med `dotnet tool update --global dotnet-ef`)*

---

## Kom igång: Steg för steg

Följ stegen i terminalen (i t.ex. Visual Studio Code eller PowerShell):

### 1. Klona repot och navigera in i mappen
```bash
git clone <URL-TILL-DITT-REPO>
cd <PROJEKT-MAPP>
```

### 2. Ladda ner projektets paket (`dotnet restore`)
Projektets beroenden (`Microsoft.EntityFrameworkCore.Sqlite` och `Microsoft.EntityFrameworkCore.Design`) är redan definierade i `.csproj`. Kör kommandot nedan för att hämta hem paketfilerna till datorn:
```bash
dotnet restore
```
> **Obs:** Detta steg är viktigt att göra innan du anropar `dotnet ef`, så att verktygets beroenden finns tillgängliga lokalt.

### 3. Skapa migreringen (`dotnet ef migrations add`)
Skapa C#-koden som beskriver hur databasschemat ska se ut utifrån dina modeller (`Course`, `Module`, `AppDbContext`):
```bash
dotnet ef migrations add InitialCreate
```
*En ny mapp med namnet `Migrations/` genereras nu i projektet.*

### 4. Skapa och uppdatera databasen (`dotnet ef database update`)
Verkställ migreringen mot SQLite för att skapa databasfilen (`.db`) och alla tabeller:
```bash
dotnet ef database update
```

### 5. Kör applikationen
```bash
dotnet run
```

---

## Så fungerar programmet

### Automatisk seedning
Vid start kontrollerar programmet om det finns några kurser lagrade (`!await db.Courses.AnyAsync()`). Om databasen är helt tom läggs kursen **DT071G** automatiskt in tillsammans med två fördefinierade moduler.

### Relationer och Foreign Keys: Viktigt att tänka på
I konsolmenyn kan du skapa nya kurser samt lägga till nya moduler:

* **I seedningen (nästlad lista):**  
  Modulerna skapas inuti kursens lista `Modules = [ new Module { ... } ]`. EF Core känner av relationen automatiskt och sätter rätt kurs-ID på modulerna när allt sparas.
* **I menyval 2 (fristående modul):**  
  När en modul skapas fristående och sparas direkt via `db.Modules.Add(module)`, **måste** du ange `CourseId = courseId`.  
  *Glömmer du att sätta `CourseId` får egenskapen standardvärdet `0` i C#, vilket gör att SQLite kastar felet `FOREIGN KEY constraint failed`.*

---

## Nollställa databasen

Om du vill rensa databasen och börja om från noll:
1. Radera den lokala SQLite-filen (t.ex. `app.db`).
2. Kör databasuppdateringen igen:
   ```bash
   dotnet ef database update
   ```