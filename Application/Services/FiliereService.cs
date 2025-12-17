using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class FiliereService (FiliereRepository filiereRepository)
{
    public async Task CreateFiliere(FiliereDto filiereDto)
    {
        Filiere filiere = filiereDto.Adapt<Filiere>();
        await filiereRepository.Create(filiere);
    }
}