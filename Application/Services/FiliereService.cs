using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class FiliereService (FiliereRepository filiereRepository)
{
    public async Task<List<FiliereDto>> GetAllByOrganization(int organizationId)
    {
        var filieres = await filiereRepository.GetAllByOrganization(organizationId);
        return filieres.Adapt<List<FiliereDto>>();
    }

    public async Task CreateFiliere(FiliereDto filiereDto)
    {
        Filiere filiere = filiereDto.Adapt<Filiere>();
        await filiereRepository.Create(filiere);
    }

    public async Task UpdateFiliere(FiliereDto filiereDto)
    {
        Filiere filiere = filiereDto.Adapt<Filiere>();
        await filiereRepository.Update(filiere);
    }

    public async Task DeleteFiliere(int id)
    {
        await filiereRepository.Delete(id);
    }
}