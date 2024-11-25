using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories;

public interface IHoneyCollectionRepository
{
    Task<List<HoneyCollection>> GetItems(long hiveId);
    Task<HoneyCollection?> SaveItem(HoneyCollectionDto honeyCollection);
    Task<ActionResult<HoneyCollection?>> GetItem(long id);
    Task<ActionResult> Remove(long id);
    Task<HoneyCollection?> EditItem(HoneyCollectionDto honeyCollection);
}