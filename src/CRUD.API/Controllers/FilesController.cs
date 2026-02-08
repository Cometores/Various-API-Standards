using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CRUD.API.Controllers;

/// <summary>The FilesController handles file-related operations, such as downloading specific files.</summary>
[Route("api/files")]
[Authorize]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    /// <summary> <see cref="FilesController"/> constructor.</summary>
    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ??
                                            throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
    }

    /// <summary> Downloads a specific file by its identifier.</summary>
    /// <param name="fileId">The identifier of the file to download.</param>
    /// <returns>The requested file as a stream.</returns>
    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
        var pathToFile = "Lenna.png";

        if (!System.IO.File.Exists(pathToFile))
            return NotFound();

        if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}