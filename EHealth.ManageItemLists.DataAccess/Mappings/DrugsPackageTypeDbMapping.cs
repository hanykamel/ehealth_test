using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class DrugsPackageTypeDbMapping : IEntityTypeConfiguration<DrugsPackageType>
    {
        public void Configure(EntityTypeBuilder<DrugsPackageType> builder)
        {
            builder.ToTable("DrugsPackageTypes").HasKey(k => k.Id);
            builder.Property(k => k.NameAr).IsRequired();
            builder.Property(k => k.NameEN).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(k => k.Validator);
        }
    }
}
