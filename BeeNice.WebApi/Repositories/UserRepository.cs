using BeeNice.WebApi.Data;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BeeNiceDbContext _dbContext;
        public UserRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUserExist(string userId)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(i => i.Id == userId);
        }
    }
}
