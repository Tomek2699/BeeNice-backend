using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/QueenController")]
    [ApiController]
    [Authorize]
    public class QueenController : BaseController
    {
        private readonly IQueenRepository _queenRepository;

        public QueenController(IQueenRepository repository)
        {
            _queenRepository = repository;
        }

        [HttpGet]
        [Route("GetQueens/{hievId}")]
        public async Task<ActionResult<IEnumerable<QueenDto>>> GetItems(long hiveId)
        {
            try
            {
                var userId = GetUserId();
                List<QueenDto> queenDtos = new List<QueenDto>();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var queens = await _queenRepository.GetItems(hiveId, userId);
                    queenDtos = Queen2QueenDtoTranslator.Translate(queens);
                }

                return Ok(queenDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving queens");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<QueenDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                Queen? queen = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    queen = await _queenRepository.GetItem(id, userId);
                    if (queen != null)
                    {
                        var item = Queen2QueenDtoTranslator.TranslateOne(queen);
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
        public async Task<ActionResult<QueenDto>> Save(QueenDto queen)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedQueen = await _queenRepository.SaveItem(queen, userId);
                    if (savedQueen != null)
                    {
                        var item = Queen2QueenDtoTranslator.TranslateOne(savedQueen);
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
        public async Task<ActionResult<QueenDto>> Update(QueenDto queen)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _queenRepository.EditItem(queen, userId);
                    if (updatedItem != null)
                    {
                        var item = Queen2QueenDtoTranslator.TranslateOne(updatedItem);
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
                    await _queenRepository.Remove(id, userId);
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
