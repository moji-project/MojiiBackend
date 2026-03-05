using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController(ReportService reportService) : ControllerBase
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
    public async Task<ActionResult> CreateReport([FromBody] ReportDto reportDto)
    {
        await reportService.CreateReport(reportDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateReport([FromBody] ReportDto reportDto)
    {
        await reportService.UpdateReport(reportDto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteReport(int id)
    {
        await reportService.DeleteReport(id);
        return Ok();
    }
}
