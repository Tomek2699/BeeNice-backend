using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class TherapeuticTreatment2TherapeuticTreatmentDtoTranslator
    {
        public static List<TherapeuticTreatmentDto> Translate(IEnumerable<TherapeuticTreatment> entities)
        {
            return entities.Select(i => new TherapeuticTreatmentDto()
            {
                Id = i.Id,
                TreatmentDate = i.TreatmentDate,
                Medicine = i.Medicine,
                Dose = i.Dose,
                Comment = i.Comment,
                HiveId = i.HiveId,
            }).ToList();
        }

        public static TherapeuticTreatmentDto TranslateOne(TherapeuticTreatment entity)
        {
            return new TherapeuticTreatmentDto
            {
                Id = entity.Id,
                TreatmentDate = entity.TreatmentDate,
                Medicine = entity.Medicine,
                Dose = entity.Dose,
                Comment = entity.Comment,
                HiveId = entity.HiveId,
            };
        }
    }
}
