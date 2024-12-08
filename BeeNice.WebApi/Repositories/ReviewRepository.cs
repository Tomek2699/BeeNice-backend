using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public ReviewRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Review>> GetItems(long hiveId, string userId)
        {
            var items = await _dbContext.Review.AsNoTracking()
                .Where(i => i.Hive.Apiary.UserId == userId && i.HiveId == hiveId)
                .ToListAsync();
            return items;
        }

        public async Task<Review?> GetItem(long id, string userId)
        {
            var item = await _dbContext.Review.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            return item;
        }

        public async Task<Review?> SaveItem(ReviewDto review, string userId)
        {
            var hive = await _dbContext.Hive.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == review.HiveId && i.Apiary.UserId == userId);
            if (hive != null)
            {
                var itemToSave = new Review()
                {
                    ReviewDate = review.ReviewDate,
                    FamilyState = review.FamilyState,
                    Comment = review.Comment,
                    HiveId = review.HiveId,
                };

                _dbContext.Review.Add(itemToSave);
                await _dbContext.SaveChangesAsync();
                var returnedItem = GetItem(itemToSave.Id, userId).Result;
                return returnedItem;
            }

            return null;
        }

        public async Task<Review?> EditItem(ReviewDto review, string userId)
        {
            var itemToUpdate = await _dbContext.Review.FirstOrDefaultAsync(i => i.Id == review.Id && i.Hive.Apiary.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.ReviewDate != review.ReviewDate)
                {
                    itemToUpdate.ReviewDate = review.ReviewDate;
                }

                if (itemToUpdate.FamilyState != review.FamilyState)
                {
                    itemToUpdate.FamilyState = review.FamilyState;
                }

                if (itemToUpdate.Comment != review.Comment)
                {
                    itemToUpdate.Comment = review.Comment;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.Review.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.Review.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
