using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class HoneyCollectionRepository : IHoneyCollectionRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public HoneyCollectionRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HoneyCollection>> GetItems(long hiveId, string userId)
        {
            var items = await _dbContext.HoneyCollection.AsNoTracking()
                .Where(i => i.Hive.Apiary.UserId == userId && i.HiveId == hiveId)
                .ToListAsync();
            return items;
        }

        public async Task<HoneyCollection?> GetItem(long id, string userId)
        {
            var item = await _dbContext.HoneyCollection.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            return item;
        }

        public async Task<HoneyCollection?> SaveItem(HoneyCollectionDto honeyCollection, string userId)
        {
            var hive = await _dbContext.Hive.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == honeyCollection.HiveId && i.Apiary.UserId == userId);
            if (hive != null)
            {
                var itemToSave = new HoneyCollection()
                {
                    HiveId = honeyCollection.HiveId,
                    HoneyQuantity = honeyCollection.HoneyQuantity,
                    CollectionDate = honeyCollection.CollectionDate,
                    TypeOfHoney = honeyCollection.TypeOfHoney,
                };

                _dbContext.HoneyCollection.Add(itemToSave);
                await _dbContext.SaveChangesAsync();
                var returnedItem = GetItem(itemToSave.Id, userId).Result;
                return returnedItem;
            }

            return null;
        }

        public async Task<HoneyCollection?> EditItem(HoneyCollectionDto honeyCollection, string userId)
        {
            var itemToUpdate = await _dbContext.HoneyCollection
                .FirstOrDefaultAsync(i => i.Id == honeyCollection.Id && i.Hive.Apiary.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.HoneyQuantity != honeyCollection.HoneyQuantity)
                {
                    itemToUpdate.HoneyQuantity = honeyCollection.HoneyQuantity;
                }

                if (itemToUpdate.TypeOfHoney != honeyCollection.TypeOfHoney)
                {
                    itemToUpdate.TypeOfHoney = honeyCollection.TypeOfHoney;
                }

                if (itemToUpdate.CollectionDate != honeyCollection.CollectionDate)
                {
                    itemToUpdate.CollectionDate = honeyCollection.CollectionDate;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.HoneyCollection
                .FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.HoneyCollection.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
