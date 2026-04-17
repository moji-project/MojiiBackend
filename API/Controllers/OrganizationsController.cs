using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationsController (OrganizationService organizationService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet("{organizationId:int}")]
    public async Task<ActionResult<OrganizationDto?>> GetOrganizationById(int organizationId)
    {
        var organization = await organizationService.GetOrganizationById(organizationId);
        return Ok(organization);
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrganization([FromBody] OrganizationDto organizationDto)
    {
        await organizationService.CreateOrganization(organizationDto);
        await realtimeService.BroadcastEntityChanged("Organization", "Created", organizationDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateOrganization([FromBody] OrganizationDto organizationDto)
    {
        await organizationService.UpdateOrganization(organizationDto);
        await realtimeService.BroadcastEntityChanged("Organization", "Updated", organizationDto);
        return Ok();
    }
}
