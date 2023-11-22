using EHealth.ManageItemLists.Domain.PriceUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class PriceUnitDbMapping : IEntityTypeConfiguration<PriceUnit>
    {
        public void Configure(EntityTypeBuilder<PriceUnit> builder)
        {
            builder.ToTable("PriceUnits").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.NameAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.NameEN).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DefinitionAr).HasMaxLength(1500);
            builder.Property(k => k.DefinitionEN).HasMaxLength(1500);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
