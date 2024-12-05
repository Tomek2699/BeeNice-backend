using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class QueenRepository : IQueenRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public QueenRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Queen>> GetItems(long hiveId, string userId)
        {
            var items = await _dbContext.Queen.AsNoTracking()
                .Where(i => i.Hive.Apiary.UserId == userId && i.HiveId == hiveId)
                .ToListAsync();
            return items;
        }

        public async Task<Queen?> GetItem(long id, string userId)
        {
            var item = await _dbContext.Queen.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            return item;
        }

        public async Task<Queen?> SaveItem(QueenDto queen, string userId)
        {
            var hive = await _dbContext.Hive.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == queen.HiveId && i.Apiary.UserId == userId);
            if (hive != null)
            {
                var itemToSave = new Queen()
                {
                    QueenNumber = queen.QueenNumber,
                    State = queen.State,
                    HiveId = queen.HiveId,
                    Race = queen.Race,
                    HatchDate = queen.HatchDate,
                };

                _dbContext.Queen.Add(itemToSave);
                await _dbContext.SaveChangesAsync();
                var returnedItem = GetItem(itemToSave.Id, userId).Result;
                return returnedItem;
            }

            return null;
        }

        public async Task<Queen?> EditItem(QueenDto queen, string userId)
        {
            var itemToUpdate = await _dbContext.Queen.FirstOrDefaultAsync(i => i.Id == queen.Id && i.Hive.Apiary.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.QueenNumber != queen.QueenNumber)
                {
                    itemToUpdate.QueenNumber = queen.QueenNumber;
                }

                if (itemToUpdate.State != queen.State)
                {
                    itemToUpdate.State = queen.State;
                }

                if (itemToUpdate.HatchDate != queen.HatchDate)
                {
                    itemToUpdate.HatchDate = queen.HatchDate;
                }

                if (itemToUpdate.Race != queen.Race)
                {
                    itemToUpdate.Race = queen.Race;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.Queen.FirstOrDefaultAsync(i => i.Id == id && i.Hive.Apiary.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.Queen.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
