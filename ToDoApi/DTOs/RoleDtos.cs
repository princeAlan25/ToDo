namespace ToDoApi.DTOs;

public record CreateRoleDto()
{
    public required string Name { get; init; }
};

public record RoleDto()
{
    public int? RoleId { get; init; }
    public required string Name { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public int ReferenceUsersCount { get; init; } = 0;
}

public record UpdateRoleDto()
{
    public required string Name { get; init; }
}