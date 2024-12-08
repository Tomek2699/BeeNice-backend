using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories;

public interface IReviewRepository
{
    Task<List<Review>> GetItems(long hiveId, string userId);
    Task<Review?> GetItem(long id, string userId);
    Task<Review?> SaveItem(ReviewDto review, string userId);
    Task<Review?> EditItem(ReviewDto review, string userId);
    Task<ActionResult> Remove(long id, string userId);
}