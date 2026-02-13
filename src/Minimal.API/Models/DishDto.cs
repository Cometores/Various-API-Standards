namespace Minimal.API.Models;

/// <summary>DTO for a dish.</summary>
public class DishDto
{
    /// <summary>The unique identifier for the dish.</summary>
    public Guid Id { get; set; }
    
    /// <summary> The name of the dish.</summary>
    public required string Name { get; set; }

}