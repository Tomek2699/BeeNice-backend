using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/HoneyCollectionController")]
    [ApiController]
    public class HoneyCollectionController: BaseController
    {
        private readonly IHoneyCollectionRepository _honeyCollectionRepository;

        public HoneyCollectionController(IHoneyCollectionRepository honeyCollectionrepository)
        {
            _honeyCollectionRepository = honeyCollectionrepository;
        }

        [HttpGet]
        [Route("GetHoneyCollections/{hiveId}")]
        public async Task<ActionResult<IEnumerable<BeeFamilyDto>>> GetItems(long hiveId)
        {
            try
            {
                var userId = GetUserId();
                List<HoneyCollectionDto> honeyCollectionDtos = new List<HoneyCollectionDto>();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var honeyCollections = await _honeyCollectionRepository.GetItems(hiveId, userId);
                    honeyCollectionDtos = HoneyCollection2HoneyCollectionDtoTranslator.Translate(honeyCollections);
                }

                return Ok(honeyCollectionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving honey collections");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<HoneyCollectionDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                HoneyCollection? honeyCollection = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    honeyCollection = await _honeyCollectionRepository.GetItem(id, userId);
                    if (honeyCollection != null)
                    {
                        var item = HoneyCollection2HoneyCollectionDtoTranslator.TranslateOne(honeyCollection);
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
        public async Task<ActionResult<HoneyCollectionDto>> Save(HoneyCollectionDto honeyCollection)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedHoneyCollection = await _honeyCollectionRepository.SaveItem(honeyCollection, userId);
                    if (savedHoneyCollection != null)
                    {
                        var item = HoneyCollection2HoneyCollectionDtoTranslator.TranslateOne(savedHoneyCollection);
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
        public async Task<ActionResult<HoneyCollectionDto>> Update(HoneyCollectionDto honeyCollection)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _honeyCollectionRepository.EditItem(honeyCollection, userId);
                    if (updatedItem != null)
                    {
                        var item = HoneyCollection2HoneyCollectionDtoTranslator.TranslateOne(updatedItem);
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
                    await _honeyCollectionRepository.Remove(id, userId);
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
