using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using PdfApp.Application.Abstractions;
using PdfApp.Presentation.Api.Abstractions;
using PdfApp.Presentation.Api.Contracts;
using PdfApp.Presentation.Api.Contracts.Pdf;
using PdfApp.Presentation.Api.Mapping;
using PdfApp.Domain.Exceptions;

namespace PdfApp.Presentation.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PdfController : ApiControllerBase
{
    private readonly IPdfService _pdfService;

    public PdfController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpGet(ApiRoutes.Pdf.Get)]
    public async Task<IActionResult> Get([FromQuery] string? title = null)
    {
        var pdfs = (title is not null) 
            ?  await _pdfService.GetByTitleAsync(title)
            :  await _pdfService.GetAllAsync();

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
        var pdf = await _pdfService.GetByIdAsync(id);

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
        var pdf = await _pdfService.UpdateAsync(
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
        var pdf = await _pdfService.GetByIdAsync(id);
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

    private static string CreatePdfFileName()
    {
        return Guid.NewGuid().ToString() + ".pdf";
    }
}
