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

        public async Task<ActionResult<HoneyCollection?>> GetItem(long id)
        {
            var item = await _dbContext.HoneyCollection.FindAsync(id);
            return item;
        }

        public async Task<List<HoneyCollection>> GetItems(long hiveId)
        {
            var items = await _dbContext.HoneyCollection.AsNoTracking().Where(i => i.HiveId == hiveId).ToListAsync();
            return items;
        }

        public async Task<HoneyCollection?> EditItem(HoneyCollectionDto honeyCollection)
        {
            var itemToUpdate = await _dbContext.HoneyCollection.FindAsync(honeyCollection.Id);
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

        public async Task<ActionResult> Remove(long id)
        {
            var itemToRemove = await _dbContext.HoneyCollection.FindAsync(id);
            if (itemToRemove != null)
            {
                _dbContext.HoneyCollection.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }

        public async Task<HoneyCollection?> SaveItem(HoneyCollectionDto honeyCollection)
        {
            var itemToSave = new HoneyCollection()
            {
                HiveId = honeyCollection.HiveId,
                HoneyQuantity = honeyCollection.HoneyQuantity,
                CollectionDate = honeyCollection.CollectionDate,
                TypeOfHoney = honeyCollection.TypeOfHoney,
            };

            _dbContext.HoneyCollection.Add(itemToSave);
            long returnedId = await _dbContext.SaveChangesAsync();
            var returnedItem = GetItem(returnedId).Result?.Value;
            return returnedItem;
        }
    }
}
