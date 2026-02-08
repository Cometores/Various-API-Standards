using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.API.Entities;

/// <summary>Represents a point of interest entity.</summary>
public class PointOfInterest
{
    /// <summary>Unique identifier for the point of interest.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>Name of the point of interest.</summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    /// <summary>Description of the point of interest.</summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>City associated with the point of interest.</summary>
    [ForeignKey("CityId")]
    public City? City { get; set; }

    /// <summary>Identifier of the city associated with the point of interest.</summary>
    public int CityId { get; set; }

    /// <summary> Initializes a new instance of the <see cref="PointOfInterest"/> class.</summary>
    /// <param name="name">The name of the point of interest.</param>
    public PointOfInterest(string name)
    {
        Name = name;
    }
}