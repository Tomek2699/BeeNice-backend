﻿using BeeNice.Models.Dtos;
using BeeNice.WebApi.Entities;

namespace BeeNice.WebApi.Translators
{
    public static class BeeFamily2BeeFamilyDtoTranslator
    {
        public static List<BeeFamilyDto> Translate(IEnumerable<BeeFamily> entities)
        {
            return entities.Select(i => new BeeFamilyDto()
            {
                Id = i.Id,
                FamilyNumber = i.FamilyNumber,
                FamilyState = i.FamilyState,
                Race = i.Race,
                CreationDate = i.CreationDate,
                HiveId = i.HiveId,
            }).ToList();
        }

        public static BeeFamilyDto TranslateOne(BeeFamily entity)
        {
            return new BeeFamilyDto
            {
                Id = entity.Id,
                FamilyNumber = entity.FamilyNumber,
                FamilyState = entity.FamilyState,
                Race = entity.Race,
                CreationDate = entity.CreationDate,
                HiveId = entity.HiveId,
            };
        }
    }
}
