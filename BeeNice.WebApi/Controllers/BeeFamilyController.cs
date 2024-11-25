using BeeNice.Models.Dtos;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/BeeFamilyController")]
    [ApiController]
    public class BeeFamilyController : ControllerBase
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
                var beFamiliese = await _beeFamilyRepository.GetItems(hiveId);
                if (!beFamiliese.Any())
                {
                    return NotFound();
                }

                var beeFamilyDtos = BeeFamily2BeeFamilyDtoTranslator.Translate(beFamiliese);
                return Ok(beeFamilyDtos);
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
                var savedBeeFamily = await _beeFamilyRepository.SaveItem(beeFamily);
                if (savedBeeFamily != null)
                {
                    var item = BeeFamily2BeeFamilyDtoTranslator.TranslateOne(savedBeeFamily);
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
        public async Task<ActionResult<BeeFamilyDto>> Get(long id)
        {
            try
            {
                var beeFamily = await _beeFamilyRepository.GetItem(id);
                if (beeFamily.Value == null)
                {
                    return NotFound(StatusCodes.Status404NotFound);
                }

                return Ok(beeFamily);
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
                await _beeFamilyRepository.Remove(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<BeeFamilyDto>> Update(BeeFamilyDto beeFamily)
        {
            try
            {
                var updatedItem = await _beeFamilyRepository.EditItem(beeFamily);
                if (updatedItem != null)
                {
                    var item = BeeFamily2BeeFamilyDtoTranslator.TranslateOne(updatedItem);
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
