using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ProceduresICHIDbMapping : IEntityTypeConfiguration<ProcedureICHI>
    {
        public void Configure(EntityTypeBuilder<ProcedureICHI> builder)
        {
            builder.ToTable("ProceduresICHI").HasKey(k => k.Id);
            builder.Property(k => k.EHealthCode).IsRequired();
            builder.Property(k => k.UHIAId).IsRequired();
            builder.Property(k => k.TitleEn).IsRequired();
            builder.HasOne(k => k.ServiceCategory);
            builder.HasOne(k => k.SubCategory);
            builder.HasOne(k => k.LocalSpecialtyDepartment);
            builder.HasOne(k => k.ItemList);
            builder.HasMany(k => k.ItemListPrices);
            builder.Property(k => k.DataEffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
