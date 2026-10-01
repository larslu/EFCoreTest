
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EfCoreTest.Models;

public class Course
{
    public int CourseId { get; set; }
    [MaxLength(10)]
    public required string Code { get; set; }
    [MaxLength(40)]
    public required string Title { get; set; }

    public List<Module> Modules { get; set; } = [];

}