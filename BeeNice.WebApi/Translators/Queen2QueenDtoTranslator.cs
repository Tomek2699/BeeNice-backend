using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class Queen2QueenDtoTranslator
    {
        public static List<QueenDto> Translate(IEnumerable<Queen> entities)
        {
            return entities.Select(i => new QueenDto()
            {
                Id = i.Id,
                HiveId = i.HiveId,
                State = i.State,
                Race = i.Race,
                HatchDate = i.HatchDate,
                QueenNumber = i.QueenNumber,
            }).ToList();
        }

        public static QueenDto TranslateOne(Queen entity)
        {
            return new QueenDto
            {
                Id = entity.Id,
                HiveId = entity.HiveId,
                State = entity.State,
                Race = entity.Race,
                HatchDate = entity.HatchDate,
                QueenNumber = entity.QueenNumber,
            };
        }
    }
}
