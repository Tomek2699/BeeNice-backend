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

        public async Task<ActionResult<Queen?>> GetItem(long id)
        {
            var item = await _dbContext.Queen.FindAsync(id);
            return item;
        }

        public async Task<List<Queen>> GetItems(long beeFamilyId)
        {
            var items = await _dbContext.Queen.AsNoTracking().Where(i => i.BeeFamilyId == beeFamilyId).ToListAsync();
            return items;
        }

        public async Task<Queen?> EditItem(QueenDto queen)
        {
            var itemToUpdate = await _dbContext.Queen.FindAsync(queen.Id);
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

        public async Task<ActionResult> Remove(long id)
        {
            var itemToRemove = await _dbContext.Queen.FindAsync(id);
            if (itemToRemove != null)
            {
                _dbContext.Queen.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }

        public async Task<Queen?> SaveItem(QueenDto queen)
        {
            var itemToSave = new Queen()
            {
                QueenNumber = queen.QueenNumber,
                State = queen.State,
                BeeFamilyId = queen.BeeFamilyId,
                Race = queen.Race,
                HatchDate = queen.HatchDate,
            };

            _dbContext.Queen.Add(itemToSave);
            long returnedId = await _dbContext.SaveChangesAsync();
            var returnedItem = GetItem(returnedId).Result?.Value;
            return returnedItem;
        }
    }
}
