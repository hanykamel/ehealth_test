using EHealth.ManageItemLists.Domain.Sub_Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class SubCategoryDbMapping : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.HasOne(k => k.Category);
            builder.HasOne(k => k.ItemListSubtype);
            builder.Property(k => k.SubCategoryAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.SubCategoryEn).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DefinitionAr).HasMaxLength(1500);
            builder.Property(k => k.DefinitionEn).HasMaxLength(1500);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);

        }
    }
}
