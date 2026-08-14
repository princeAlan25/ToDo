using System.ComponentModel.DataAnnotations;
using ToDoEntityModels.Models;

namespace ToDoShared.DTOs;

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

public record SignUpRequestDto(string Email, string Name, string Password);

public record LoginRequestDto(string Email, string Password);
public record loginResponseDto(string AccessToken, string TokenType);