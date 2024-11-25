using BeeNice.Models.Dtos;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/HoneyCollectionController")]
    [ApiController]
    public class HoneyCollectionRepository : ControllerBase
    {
        private readonly IHoneyCollectionRepository _repository;

        public HoneyCollectionRepository(IHoneyCollectionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetHoneyCollections/{hiveId}")]
        public async Task<ActionResult<IEnumerable<BeeFamilyDto>>> GetItems(long hiveId)
        {
            try
            {
                var honeyCollections = await _repository.GetItems(hiveId);
                if (!honeyCollections.Any())
                {
                    return NotFound(null);
                }

                var beeFamilyDtos = HoneyCollection2HoneyCollectionDtoTranslator.Translate(honeyCollections);
                return Ok(beeFamilyDtos);
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
                var savedHoneyCollection = await _repository.SaveItem(honeyCollection);
                if (savedHoneyCollection != null)
                {
                    var item = HoneyCollection2HoneyCollectionDtoTranslator.TranslateOne(savedHoneyCollection);
                    return Ok(item);
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving data");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<HoneyCollectionDto>> Get(long id)
        {
            try
            {
                var honeyCollection = await _repository.GetItem(id);
                if (honeyCollection.Value == null)
                {
                    return NotFound(StatusCodes.Status404NotFound);
                }

                return Ok(honeyCollection);
            }
            catch
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
                await _repository.Remove(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<HoneyCollectionDto>> Update(HoneyCollectionDto honeyCollection)
        {
            try
            {
                var updatedItem = await _repository.EditItem(honeyCollection);
                if (updatedItem != null)
                {
                    var item = HoneyCollection2HoneyCollectionDtoTranslator.TranslateOne(updatedItem);
                    return Ok(item);
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
