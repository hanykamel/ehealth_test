using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class DoctorFeesUHIADbMapping : IEntityTypeConfiguration<DoctorFeesUHIA>
    {
        public void Configure(EntityTypeBuilder<DoctorFeesUHIA> builder)
        {
            builder.ToTable("DoctorFeesUHIA").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.DescriptorEn).IsRequired();
            builder.HasOne(k => k.ItemList);
            builder.HasOne(k => k.PackageComplexityClassification);
            builder.HasMany(k => k.ItemListPrices);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
