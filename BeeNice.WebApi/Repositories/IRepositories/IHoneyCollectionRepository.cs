using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories;

public interface IHoneyCollectionRepository
{
    Task<List<HoneyCollection>> GetItems(long hiveId, string userId);
    Task<HoneyCollection?> GetItem(long id, string userId);
    Task<HoneyCollection?> SaveItem(HoneyCollectionDto honeyCollection, string userId);
    Task<HoneyCollection?> EditItem(HoneyCollectionDto honeyCollection, string userId);
    Task<ActionResult> Remove(long id, string userId);
}