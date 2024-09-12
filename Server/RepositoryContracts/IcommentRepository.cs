using Entities;

namespace RepositoryContracts;

public interface IcommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(int id);
    Task<Comment> GetSingleAsync (int id);
    IQueryable<Comment> getMany();



}