using System.ComponentModel.DataAnnotations;

namespace ToDoEntityModels.Models;

public class TaskItem
{
    [Required]
    [Key]
    public int TaskId { get; set; }
    [Required]
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public TaskRate Starred { get; set; }
    public TaskPriority Priority { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = new();
}

public enum TaskPriority { Low, Medium, High }
public enum TaskRate { Low, Medium, High, VeryHigh, ExtraHigh }