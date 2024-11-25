using BeeNice.Models.Dtos;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/QueenController")]
    [ApiController]
    public class QueenController : ControllerBase
    {
        private readonly IQueenRepository _repository;

        public QueenController(IQueenRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetQueens/{beeFamilyId}")]
        public async Task<ActionResult<IEnumerable<QueenDto>>> GetItems(long beeFamilyId)
        {
            try
            {
                var queens = await _repository.GetItems(beeFamilyId);
                if (!queens.Any())
                {
                    return NotFound();
                }

                var queenDtos = Queen2QueenDtoTranslator.Translate(queens);
                return Ok(queenDtos);
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
                var savedQueen = await _repository.SaveItem(queen);
                if (savedQueen != null)
                {
                    var item = Queen2QueenDtoTranslator.TranslateOne(savedQueen);
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
        public async Task<ActionResult<QueenDto>> Get(long id)
        {
            try
            {
                var queen = await _repository.GetItem(id);
                if (queen.Value == null)
                {
                    return NotFound(StatusCodes.Status404NotFound);
                }

                return Ok(queen);
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
        public async Task<ActionResult<QueenDto>> Update(QueenDto queen)
        {
            try
            {
                var updatedItem = await _repository.EditItem(queen);
                if (updatedItem != null)
                {
                    var item = Queen2QueenDtoTranslator.TranslateOne(updatedItem);
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
