using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ToDoEntityModels;

public class User
{
    [Required]
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    [AllowNull]
    public string UserName { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
}
