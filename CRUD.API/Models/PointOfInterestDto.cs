using System.ComponentModel.DataAnnotations;

namespace CRUD.API.Models;

public class PointOfInterestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}