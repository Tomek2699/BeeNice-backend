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

        public async Task<List<Hive>> GetItems(long apiaryId)
        {
            var items = await _dbContext.Hive.AsNoTracking().Where(i => i.ApiaryId == apiaryId).ToListAsync();
            return items;
        }

        public async Task<Hive?> SaveItem(HiveDto hive)
        {
            var itemToSave = new Hive
            {
                ApiaryId = hive.ApiaryId,
                HiveNumber = hive.HiveNumber,
                State = hive.State,
                Type = hive.Type
            };

            _dbContext.Hive.Add(itemToSave);
            long returnedId = await _dbContext.SaveChangesAsync();
            var returnedItem = GetItem(returnedId).Result?.Value;
            return returnedItem;
        }

        public async Task<ActionResult<Hive?>> GetItem(long id)
        {
            var item = await _dbContext.Hive.FindAsync(id);
            return item;
        }

        public async Task<ActionResult> Remove(long id)
        {
            var itemToRemove = await _dbContext.Hive.FindAsync(id);
            if (itemToRemove != null)
            {
                _dbContext.Hive.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }

        public async Task<Hive?> EditItem(HiveDto hive)
        {
            var itemToUpdate = await _dbContext.Hive.FindAsync(hive.Id);
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
    }
}
