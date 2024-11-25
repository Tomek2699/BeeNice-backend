using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class Hive2HiveDtoTranslator
    {
        public static IEnumerable<HiveDto> Translate(IEnumerable<Hive> entities)
        {
            return entities.Select(i => new HiveDto()
            {
                Id = i.Id,
                ApiaryId = i.ApiaryId,
                HiveNumber = i.HiveNumber,
                State = i.State,
                Type = i.Type,
            });
        }

        public static HiveDto TranslateOne(Hive entity)
        {
            return new HiveDto
            {
                Id = entity.Id,
                ApiaryId = entity.ApiaryId,
                HiveNumber = entity.HiveNumber,
                State = entity.State,
                Type = entity.Type,
            };
        }
    }
}
