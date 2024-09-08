using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfApp.Application.Abstractions;
using PdfApp.Domain.Exceptions;
using PdfApp.Presentation.Api.Abstractions;
using PdfApp.Presentation.Api.Contracts;
using PdfApp.Presentation.Api.Contracts.Pdf;
using PdfApp.Presentation.Api.Mapping;
using System.Net.Mime;

namespace PdfApp.Presentation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PdfController : ApiControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPdfService _pdfService;

    public PdfController(IAuthService authService, IPdfService pdfService)
    {
        _authService = authService;
        _pdfService = pdfService;
    }

    [AllowAnonymous]
    [HttpGet("/health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { message = "Healthy" });
    }

    [HttpGet(ApiRoutes.Pdf.Get)]
    public async Task<IActionResult> Get([FromQuery] string? title = null, string? tags = null)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
               return Unauthorized();

        string[] tagsArray = tags?.Split(',') ?? [];
        var pdfs = (title is not null) 
            ?  await _pdfService.GetByTitleAsync(user.Id, title, tagsArray)
            :  await _pdfService.GetAllAsync(user.Id, tagsArray);

        return Ok(new { pdfs = pdfs.DomainToResponse() });
    }

    [HttpGet(ApiRoutes.Pdf.GetFile)]
    public async Task<IActionResult> GetFile(string fileName)
    {
        var file = await _pdfService.GetPdfFileAsync(fileName);
        return File(file, MediaTypeNames.Application.Pdf);
    }

    [HttpGet(ApiRoutes.Pdf.GetById)]
    public async Task<IActionResult> Get(int id)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        var pdf = await _pdfService.GetByIdAsync(user.Id, id);

        return Ok(new { pdf = pdf.DomainToResponse() });
    }

    [HttpPost(ApiRoutes.Pdf.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePdfRequest request)
    {
        var pdf = await _pdfService.CreateAsync(
            request.Title,
            request.Description,
            request.Author,
            request.TotalPages,
            CreatePdfFileName(),
            request.Tags.RequestToDomain(),
            false);

        return CreatedAtAction(
            nameof(Get),
            new { id = pdf.Id },
            new { pdf = pdf.DomainToResponse() });
    }

    [HttpPost(ApiRoutes.Pdf.Upload)]
    [RequestSizeLimit(1073741824)]
    [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)] 
    public async Task<IActionResult> Upload(int id, [FromForm] UploadPdfRequest request)
    {
        if (request.File.ContentType != MediaTypeNames.Application.Pdf)
            throw new InvalidFileTypeException("PDF");

        await _pdfService.UploadAsync(id, request.File);

        return Ok();
    }

    [HttpPatch(ApiRoutes.Pdf.Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePdfRequest request)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        var pdf = await _pdfService.UpdateAsync(
            user.Id,
            id,
            request.Title,
            request.Description,
            request.Author,
            request.TotalPages,
            request.FileName,
            request.Tags?.RequestToDomain());

        return Ok(new { pdf = pdf.DomainToResponse() });
    }

    [HttpDelete(ApiRoutes.Pdf.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        var pdf = await _pdfService.GetByIdAsync(user.Id, id);
        if (pdf is null)
            throw new PdfNotFoundException();

        await _pdfService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet(ApiRoutes.Pdf.GetTags)]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _pdfService.GetTagsAsync();

        return Ok(new { tags = tags.DomainToResponse() });
    }

    [HttpPost(ApiRoutes.Pdf.SaveProgress)]
    public async Task<IActionResult> SaveProgress([FromRoute] int pdfId, [FromBody] ProgressRequest request)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        await _pdfService.SaveProgressAsync(user.Id, pdfId, request.Page);
        var pdf = await _pdfService.GetByIdAsync(user.Id, pdfId);

        return Ok(new { pdf = pdf.DomainToResponse(), page = request.Page });
    }

    [HttpGet(ApiRoutes.Pdf.GetProgresses)]
    public async Task<IActionResult> GetProgresses(int pdfId)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        var progresses = await _pdfService.GetProgressesAsync(user.Id, pdfId);

        return Ok(new { progresses = progresses.DomainToResponse() });
    }

    [HttpPost(ApiRoutes.Pdf.AddToFavorites)]
    public async Task<IActionResult> AddToFavorites(int pdfId)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        await _pdfService.AddToFavorites(user.Id, pdfId);

        return Ok();
    }

    [HttpGet(ApiRoutes.Pdf.GetUserFavoritePdfs)]
    public async Task<IActionResult> GetUserFavoritePdfs([FromQuery] string? title = null, string? tags = null)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        string[] tagsArray = tags?.Split(',') ?? [];
        var pdfs = await _pdfService.GetUserFavoritePdfs(user.Id, title, tagsArray);

        return Ok(new { pdfs = pdfs.DomainToResponse() });
    }

    [HttpDelete(ApiRoutes.Pdf.RemoveFromFavorites)]
    public async Task<IActionResult> RemoveFromFavorites(int pdfId)
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
            return Unauthorized();

        await _pdfService.RemoveFromFavorites(user.Id, pdfId);

        return Ok();
    }

    private static string CreatePdfFileName()
    {
        return Guid.NewGuid().ToString() + ".pdf";
    }
}
