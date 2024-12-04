using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/HiveController")]
    [ApiController]
    [Authorize]
    public class HiveController : BaseController
    {
        private readonly IHiveRepository _hiveRepository;

        public HiveController(IHiveRepository hiveRepository)
        {
            _hiveRepository = hiveRepository;
        }

        [HttpGet]
        [Route("GetHives/{apiaryId}")]
        public async Task<ActionResult<IEnumerable<HiveDto>>> GetItems(long apiaryId)
        {
            try
            {
                var userId = GetUserId();
                List<HiveDto> hiveDtos = new List<HiveDto>();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var hives = await _hiveRepository.GetItems(apiaryId, userId);
                    hiveDtos = Hive2HiveDtoTranslator.Translate(hives);
                }

                return Ok(hiveDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving hives");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<HiveDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                Hive? hive = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    hive = await _hiveRepository.GetItem(id, userId);
                    if (hive != null)
                    {
                        var item = Hive2HiveDtoTranslator.TranslateOne(hive);
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
        public async Task<ActionResult<HiveDto>> Save(HiveDto hive)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedHive = await _hiveRepository.SaveItem(hive, userId);
                    if (savedHive != null)
                    {
                        var item = Hive2HiveDtoTranslator.TranslateOne(savedHive);
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
        public async Task<ActionResult<HiveDto>> Update(HiveDto hive)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _hiveRepository.EditItem(hive, userId);
                    if (updatedItem != null)
                    {
                        var item = Hive2HiveDtoTranslator.TranslateOne(updatedItem);
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
                    await _hiveRepository.Remove(id, userId);
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
