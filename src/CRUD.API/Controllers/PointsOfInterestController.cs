using AutoMapper;
using CRUD.API.Models;
using CRUD.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.API.Controllers;

/// <summary>
/// The PointsOfInterestController manages points of interest within a specific city.
/// It provides endpoints for creating, retrieving, updating, and deleting points of interest.
/// </summary>
[ApiController]
[Authorize(Policy = "MustBeFromAntwerp")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/cities/{cityId:int}/pointsofinterest")]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    /// <summary><see cref="PointsOfInterestController"/> constructor.</summary>
    public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
        IMailService mailService,
        ICityInfoRepository cityInfoRepository,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>Retrieves all points of interest for a given city.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <returns>A collection of points of interest for the specified city.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        /* User can only get Points of interest from the city he lives in*/
        // var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
        // if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
        // {
        //     return Forbid();
        // }

        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation("City with id {CityId} wasn't found when accessing points of interest.", cityId);
            return NotFound();
        }

        var pointsOfInterestForCity = await _cityInfoRepository
            .GetPointsOfInterestForCityAsync(cityId);
        
        return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
    }

    /// <summary>Retrieves a specific point of interest in a given city.</summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterestId">The identifier of the point of interest.</param>
    /// <returns>The requested point of interest.</returns>
    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        var pointOfInterest = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    /// <summary>Creates a new point of interest in a given city.</summary>
    /// <param name="cityId">The identifier of the city where the point of interest will be created.</param>
    /// <param name="pointOfInterest">The data for the new point of interest.</param>
    /// <returns>The newly created point of interest.</returns>
    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
        [FromRoute] int cityId,
        [FromBody] PointOfInterestForCreationDto pointOfInterest)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        // map PointOfInterestForCreationDto to PointOfInterestEntity
        var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

        await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

        await _cityInfoRepository.SaveChangesAsync();

        var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

        return CreatedAtRoute(
            "GetPointOfInterest",
            new
            {
                cityId,
                pointOfInterestId = createdPointOfInterestToReturn.Id
            },
            createdPointOfInterestToReturn
        );
    }

    /// <summary>
    /// Updates a specific point of interest in a given city.
    /// </summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterestId">The identifier of the point of interest to update.</param>
    /// <param name="pointOfInterest">The updated data for the point of interest.</param>
    /// <returns>An IActionResult representing the result of the update.</returns>
    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(
        [FromRoute] int cityId,
        [FromRoute] int pointOfInterestId,
        [FromBody] PointOfInterestForUpdateDto pointOfInterest)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _mapper.Map(pointOfInterest, pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Partially updates a specific point of interest in a given city using JSON Patch.
    /// </summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterestId">The identifier of the point of interest to update.</param>
    /// <param name="patchDocument">The JSON Patch document containing the changes.</param>
    /// <returns>An IActionResult representing the result of the update.</returns>
    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(
        [FromRoute] int cityId,
        [FromRoute] int pointOfInterestId,
        [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
    )
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // check if model is valid after patch
        if (!TryValidateModel(pointOfInterestToPatch))
            return BadRequest(ModelState);

        _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific point of interest in a given city.
    /// </summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterestId">The identifier of the point of interest to delete.</param>
    /// <returns>An IActionResult representing the result of the deletion.</returns>
    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(
        [FromRoute] int cityId,
        [FromRoute] int pointOfInterestId
    )
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();

        // Custom service - Mail logging
        _mailService.Send("Point of interest deleted.",
            $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} " +
            $"was deleted");

        return NoContent();
    }
}