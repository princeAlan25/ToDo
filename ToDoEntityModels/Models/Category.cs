using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ToDoEntityModels.Models;

public class Category
{
    [Key]
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public string ColorCode { get; set; } = "";
    public string? Description { get; set; } = "";
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Guid UserId { get; set; }
    public User User { get; set; } = new();
    public ICollection<TaskItem> Tasks { get; set; } = [];
}
