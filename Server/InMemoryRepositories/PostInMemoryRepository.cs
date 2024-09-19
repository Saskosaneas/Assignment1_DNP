    using Entities;
    using RepositoryContracts;

    namespace InMemoryRepositories;

    public class PostInMemoryRepository : IpostReporsitory
    {
        private List<Post> posts;

        public PostInMemoryRepository()
        {
            posts = new List<Post>();
        }

        public Task<Post> AddAsync(Post post)
        {
            post.ID = posts.Any()
                ? posts.Max(p => p.ID) + 1
                : 1;
            posts.Add(post);
            return Task.FromResult(post); 
            }

        public Task UpdateAsync(Post post)
        {
            Post? existingPost = posts.SingleOrDefault(p => p.ID == post.ID);
            if (existingPost == null)
            {
                throw new InvalidOperationException(
                    $"Post with ID'{post.ID}'not found");
            }
            posts.Remove(existingPost);
            posts.Add(post);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int ID)
        {
            Post? postToRemove = posts.SingleOrDefault(p => p.ID == ID);
            if (postToRemove is null)
            {
                throw new InvalidOperationException($"Post with ID '{ID}' not found");
            }

            posts.Remove(postToRemove);
            return Task.CompletedTask;
        }

        public Task<Post> GetSingleAsync(int id)
        {
            Post? post = posts.SingleOrDefault(p => p.ID == id);
            if (post is null)
            {
                throw new InvalidOperationException( $"Post with ID '{id}' not found");
            }
            return Task.FromResult(post);
        }

        public IQueryable<Post> GetMany()
        {
            return posts.AsQueryable();
        }

    }