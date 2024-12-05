using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class HiveRepository : IHiveRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public HiveRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hive>> GetItems(long apiaryId, string userId)
        {
            var items = await _dbContext.Hive.AsNoTracking()
                .Where(i => i.Apiary.UserId == userId && i.ApiaryId == apiaryId)
                .ToListAsync();
            return items;
        }

        public async Task<Hive?> GetItem(long id, string userId)
        {
            var item = await _dbContext.Hive.FirstOrDefaultAsync(i => i.Id == id && i.Apiary.UserId == userId);
            return item;
        }

        public async Task<Hive?> SaveItem(HiveDto hive, string userId)
        {
            var apiary = await _dbContext.Apiary.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == hive.ApiaryId && i.UserId == userId);
            if (apiary != null)
            {
                var itemToSave = new Hive
                {
                    ApiaryId = hive.ApiaryId,
                    HiveNumber = hive.HiveNumber,
                    State = hive.State,
                    Type = hive.Type
                };

                _dbContext.Hive.Add(itemToSave);
                await _dbContext.SaveChangesAsync();
                var returnedItem = GetItem(itemToSave.Id, userId).Result;
                return returnedItem;
            }

            return null;
        }

        public async Task<Hive?> EditItem(HiveDto hive, string userId)
        {
            var itemToUpdate = await _dbContext.Hive.FirstOrDefaultAsync(i => i.Id == hive.Id && i.Apiary.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.HiveNumber != hive.HiveNumber)
                {
                    itemToUpdate.HiveNumber = hive.HiveNumber;
                }

                if (itemToUpdate.State != hive.State)
                {
                    itemToUpdate.State = hive.State;
                }

                if (itemToUpdate.Type != hive.Type)
                {
                    itemToUpdate.Type = hive.Type;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.Hive.FirstOrDefaultAsync(i => i.Id == id && i.Apiary.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.Hive.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
