﻿using System.Text.Json;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly ICourseLibraryRepository _courseLibraryRepository;
    private readonly IMapper _mapper;
    private readonly IPropertyMappingService _propertyMappingService;

    public AuthorsController(
        ICourseLibraryRepository courseLibraryRepository,
        IMapper mapper, IPropertyMappingService propertyMappingService)
    {
        _courseLibraryRepository = courseLibraryRepository ??
                                   throw new ArgumentNullException(nameof(courseLibraryRepository));
        _mapper = mapper ??
                  throw new ArgumentNullException(nameof(mapper));
        _propertyMappingService = propertyMappingService ?? 
                                  throw new ArgumentNullException(nameof(propertyMappingService));
    }

    [HttpGet(Name = "GetAuthors")]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(
        [FromQuery] AuthorsResourceParameters authorsResourceParameters)
    {
        // throw new Exception("test exception");

        if (!_propertyMappingService.ValidMappingExistsFor<AuthorDto, Entities.Author>(
                authorsResourceParameters.OrderBy))
        {
            return BadRequest();
        }
        
        var authorsFromRepo = await _courseLibraryRepository
            .GetAuthorsAsync(authorsResourceParameters);

        var previousPageLink = authorsFromRepo.HasPrevious
            ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage)
            : null;

        var nextPageLink = authorsFromRepo.HasNext
            ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage)
            : null;

        var paginationMetadata = new
        {
            totalCount = authorsFromRepo.TotalCount,
            pageSize = authorsFromRepo.PageSize,
            currentPage = authorsFromRepo.CurrentPage,
            totalPages = authorsFromRepo.TotalPages,
            previousPageLink,
            nextPageLink
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
    }

    private string? CreateAuthorsResourceUri(
        AuthorsResourceParameters authorsResourceParameters,
        ResourceUriType type)
    {
        switch (type)
        {
            case ResourceUriType.PreviousPage:
                return Url.Link("GetAuthors",
                    new
                    {
                        orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber - 1,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
            case ResourceUriType.NextPage:
                return Url.Link("GetAuthors",
                    new
                    {
                        orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber + 1,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
            default:
                return Url.Link("GetAuthors",
                    new
                    {
                        orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
        }
    }

    [HttpGet("{authorId}", Name = "GetAuthor")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
    {
        var authorFromRepo = await _courseLibraryRepository.GetAuthorAsync(authorId);

        if (authorFromRepo == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto author)
    {
        var authorEntity = _mapper.Map<Entities.Author>(author);

        _courseLibraryRepository.AddAuthor(authorEntity);
        await _courseLibraryRepository.SaveAsync();

        var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

        return CreatedAtRoute("GetAuthor",
            new { authorId = authorToReturn.Id },
            authorToReturn);
    }

    [HttpOptions]
    public IActionResult GetAuthorsOptions()
    {
        Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");
        return Ok();
    }
}