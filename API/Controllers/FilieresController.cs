using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilieresController (FiliereService filiereService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateFiliere([FromBody] FiliereDto filiereDto)
    {
        await filiereService.CreateFiliere(filiereDto);
        return Ok();
    }
}