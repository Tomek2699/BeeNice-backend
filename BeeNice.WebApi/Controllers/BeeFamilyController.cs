using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/BeeFamilyController")]
    [ApiController]
    public class BeeFamilyController : BaseController
    {
        private readonly IBeeFamilyRepository _beeFamilyRepository;

        public BeeFamilyController(IBeeFamilyRepository beeFamilyRepository)
        {
            _beeFamilyRepository = beeFamilyRepository;
        }

        [HttpGet]
        [Route("GetBeeFamilies/{hiveId}")]
        public async Task<ActionResult<IEnumerable<BeeFamilyDto>>> GetItems(long hiveId)
        {
            try
            {
                var userId = GetUserId();
                List<BeeFamilyDto> beeFamilyDtos = new List<BeeFamilyDto>();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var beeFamilies = await _beeFamilyRepository.GetItems(hiveId, userId);
                    beeFamilyDtos = BeeFamily2BeeFamilyDtoTranslator.Translate(beeFamilies);
                }

                return Ok(beeFamilyDtos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving bee families");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<BeeFamilyDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                BeeFamily? beeFamily = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    beeFamily = await _beeFamilyRepository.GetItem(id, userId);
                    if (beeFamily != null)
                    {
                        var item = BeeFamily2BeeFamilyDtoTranslator.TranslateOne(beeFamily);
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
        public async Task<ActionResult<BeeFamilyDto>> Save(BeeFamilyDto beeFamily)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedBeeFamily = await _beeFamilyRepository.SaveItem(beeFamily, userId);
                    if (savedBeeFamily != null)
                    {
                        var item = BeeFamily2BeeFamilyDtoTranslator.TranslateOne(savedBeeFamily);
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
        public async Task<ActionResult<BeeFamilyDto>> Update(BeeFamilyDto beeFamily)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _beeFamilyRepository.EditItem(beeFamily, userId);
                    if (updatedItem != null)
                    {
                        var item = BeeFamily2BeeFamilyDtoTranslator.TranslateOne(updatedItem);
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
                    await _beeFamilyRepository.Remove(id, userId);
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
