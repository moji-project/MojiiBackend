using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class FiliereService (FiliereRepository filiereRepository)
{
    public async Task<List<FiliereWithInfosDto>> GetAllByOrganization(int organizationId)
    {
        var filieres = await filiereRepository.GetAllByOrganization(organizationId);
        List<FiliereWithInfosDto> filieresWithInfosDto = filieres.Adapt<List<FiliereWithInfosDto>>();
        foreach (var filiere in filieresWithInfosDto)
        {
            filiere.NbOfStudents = await filiereRepository.GetNbOfStudentsForFiliere(filiere.Id);
            filiere.NbOfMojiis = await filiereRepository.GetNbOfMojiisForFiliere(filiere.Id);
        }
        return filieresWithInfosDto.Adapt<List<FiliereWithInfosDto>>();
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

    public async Task<List<UserDto>> GetUsersByFiliere(int filiereId)
    {
        var users = await filiereRepository.GetUsersByFiliere(filiereId);
        return users.Adapt<List<UserDto>>();
    }
}