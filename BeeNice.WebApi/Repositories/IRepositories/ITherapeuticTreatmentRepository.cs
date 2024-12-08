using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Repositories.IRepositories;

public interface ITherapeuticTreatmentRepository
{
    Task<List<TherapeuticTreatment>> GetItems(long hiveId, string userId);
    Task<TherapeuticTreatment?> GetItem(long id, string userId);
    Task<TherapeuticTreatment?> SaveItem(TherapeuticTreatmentDto review, string userId);
    Task<TherapeuticTreatment?> EditItem(TherapeuticTreatmentDto review, string userId);
    Task<ActionResult> Remove(long id, string userId);
}