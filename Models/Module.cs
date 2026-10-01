namespace EfCoreTest.Models;

public class Module
{
    public int ModuleId { get; set; }
    public int CourseId { get; set; }

    public required string Title { get; set; }

    public int OrderIndex { get; set; }

    public bool IsPublished { get; set; }

    public Course Course {get; set;} = null!;

}