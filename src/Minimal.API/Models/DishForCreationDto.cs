using System.ComponentModel.DataAnnotations;

namespace Minimal.API.Models;

/// <summary>DTO for creating a dish</summary>
public class DishForCreationDto
{
    /// <summary>The name of the dish.</summary>
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }
}