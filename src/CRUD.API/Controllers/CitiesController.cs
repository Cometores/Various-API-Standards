using System.Text.Json;
using AutoMapper;
using CRUD.API.Models;
using CRUD.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.API.Controllers;

/// <summary>
/// The CitiesController provides endpoints for managing and retrieving city information.
/// It supports operations such as listing cities with filtering and pagination, 
/// and retrieving detailed information for a specific city.
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/cities")]
public class CitiesController : ControllerBase
{
    private const int MAX_CITIES_PAGE_SIZE = 20;
    
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    /// <summary>The <see cref="CitiesController"/> constructor.</summary>
    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    /// <summary>Retrieves all cities.</summary>
    /// <param name="name">Optional name to filter cities by.</param>
    /// <param name="searchQuery">Optional search query to match against city names or descriptions.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A collection of cities without their points of interest.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery] string? name,
        [FromQuery] string? searchQuery,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > MAX_CITIES_PAGE_SIZE)
        {
            pageSize = MAX_CITIES_PAGE_SIZE;
        }

        var (cityEntities, paginationMetadata) = await _cityInfoRepository
            .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }
    
    /// <summary>Retrieves a city by id.</summary>
    /// <param name="id">The id of the city to get</param>
    /// <param name="includePointsOfInterest">Whether to include the points of interest</param>
    /// <returns>An IActionResult</returns>
    /// <response code="200">Returns the requested city</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCity(
        [FromRoute] int id,
        [FromQuery] bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));
        }

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}