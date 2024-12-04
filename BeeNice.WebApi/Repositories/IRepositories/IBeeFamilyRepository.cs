using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IBeeFamilyRepository
    {
        Task<List<BeeFamily>> GetItems(long hiveId, string userId);
        Task<BeeFamily?> GetItem(long id, string userId);
        Task<BeeFamily?> SaveItem(BeeFamilyDto hive, string userId);
        Task<BeeFamily?> EditItem(BeeFamilyDto hive, string userId);
        Task<ActionResult> Remove(long id, string userId);
    }
}
