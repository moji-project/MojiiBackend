using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class ReportService(ReportRepository reportRepository)
{
    public async Task<List<ReportDto>> GetAllReports()
    {
        var reports = await reportRepository.GetAll();
        return reports.Adapt<List<ReportDto>>();
    }

    public async Task<ReportDto?> GetReportById(int id)
    {
        var report = await reportRepository.GetById(id);
        return report?.Adapt<ReportDto>();
    }

    public async Task CreateReport(ReportDto reportDto)
    {
        Report report = reportDto.Adapt<Report>();
        await reportRepository.Create(report);
    }

    public async Task UpdateReport(ReportDto reportDto)
    {
        Report report = reportDto.Adapt<Report>();
        await reportRepository.Update(report);
    }

    public async Task DeleteReport(int id)
    {
        await reportRepository.Delete(id);
    }
}
