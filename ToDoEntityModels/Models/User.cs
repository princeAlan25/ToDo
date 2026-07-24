using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ToDoEntityModels.Models;

public class User
{
    [Required]
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    [AllowNull]
    public string Name { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public ICollection<Category> Categories { get; set; } = [];
}
