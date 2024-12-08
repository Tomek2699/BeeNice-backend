using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class Hive2HiveDtoTranslator
    {
        public static List<HiveDto> Translate(IEnumerable<Hive> entities)
        {
            return entities.Select(i => new HiveDto()
            {
                Id = i.Id,
                HiveNumber = i.HiveNumber,
                State = i.State,
                Type = i.Type,
                ApiaryId = i.ApiaryId,
            }).ToList();
        }

        public static HiveDto TranslateOne(Hive entity)
        {
            return new HiveDto
            {
                Id = entity.Id,
                HiveNumber = entity.HiveNumber,
                State = entity.State,
                Type = entity.Type,
                ApiaryId = entity.ApiaryId,
            };
        }
    }
}
