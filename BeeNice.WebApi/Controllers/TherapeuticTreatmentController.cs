using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/TherapeuticTreatmentController")]
    [ApiController]
    [Authorize]
    public class TherapeuticTreatmentController : BaseController
    {
        private readonly ITherapeuticTreatmentRepository _therapeuticTreatmentRepository;

        public TherapeuticTreatmentController(ITherapeuticTreatmentRepository repository)
        {
            _therapeuticTreatmentRepository = repository;
        }

        [HttpGet]
        [Route("GetTherapeuticTreatments/{hiveId}")]
        public async Task<ActionResult<IEnumerable<TherapeuticTreatmentDto>>> GetItems(long hiveId)
        {
            try
            {
                var userId = GetUserId();
                List<TherapeuticTreatmentDto> therapeuticTreatmentDtos = new List<TherapeuticTreatmentDto>();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var reviews = await _therapeuticTreatmentRepository.GetItems(hiveId, userId);
                    therapeuticTreatmentDtos = TherapeuticTreatment2TherapeuticTreatmentDtoTranslator.Translate(reviews);
                }

                return Ok(therapeuticTreatmentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving queens");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<TherapeuticTreatmentDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                TherapeuticTreatment? therapeuticTreatment = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    therapeuticTreatment = await _therapeuticTreatmentRepository.GetItem(id, userId);
                    if (therapeuticTreatment != null)
                    {
                        var item = TherapeuticTreatment2TherapeuticTreatmentDtoTranslator.TranslateOne(therapeuticTreatment);
                        return Ok(item);
                    }
                }

                return NotFound(StatusCodes.Status404NotFound);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult<TherapeuticTreatmentDto>> Save(TherapeuticTreatmentDto therapeuticTreatment)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedTherapeuticTreatment = await _therapeuticTreatmentRepository.SaveItem(therapeuticTreatment, userId);
                    if (savedTherapeuticTreatment != null)
                    {
                        var item = TherapeuticTreatment2TherapeuticTreatmentDtoTranslator.TranslateOne(savedTherapeuticTreatment);
                        return Ok(item);
                    }
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving data");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<TherapeuticTreatmentDto>> Update(TherapeuticTreatmentDto therapeuticTreatment)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _therapeuticTreatmentRepository.EditItem(therapeuticTreatment, userId);
                    if (updatedItem != null)
                    {
                        var item = TherapeuticTreatment2TherapeuticTreatmentDtoTranslator.TranslateOne(updatedItem);
                        return Ok(item);
                    }
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpDelete]
        [Route("Remove/{id}")]
        public async Task<ActionResult> Remove(long id)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    await _therapeuticTreatmentRepository.Remove(id, userId);
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }
    }
}
