using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories;

public interface IApiaryRepository
{
    Task<List<Apiary>> GetItems(string userId);
    Task<Apiary?> GetItem(long id, string userId);
    Task<Apiary?> SaveItem(ApiaryDto apiary, string userId);
    Task<Apiary?> EditItem(ApiaryDto apiary, string userId);
    Task<ActionResult> Remove(long id, string userId);
}