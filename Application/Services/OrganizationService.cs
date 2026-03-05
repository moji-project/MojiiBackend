using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class OrganizationService (OrganizationRepository organizationRepository)
{
    public async Task CreateOrganization(OrganizationDto organizationDto)
    {
        Organization organization = organizationDto.Adapt<Organization>();
        await organizationRepository.Create(organization);
    }

    public async Task<OrganizationDto?> GetOrganizationById(int organizationId)
    {
        Organization? organization = await organizationRepository.GetById(organizationId);
        return organization.Adapt<OrganizationDto>();
    }

    public async Task UpdateOrganization(OrganizationDto organizationDto)
    {
        Organization organization = organizationDto.Adapt<Organization>();
        await  organizationRepository.Update(organization);
    }
}