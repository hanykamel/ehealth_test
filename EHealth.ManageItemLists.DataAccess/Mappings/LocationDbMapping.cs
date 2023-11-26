using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class LocationDbMapping : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.LocationAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.LocationEn).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DefinitionAr).HasMaxLength(1500);
            builder.Property(k => k.DefinitionEn).HasMaxLength(1500);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
