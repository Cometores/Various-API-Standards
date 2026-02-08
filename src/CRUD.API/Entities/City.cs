using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.API.Entities;

/// <summary>Represents a city entity.</summary>
public class City
{
    /// <summary>Unique identifier for the city.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>Name of the city.</summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    /// <summary>Description of the city.</summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>Collection of points of interest associated with the city.</summary>
    public ICollection<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();

    /// <summary>Initializes a new instance of the <see cref="City"/> class.</summary>
    /// <param name="name">The name of the city.</param>
    public City(string name)
    {
        Name = name;
    }
}