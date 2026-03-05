using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class CommentService (CommentRepository commentRepository)
{
    public async Task CreateComment(CommentDto commentDto)
    {
        Comment comment = commentDto.Adapt<Comment>();
        await commentRepository.Create(comment);
    }

    public async Task UpdateComment(CommentDto commentDto)
    {
        Comment comment = commentDto.Adapt<Comment>();
        await  commentRepository.Update(comment);
    }
    
    public async  Task DeleteComment(int commentId)
    {
        await commentRepository.Delete(commentId);
    }
}