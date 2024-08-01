using Microsoft.EntityFrameworkCore;
using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly DataContext _dataContext;

        public PostsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Post? Find(Guid id)
        {
            return _dataContext.Posts.Find(id);
        }

        public IEnumerable<Post> GetAll()
        {
            // think for big results in order to retrieve first 100 results and then the rest, to be performant
            return _dataContext.Posts.Include(p => p.Attachments);
        }

        public Post Create(Post post)
        {
            _dataContext.Posts.Add(post);
            _dataContext.SaveChanges();

            return post;
        }

        public Post Update(Post post)
        {
            var postDb = _dataContext.Posts.Find(post.Id);

            if (postDb != null)
            {
                // postDb.Attachments.Clear();
                _dataContext.Entry(postDb).CurrentValues.SetValues(post);
                _dataContext.SaveChanges();
            }
            else
            {
                return null;
            }

            return post;
        }

        public bool Delete(Guid id)
        {
            //var post = _dataContext.Posts.Find(id);
            var post = _dataContext.Posts.Where(p => p.Id == id).Include(p => p.Attachments).FirstOrDefault();

            if (post != null)
            {
                _dataContext.Posts.Remove(post);
                _dataContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
