using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;
using BeeNice.WebApi.Repositories.IRepositories;
using BeeNice.WebApi.Translators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/ReviewController")]
    [ApiController]
    [Authorize]
    public class ReviewController : BaseController
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository repository)
        {
            _reviewRepository = repository;
        }

        [HttpGet]
        [Route("GetReviews/{hiveId}")]
        public async Task<ActionResult<IEnumerable<QueenDto>>> GetItems(long hiveId)
        {
            try
            {
                var userId = GetUserId();
                List<ReviewDto> reviewDtos = new List<ReviewDto>();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var reviews = await _reviewRepository.GetItems(hiveId, userId);
                    reviewDtos = Review2ReviewDtoTranslator.Translate(reviews);
                }

                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving queens");
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<ReviewDto>> Get(long id)
        {
            try
            {
                var userId = GetUserId();
                Review? review = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    review = await _reviewRepository.GetItem(id, userId);
                    if (review != null)
                    {
                        var item = Review2ReviewDtoTranslator.TranslateOne(review);
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
        public async Task<ActionResult<ReviewDto>> Save(ReviewDto review)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var savedReview = await _reviewRepository.SaveItem(review, userId);
                    if (savedReview != null)
                    {
                        var item = Review2ReviewDtoTranslator.TranslateOne(savedReview);
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
        public async Task<ActionResult<ReviewDto>> Update(ReviewDto review)
        {
            try
            {
                var userId = GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var updatedItem = await _reviewRepository.EditItem(review, userId);
                    if (updatedItem != null)
                    {
                        var item = Review2ReviewDtoTranslator.TranslateOne(updatedItem);
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
                    await _reviewRepository.Remove(id, userId);
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
