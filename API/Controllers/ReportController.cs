using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController(ReportService reportService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ReportDto>>> GetAllReports()
    {
        var reports = await reportService.GetAllReports();
        return Ok(reports);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReportDto>> GetReportById(int id)
    {
        var report = await reportService.GetReportById(id);
        if (report == null)
            return NotFound();
        return Ok(report);
    }

    [HttpPost]
    public async Task<ActionResult<ReportDto>> CreateReport([FromBody] CreateReportRequestDto createReportRequestDto)
    {
        try
        {
            var createdReport = await reportService.CreateReport(createReportRequestDto);
            await realtimeService.BroadcastEntityChanged("Report", "Created", createdReport);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.Id }, createdReport);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateReport([FromBody] ReportDto reportDto)
    {
        await reportService.UpdateReport(reportDto);
        await realtimeService.BroadcastEntityChanged("Report", "Updated", reportDto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteReport(int id)
    {
        await reportService.DeleteReport(id);
        await realtimeService.BroadcastEntityChanged("Report", "Deleted", new { id });
        return Ok();
    }
}
