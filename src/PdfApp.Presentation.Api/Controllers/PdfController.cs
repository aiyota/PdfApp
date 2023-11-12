using Microsoft.AspNetCore.Mvc;
using PdfApp.Application.Abstractions;
using PdfApp.Presentation.Api.Abstractions;
using PdfApp.Presentation.Api.Contracts;
using PdfApp.Presentation.Api.Contracts.Pdf;
using PdfApp.Presentation.Api.Mapping;

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
            request.FileName,
            request.Tags);

        return CreatedAtAction(
            nameof(Get),
            new { id = pdf.Id },
            new { pdf = pdf.DomainToResponse() });
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
            request.Tags);

        return Ok(new { pdf = pdf.DomainToResponse() });
    }

    [HttpDelete(ApiRoutes.Pdf.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        await _pdfService.DeleteAsync(id);

        return NoContent();
    }
}
