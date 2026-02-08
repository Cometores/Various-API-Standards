using CRUD.API.Entities;

namespace CRUD.API.Services;

/// <summary>
/// Defines the contract for a repository that manages city and point of interest data.
/// </summary>
public interface ICityInfoRepository
{
    /// <summary>Retrieves all cities.</summary>
    /// <returns>A collection of cities.</returns>
    Task<IEnumerable<City>> GetCitiesAsync();

    /// <summary> Retrieves a collection of cities with optional filtering and pagination.</summary>
    /// <param name="name">The name to filter cities by.</param>
    /// <param name="searchQuery">A search query to match against city names or descriptions.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the collection of cities and pagination metadata.</returns>
    Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

    /// <summary>Retrieves a city by its identifier.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="includePointsOfInterest">Whether to include points of interest in the result.</param>
    /// <returns>The city if found; otherwise, null.</returns>
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    /// <summary>Checks if a city exists by its identifier.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <returns>True if the city exists; otherwise, false.</returns>
    Task<bool> CityExistsAsync(int cityId);

    /// <summary>Retrieves all points of interest for a given city.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <returns>A collection of points of interest.</returns>
    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

    /// <summary>Retrieves a specific point of interest for a given city.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterestId">The identifier of the point of interest.</param>
    /// <returns>The point of interest if found; otherwise, null.</returns>
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

    /// <summary> Adds a point of interest to a city.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterest">The point of interest to add.</param>
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

    /// <summary>Saves all changes made in the repository to the underlying data store.</summary>
    /// <returns>True if changes were saved successfully; otherwise, false.</returns>
    Task<bool> SaveChangesAsync();

    /// <summary>Deletes a point of interest.</summary>
    /// <param name="pointOfInterest">The point of interest to delete.</param>
    void DeletePointOfInterest(PointOfInterest pointOfInterest);

    /// <summary>Checks if the provided city name matches the city with the given identifier.</summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="cityId">The identifier of the city.</param>
    /// <returns>True if they match; otherwise, false.</returns>
    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
}