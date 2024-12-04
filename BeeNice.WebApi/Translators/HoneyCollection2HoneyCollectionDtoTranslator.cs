using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class HoneyCollection2HoneyCollectionDtoTranslator
    {
        public static List<HoneyCollectionDto> Translate(IEnumerable<HoneyCollection> entities)
        {
            return entities.Select(i => new HoneyCollectionDto()
            {
                Id = i.Id,
                HiveId = i.HiveId,
                CollectionDate = i.CollectionDate,
                TypeOfHoney = i.TypeOfHoney,
                HoneyQuantity = i.HoneyQuantity
            }).ToList();
        }

        public static HoneyCollectionDto TranslateOne(HoneyCollection entity)
        {
            return new HoneyCollectionDto
            {
                Id = entity.Id,
                HiveId = entity.HiveId,
                CollectionDate = entity.CollectionDate,
                TypeOfHoney = entity.TypeOfHoney,
                HoneyQuantity = entity.HoneyQuantity
            };
        }
    }
}
