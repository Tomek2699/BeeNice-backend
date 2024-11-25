using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class Apiary2ApiaryDtoTranslator
    {
        public static IEnumerable<ApiaryDto> Translate(IEnumerable<Apiary> entities)
        {
            return entities.Select(i => new ApiaryDto()
            {
                Id = i.Id,
                Name = i.Name,
                CreationDate = i.CreationDate,
                Location = i.Location,
            });
        }

        public static ApiaryDto TranslateOne(Apiary entity)
        {
            return new ApiaryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreationDate = entity.CreationDate,
                Location = entity.Location,
            };
        }
    }
}
