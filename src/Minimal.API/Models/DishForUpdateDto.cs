namespace Minimal.API.Models;

/// <summary>DTO for updating a dish.</summary>
public class DishForUpdateDto
{
    /// <summary>The name of the dish.</summary>
    public required string Name { get; set; }
}