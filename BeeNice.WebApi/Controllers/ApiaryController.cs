using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/ApiaryController")]
    [ApiController]
    [Authorize]
    public class ApiaryController : BaseController
    {
        private readonly IApiaryRepository _apiaryRepository;

        public ApiaryController(IApiaryRepository apiaryRepository)
        {
            _apiaryRepository = apiaryRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ApiaryDto>>> GetItems()
        {
            try
            {
                var userId = GetUserId();
                List<Apiary> apiaries = new List<Apiary>();
                if (!string.IsNullOrEmpty(userId))
                {
                    apiaries = await _apiaryRepository.GetItems(userId);
                }

                var apiaryDtos = Apiary2ApiaryDtoTranslator.Translate(apiaries);
                return Ok(apiaryDtos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult<ApiaryDto>> Save(ApiaryDto apiary)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedApiary = await _apiaryRepository.SaveItem(apiary, userId);
                    if (savedApiary != null)
                    {
                        var item = Apiary2ApiaryDtoTranslator.TranslateOne(savedApiary);
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

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<ApiaryDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                Apiary? apiary = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    apiary = await _apiaryRepository.GetItem(id, userId);
                    if (apiary != null)
                    {
                        var item = Apiary2ApiaryDtoTranslator.TranslateOne(apiary);
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

        [HttpDelete]
        [Route("Remove/{id}")]
        public async Task<ActionResult> Remove(long id)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    await _apiaryRepository.Remove(id, userId);
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<ApiaryDto>> Update(ApiaryDto apiary)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _apiaryRepository.EditItem(apiary, userId);
                    if (updatedItem != null)
                    {
                        var item = Apiary2ApiaryDtoTranslator.TranslateOne(updatedItem);
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
    }
}
