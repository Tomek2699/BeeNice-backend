using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories
{
    public interface IQueenRepository
    {
        Task<List<Queen>> GetItems(long beeFamilyId);
        Task<Queen?> SaveItem(QueenDto queen);
        Task<ActionResult<Queen?>> GetItem(long id);
        Task<ActionResult> Remove(long id);
        Task<Queen?> EditItem(QueenDto queen);
    }
}
