using BeeNice.Models.Dtos;
using BeeNice.WebApi.Data;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Repositories
{
    public class TherapeuticTreatmentRepository : ITherapeuticTreatmentRepository
    {
        private readonly BeeNiceDbContext _dbContext;

        public TherapeuticTreatmentRepository(BeeNiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TherapeuticTreatment>> GetItems(long hiveId, string userId)
        {
            var items = await _dbContext.TherapeuticTreatment.AsNoTracking()
                .Where(i => i.Hive.Apiary.UserId == userId && i.HiveId == hiveId)
                .ToListAsync();
            return items;
        }

        public async Task<TherapeuticTreatment?> GetItem(long id, string userId)
        {
            var item = await _dbContext.TherapeuticTreatment.FirstOrDefaultAsync(i => i.Id 
                == id && i.Hive.Apiary.UserId == userId);
            return item;
        }

        public async Task<TherapeuticTreatment?> SaveItem(TherapeuticTreatmentDto therapeuticTreatment, string userId)
        {
            var hive = await _dbContext.Hive.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == therapeuticTreatment.HiveId && i.Apiary.UserId == userId);
            if (hive != null)
            {
                var itemToSave = new TherapeuticTreatment()
                {
                    TreatmentDate = therapeuticTreatment.TreatmentDate,
                    Medicine = therapeuticTreatment.Medicine,
                    Dose = therapeuticTreatment.Dose,
                    Comment = therapeuticTreatment.Comment,
                    HiveId = therapeuticTreatment.HiveId,
                };

                _dbContext.TherapeuticTreatment.Add(itemToSave);
                await _dbContext.SaveChangesAsync();
                var returnedItem = GetItem(itemToSave.Id, userId).Result;
                return returnedItem;
            }

            return null;
        }

        public async Task<TherapeuticTreatment?> EditItem(TherapeuticTreatmentDto therapeuticTreatment, string userId)
        {
            var itemToUpdate = await _dbContext.TherapeuticTreatment.FirstOrDefaultAsync(i => i.Id 
                == therapeuticTreatment.Id && i.Hive.Apiary.UserId == userId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.TreatmentDate != therapeuticTreatment.TreatmentDate)
                {
                    itemToUpdate.TreatmentDate = therapeuticTreatment.TreatmentDate;
                }

                if (itemToUpdate.Medicine != therapeuticTreatment.Medicine)
                {
                    itemToUpdate.Medicine = therapeuticTreatment.Medicine;
                }

                if (itemToUpdate.Dose != therapeuticTreatment.Dose)
                {
                    itemToUpdate.Dose = therapeuticTreatment.Dose;
                }

                if (itemToUpdate.Comment != therapeuticTreatment.Comment)
                {
                    itemToUpdate.Comment = therapeuticTreatment.Comment;
                }
            }

            await _dbContext.SaveChangesAsync();
            return itemToUpdate;
        }

        public async Task<ActionResult> Remove(long id, string userId)
        {
            var itemToRemove = await _dbContext.TherapeuticTreatment.FirstOrDefaultAsync(i => i.Id 
                == id && i.Hive.Apiary.UserId == userId);
            if (itemToRemove != null)
            {
                _dbContext.TherapeuticTreatment.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
