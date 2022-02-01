using Microsoft.EntityFrameworkCore;

namespace aspnetserver.Data
{
    public class PostRepository
    {
        private readonly AppDbContext _dbContext;

        public PostRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetPosts()
        {
            return await _dbContext.Posts.ToListAsync();
        }

        public async Task<Post?> GetPostById(int postId)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
        }

        public async Task<bool> CreatePost(Post post)
        {
            _dbContext.Add(post);

            return await _dbContext.SaveChangesAsync() >= 1;
        }

        public async Task<bool> UpdatePost(Post post)
        {
            _dbContext.Update(post);

            return await _dbContext.SaveChangesAsync() >= 1;
        }

        public async Task<bool> DeletePost(int postId)
        {
            var post = await GetPostById(postId);

            if (post == null) return false;

            _dbContext.Remove(post);

            return await _dbContext.SaveChangesAsync() >= 1;
        }
    }
}
