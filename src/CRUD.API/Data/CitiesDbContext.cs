using CRUD.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD.API.Data;

/// <summary>
/// Represents the database context for the Cities application.
/// Provides access to the Cities and Points of Interest data models using Entity Framework Core.
/// </summary>
/// <remarks>SQLite used in the project.</remarks>
public class CitiesDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the collection of cities managed by the <see cref="CitiesDbContext"/> class.
    /// </summary>
    public DbSet<City> Cities { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of points of interest managed by the <see cref="CitiesDbContext"/> class.
    /// </summary>
    public DbSet<PointOfInterest?> PointsOfInterest { get; set; } = null!;

    /// <summary>Represents the Entity Framework database context for managing the Cities application data.</summary>
    /// <remarks>
    /// This context is responsible for managing the interaction with the database, including
    /// the `City` and `PointOfInterest` entities. It uses SQLite as the underlying database provider
    /// and contains seeding data for initialization.
    /// </remarks>
    public CitiesDbContext(DbContextOptions<CitiesDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the model relationships, constraints, and initial seeding data for the database context.
    /// </summary>
    /// <param name="modelBuilder">An instance of <c>ModelBuilder</c> used to define the database schema, relationships, and seed data for the entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(
                new City("Leipzig")
                {
                    Id = 1,
                    Description = "The one with music heritage and a lively student vibe."
                },
                new City("Düsseldorf")
                {
                    Id = 2,
                    Description = "The one with the Rhine promenade and modern architecture."
                },
                new City("Köln")
                {
                    Id = 3,
                    Description = "The one with the cathedral and a strong carnival spirit."
                });

        modelBuilder.Entity<PointOfInterest>()
            .HasData(
                new PointOfInterest("St. Thomas Church (Thomaskirche)")
                {
                    Id = 1,
                    CityId = 1,
                    Description =
                        "Famous for the St. Thomas Choir and closely associated with Johann Sebastian Bach."
                },
                new PointOfInterest("Monument to the Battle of the Nations (Völkerschlachtdenkmal)")
                {
                    Id = 2,
                    CityId = 1,
                    Description =
                        "A massive memorial commemorating the 1813 Battle of Leipzig, with panoramic views from the top."
                },
                new PointOfInterest("Rhine Promenade (Rheinuferpromenade)")
                {
                    Id = 3,
                    CityId = 2,
                    Description =
                        "A popular riverside walkway connecting the Old Town with modern parts of the city."
                },
                new PointOfInterest("MediaHarbor (MedienHafen)")
                {
                    Id = 4,
                    CityId = 2,
                    Description =
                        "A redeveloped harbor district known for contemporary buildings, including designs by Frank Gehry."
                },
                new PointOfInterest("Cologne Cathedral (Kölner Dom)")
                {
                    Id = 5,
                    CityId = 3,
                    Description =
                        "A landmark Gothic cathedral and UNESCO World Heritage Site, one of Germany’s most visited attractions."
                },
                new PointOfInterest("Hohenzollern Bridge (Hohenzollernbrücke)")
                {
                    Id = 6,
                    CityId = 3,
                    Description =
                        "A Rhine crossing famous for its love locks and great views of the cathedral skyline."
                });
        base.OnModelCreating(modelBuilder);
    }

#if false
    /// <summary>
    /// Alternative approach: configure provider inside DbContext.
    /// Not used in this project because configuration is done via DI.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=CityInfo.db");
    }
#endif
}