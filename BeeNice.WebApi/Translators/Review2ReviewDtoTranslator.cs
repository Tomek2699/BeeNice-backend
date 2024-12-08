using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class Review2ReviewDtoTranslator
    {
        public static List<ReviewDto> Translate(IEnumerable<Review> entities)
        {
            return entities.Select(i => new ReviewDto()
            {
                Id = i.Id,
                ReviewDate = i.ReviewDate,
                FamilyState = i.FamilyState,
                Comment = i.Comment,
                HiveId = i.HiveId,
            }).ToList();
        }

        public static ReviewDto TranslateOne(Review entity)
        {
            return new ReviewDto
            {
                Id = entity.Id,
                ReviewDate = entity.ReviewDate,
                FamilyState = entity.FamilyState,
                Comment = entity.Comment,
                HiveId = entity.HiveId,
            };
        }
    }
}
