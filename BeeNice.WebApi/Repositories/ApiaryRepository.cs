using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class ApiaryRepository : IApiaryRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public ApiaryRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Apiary>> GetItems(string userId)
        {
            var items = await _dbContext.Apiary.AsNoTracking().Where(i => i.UserId == userId).ToListAsync();
            return items;
        }

        public async Task<Apiary?> GetItem(long id, string userId)
        {
            var item = await _dbContext.Apiary.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            return item;
        }

        public async Task<Apiary?> SaveItem(ApiaryDto apiary, string userId)
        {
            var itemToSave = new Apiary
            {
                UserId = userId,
                Name = apiary.Name,
                CreationDate = DateTime.Now,
                Location = apiary.Location
            };

            _dbContext.Apiary.Add(itemToSave);
            await _dbContext.SaveChangesAsync();
            var returnedItem = GetItem(itemToSave.Id, userId).Result;
            return returnedItem;
        }

        public async Task<Apiary?> EditItem(ApiaryDto apiary, string userId)
        {
            var itemToUpdate = await _dbContext.Apiary.FirstOrDefaultAsync(i => i.Id == apiary.Id && i.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.Name != apiary.Name)
                {
                    itemToUpdate.Name = apiary.Name;
                }

                if (itemToUpdate.Location != apiary.Location)
                {
                    itemToUpdate.Location = apiary.Location;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.Apiary.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.Apiary.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
