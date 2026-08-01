using System.ComponentModel.DataAnnotations;
using ToDoEntityModels.Models;

namespace ToDoApi.DTOs;


public record UserDto(
    Guid UserId,
    string Email,
    string Name,
    Role? Role,
    List<Category>? Categories,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );

public record UpdateUserDto(
    Guid UserId,
    string Email,
    string Name,
    int RoleId,
    DateTime UpdatedAt
    );

public record CreateUserDto(string Email, string Name, string Password);