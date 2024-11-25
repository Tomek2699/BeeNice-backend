using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IHiveRepository
    {
        Task<List<Hive>> GetItems(long apiaryId);
        Task<Hive?> SaveItem(HiveDto hive);
        Task<ActionResult<Hive?>> GetItem(long id);
        Task<ActionResult> Remove(long id);
        Task<Hive?> EditItem(HiveDto hive);
    }
}
