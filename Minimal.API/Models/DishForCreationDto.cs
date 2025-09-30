using System.ComponentModel.DataAnnotations;

namespace Minimal.API.Models;

public class DishForCreationDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }
}