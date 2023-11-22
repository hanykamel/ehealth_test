using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class FacilityUHIADbMapping : IEntityTypeConfiguration<FacilityUHIA>
    {
        public void Configure(EntityTypeBuilder<FacilityUHIA> builder)
        {
            builder.ToTable("FacilityUHIA").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.DescriptorAr).HasMaxLength(100);
            builder.Property(k => k.DescriptorEn).IsRequired().HasMaxLength(100);
            builder.Property(k => k.OccupancyRate).HasPrecision(11, 7);
            builder.Property(k => k.OperatingRateInHoursPerDay).IsRequired().HasPrecision(11,7);
            builder.Property(k => k.OperatingDaysPerMonth).IsRequired().HasPrecision(11,7);
            builder.HasOne(k => k.ItemList);
            builder.HasOne(k => k.SubCategory);
            builder.HasOne(k => k.Category);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
