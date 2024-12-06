using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class BeeFamilyRepository : IBeeFamilyRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public BeeFamilyRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BeeFamily>> GetItems(long hiveId, string userId)
        {
            var items = await _dbContext.BeeFamily.AsNoTracking()
                .Where(i => i.Hive.Apiary.UserId == userId && i.HiveId == hiveId)
                .ToListAsync();
            return items;
        }

        public async Task<BeeFamily?> GetItem(long id, string userId)
        {
            var item = await _dbContext.BeeFamily.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            return item;
        }

        public async Task<BeeFamily?> SaveItem(BeeFamilyDto beeFamily, string userId)
        {
            var hive = await _dbContext.Hive.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == beeFamily.HiveId && i.Apiary.UserId == userId);
            if (hive != null)
            {
                var itemToSave = new BeeFamily()
                {
                    HiveId = beeFamily.HiveId,
                    FamilyNumber = beeFamily.FamilyNumber,
                    FamilyState = beeFamily.FamilyState,
                    Race = beeFamily.Race,
                    CreationDate = beeFamily.CreationDate
                };

                _dbContext.BeeFamily.Add(itemToSave);
                await _dbContext.SaveChangesAsync();
                var returnedItem = GetItem(itemToSave.Id, userId).Result;
                return returnedItem;
            }

            return null;
        }

        public async Task<BeeFamily?> EditItem(BeeFamilyDto beeFamily, string userId)
        {
            var itemToUpdate = await _dbContext.BeeFamily.FirstOrDefaultAsync(i => i.Id == beeFamily.Id && i.Hive.Apiary.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.FamilyNumber != beeFamily.FamilyNumber)
                {
                    itemToUpdate.FamilyNumber = beeFamily.FamilyNumber;
                }

                if (itemToUpdate.FamilyState != beeFamily.FamilyState)
                {
                    itemToUpdate.FamilyState = beeFamily.FamilyState;
                }

                if (itemToUpdate.CreationDate != beeFamily.CreationDate)
                {
                    itemToUpdate.CreationDate = beeFamily.CreationDate;
                }

                if (itemToUpdate.Race != beeFamily.Race)
                {
                    itemToUpdate.Race = beeFamily.Race;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.BeeFamily.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.BeeFamily.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
