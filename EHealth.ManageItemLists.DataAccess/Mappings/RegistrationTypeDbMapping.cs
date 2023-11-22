using EHealth.ManageItemLists.Domain.RegistrationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class RegistrationTypeDbMapping : IEntityTypeConfiguration<RegistrationType>
    {
        public void Configure(EntityTypeBuilder<RegistrationType> builder)
        {
            builder.ToTable("RegistrationTypes").HasKey(k => k.Id);
            builder.Property(k => k.RegistrationTypeAr).IsRequired();
            builder.Property(k => k.RegistrationTypeENG).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(k => k.Validator);
        }
    }
}
