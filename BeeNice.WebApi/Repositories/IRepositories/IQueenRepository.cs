using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IQueenRepository
    {
        Task<List<Queen>> GetItems(long hiveId, string userId);
        Task<Queen?> GetItem(long id, string userId);
        Task<Queen?> SaveItem(QueenDto queen, string userId);
        Task<Queen?> EditItem(QueenDto queen, string userId);
        Task<ActionResult> Remove(long id, string userId);
    }
}
