using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IBeeFamilyRepository
    {
        Task<List<BeeFamily>> GetItems(long hiveId);
        Task<BeeFamily?> SaveItem(BeeFamilyDto hive);
        Task<ActionResult<BeeFamily?>> GetItem(long id);
        Task<ActionResult> Remove(long id);
        Task<BeeFamily?> EditItem(BeeFamilyDto hive);
    }
}
