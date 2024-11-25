using BeeNice.Models.Dtos;
using BeeNice.WebApi.Repositories;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/HiveController")]
    [ApiController]
    public class HiveController : ControllerBase
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
                var hives = await _hiveRepository.GetItems(apiaryId);
                if (!hives.Any())
                {
                    return NotFound();
                }

                var hiveDtos = Hive2HiveDtoTranslator.Translate(hives);
                return Ok(hiveDtos);
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
                var savedHive = await _hiveRepository.SaveItem(hive);
                if (savedHive != null)
                {
                    var item = Hive2HiveDtoTranslator.TranslateOne(savedHive);
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
        public async Task<ActionResult<HiveDto>> Get(long id)
        {
            try
            {
                var hive = await _hiveRepository.GetItem(id);
                if (hive.Value == null)
                {
                    return NotFound(StatusCodes.Status404NotFound);
                }

                return Ok(hive);
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
                await _hiveRepository.Remove(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<HiveDto>> Update(HiveDto hive)
        {
            try
            {
                var updatedItem = await _hiveRepository.EditItem(hive);
                if (updatedItem != null)
                {
                    var item = Hive2HiveDtoTranslator.TranslateOne(updatedItem);
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
