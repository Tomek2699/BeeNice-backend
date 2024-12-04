using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IHiveRepository
    {
        Task<List<Hive>> GetItems(long apiaryId, string userId);
        Task<Hive?> GetItem(long id, string userId);
        Task<Hive?> SaveItem(HiveDto hive, string userId);
        Task<Hive?> EditItem(HiveDto hive, string userId);
        Task<ActionResult> Remove(long id, string userId);
    }
}
