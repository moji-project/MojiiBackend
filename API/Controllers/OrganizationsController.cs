using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationsController (OrganizationService organizationService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateOrganization([FromBody] OrganizationDto organizationDto)
    {
        await organizationService.CreateOrganization(organizationDto);
        return Ok();
    }
    
    [HttpGet("{organizationId:int}")]
    public async Task<ActionResult<OrganizationDto?>> GetOrganizationById(int organizationId)
    {
        var organization = await organizationService.GetOrganizationById(organizationId);
        return Ok(organization);
    }
}