using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories;

public interface IApiaryRepository
{
    Task<List<Apiary>> GetItems(string userId);
    Task<Apiary?> SaveItem(ApiaryDto apiary, string userId);
    Task<ActionResult<Apiary?>> GetItem(long id);
    Task<ActionResult> Remove(long id);
    Task<Apiary?> EditItem(ApiaryDto apiary);
}