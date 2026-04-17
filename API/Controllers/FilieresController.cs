using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilieresController (FiliereService filiereService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet("GetAllByOrganization/{organizationId:int}")]
    public async Task<ActionResult<List<FiliereDto>>> GetAllByOrganization(int organizationId)
    {
        var filieres = await filiereService.GetAllByOrganization(organizationId);
        return Ok(filieres);
    }

    [HttpPost]
    public async Task<ActionResult> CreateFiliere([FromBody] FiliereDto filiereDto)
    {
        await filiereService.CreateFiliere(filiereDto);
        await realtimeService.BroadcastEntityChanged("Filiere", "Created", filiereDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateFiliere([FromBody] FiliereDto filiereDto)
    {
        await filiereService.UpdateFiliere(filiereDto);
        await realtimeService.BroadcastEntityChanged("Filiere", "Updated", filiereDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFiliere(int id)
    {
        await filiereService.DeleteFiliere(id);
        await realtimeService.BroadcastEntityChanged("Filiere", "Deleted", new { id });
        return Ok();
    }
}
