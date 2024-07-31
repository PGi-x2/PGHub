using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public interface IPostsRepository
    {
        Post Create(Post post);
        bool Delete(Guid id);
        Post? Find(Guid id);
        IEnumerable<Post> GetAll();
        Post Update(Post post);
    }
}