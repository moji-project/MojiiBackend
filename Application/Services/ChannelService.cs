using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;
using Mapster;

namespace MojiiBackend.Application.Services;

public class ChannelService (ChannelRepository channelRepository)
{
    public async Task<List<ChannelDto>> GetAllChannelsForUser(int userId)
    {
        var channels = await channelRepository.GetAllChannelsForUser(userId);
        return channels.Adapt<List<ChannelDto>>();
    }
    
    public async Task CreateChannel(ChannelDto channelDto)
    {
        var channel = channelDto.Adapt<Channel>();
        await channelRepository.Create(channel);
    }
    
    public async Task DeleteChannel(int id)
    {
        await channelRepository.Delete(id);
    }
}