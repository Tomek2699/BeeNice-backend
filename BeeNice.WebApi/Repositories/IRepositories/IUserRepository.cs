namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> IsUserExist(string userId);
    }
}
