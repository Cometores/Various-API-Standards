using CRUD.API.Models;

namespace CRUD.API.Data;

/// <summary>Provides an in-memory data store for city data (used for early API prototyping).</summary>
public class CitiesInMemoryDataStore
{
    /// <summary>
    /// Gets or sets the collection of cities managed by the <see cref="CitiesInMemoryDataStore"/> class.
    /// This property provides access to a list of city data including details such as name,
    /// description, and associated points of interest.
    /// </summary>
    public List<CityDto> Cities { get; private set; }

    /// <summary>
    /// Gets the singleton instance of the <see cref="CitiesInMemoryDataStore"/> class,
    /// which provides an in-memory data store for managing city data.
    /// </summary>
    public static CitiesInMemoryDataStore Current { get; } = new();

    /// <summary>Initializing dummy data.</summary>
    public CitiesInMemoryDataStore()
    {
        Cities = new List<CityDto>
        {
            new()
            {
                Id = 1,
                Name = "Leipzig",
                Description = "The one with music heritage and a lively student vibe.",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new()
                    {
                        Id = 1,
                        Name = "St. Thomas Church (Thomaskirche)",
                        Description =
                            "Famous for the St. Thomas Choir and closely associated with Johann Sebastian Bach."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Monument to the Battle of the Nations (Völkerschlachtdenkmal)",
                        Description =
                            "A massive memorial commemorating the 1813 Battle of Leipzig, with panoramic views from the top."
                    }
                }
            },
            new()
            {
                Id = 2,
                Name = "Düsseldorf",
                Description = "The one with the Rhine promenade and modern architecture.",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new()
                    {
                        Id = 3,
                        Name = "Rhine Promenade (Rheinuferpromenade)",
                        Description =
                            "A popular riverside walkway connecting the Old Town with modern parts of the city."
                    },
                    new()
                    {
                        Id = 4,
                        Name = "MediaHarbor (MedienHafen)",
                        Description =
                            "A redeveloped harbor district known for contemporary buildings, including designs by Frank Gehry."
                    }
                }
            },
            new()
            {
                Id = 3,
                Name = "Köln",
                Description = "The one with the cathedral and a strong carnival spirit.",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new()
                    {
                        Id = 5,
                        Name = "Cologne Cathedral (Kölner Dom)",
                        Description =
                            "A landmark Gothic cathedral and UNESCO World Heritage Site, one of Germany’s most visited attractions."
                    },
                    new()
                    {
                        Id = 6,
                        Name = "Hohenzollern Bridge (Hohenzollernbrücke)",
                        Description =
                            "A Rhine crossing famous for its love locks and great views of the cathedral skyline."
                    }
                }
            }
        };
    }
}