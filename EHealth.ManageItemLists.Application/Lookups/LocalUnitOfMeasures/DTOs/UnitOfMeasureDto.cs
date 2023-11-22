using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;

namespace EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs
{
    public class UnitOfMeasureDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? MeasureTypeAr { get; set; }
        public string? MeasureTypeEn { get; set; }
        public string? DefinitionAr { get; set; }
        public string? DefinitionEn { get; set; }
        public bool IsDeleted { get; set; }

        public static UnitOfMeasureDto FromLocalUnitOfMeasure(UnitOfMeasure input) =>
      new UnitOfMeasureDto
      {
          Id = input.Id,
          Code = input.Code,
          MeasureTypeAr = input.MeasureTypeAr,
          MeasureTypeEn = input.MeasureTypeENG,
          DefinitionAr = input.DefinitionAr,
          DefinitionEn = input.DefinitionENG,
          IsDeleted = input.IsDeleted,

      };
    }
}
