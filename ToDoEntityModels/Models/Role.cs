using System.ComponentModel.DataAnnotations;

namespace ToDoEntityModels.Models;

public class Role
{
    [Required]
    [Key]
    public int? RoleId { get; set; }
    [Required]
    [StringLength(30)]
    public string Name { get; set; } = "guest";
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public DateTime UpdatedAt { get; set;} = DateTime.Now;
    public ICollection<User>? Users { get; set; } = [];
}
