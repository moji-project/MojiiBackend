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

    public async Task<List<UserDto>> GetStudentsOfOrganizationOrderByCreationDate(int organizationId)
    {
        var users = await organizationRepository.GetStudentsOfOrganizationOrderByCreationDate(organizationId);
        return users.Select(user =>
        {
            var userDto = user.Adapt<UserDto>();
            userDto.NbOfPosts = user.CreatedPosts.Count;
            return userDto;
        }).ToList();
    }

    public async Task<OrganizationStatisticsDto> GetStatistics(int organizationId)
    {
        return new OrganizationStatisticsDto
        {
            NbOfStudents = await organizationRepository.GetNbOfStudents(organizationId),
            NbOfFilieres = await organizationRepository.GetNbOfFilieres(organizationId),
            NbOfPostsThisWeek = await organizationRepository.GetNbOfPostsThisWeek(organizationId),
            NbOfOpenReports = await organizationRepository.GetNbOfOpenReports(organizationId)
        };
    }

    public async Task UpdateOrganization(OrganizationDto organizationDto)
    {
        Organization organization = organizationDto.Adapt<Organization>();
        await  organizationRepository.Update(organization);
    }
}
