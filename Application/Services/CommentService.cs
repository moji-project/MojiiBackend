using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class CommentService (CommentRepository commentRepository)
{
    public async Task<List<CommentDto>> GetByPostId(int postId)
    {
        var comments = await commentRepository.GetByPostId(postId);
        return comments.Adapt<List<CommentDto>>();
    }

    public async Task<List<CommentDto>> GetByEventId(int eventId)
    {
        var comments = await commentRepository.GetByEventId(eventId);
        return comments.Adapt<List<CommentDto>>();
    }

    public async Task<CommentDto> CreateComment(CommentDto commentDto)
    {
        var normalizedContent = commentDto.Content?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedContent))
            throw new ArgumentException("Comment content cannot be empty.");
        if (normalizedContent.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters.");
        if (commentDto.PostId.HasValue == commentDto.EventId.HasValue)
            throw new ArgumentException("A comment must target exactly one entity: post or event.");

        Comment comment = new()
        {
            Content = normalizedContent,
            UserId = commentDto.UserId,
            PostId = commentDto.PostId,
            EventId = commentDto.EventId,
            User = null!
        };
        await commentRepository.Create(comment);

        var createdComment = await commentRepository.GetByIdWithRelations(comment.Id);
        return (createdComment ?? comment).Adapt<CommentDto>();
    }

    public async Task<CommentDto> UpdateComment(CommentDto commentDto)
    {
        var existingComment = await commentRepository.GetById(commentDto.Id);
        if (existingComment == null)
            throw new ArgumentException("Comment not found.");

        var normalizedContent = commentDto.Content?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedContent))
            throw new ArgumentException("Comment content cannot be empty.");
        if (normalizedContent.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters.");

        existingComment.Content = normalizedContent;
        await commentRepository.Update(existingComment);

        var updatedComment = await commentRepository.GetByIdWithRelations(existingComment.Id);
        return (updatedComment ?? existingComment).Adapt<CommentDto>();
    }
    
    public async Task<CommentDto> DeleteComment(int commentId)
    {
        var existingComment = await commentRepository.GetByIdWithRelations(commentId);
        if (existingComment == null)
            throw new ArgumentException("Comment not found.");

        await commentRepository.Delete(commentId);
        return existingComment.Adapt<CommentDto>();
    }
}
