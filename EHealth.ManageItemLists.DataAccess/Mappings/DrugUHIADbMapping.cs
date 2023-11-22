using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class DrugUHIADbMapping : IEntityTypeConfiguration<DrugUHIA>
    {
        public void Configure(EntityTypeBuilder<DrugUHIA> builder)
        {
            builder.ToTable("DrugsUHIA").HasKey(k => k.Id);
            builder.Property(k => k.LocalDrugCode).IsRequired();
            builder.Property(k => k.ProprietaryName).IsRequired();
            builder.Property(k => k.DosageForm).IsRequired();
            builder.HasOne(k => k.SubUnit);
            builder.Property(k => k.SubUnitId).IsRequired();
            builder.HasOne(k => k.MainUnit);
            builder.HasOne(k => k.ReimbursementCategory);
            builder.HasOne(k => k.RegistrationType);
            builder.HasOne(k => k.DrugsPackageType);
            builder.HasOne(k => k.ItemList);
            builder.HasMany(k => k.DrugPrices);

            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
