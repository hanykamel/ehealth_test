using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class DoctorFeesItemPriceDbMapping : IEntityTypeConfiguration<DoctorFeesItemPrice>
    {
        public void Configure(EntityTypeBuilder<DoctorFeesItemPrice> builder)
        {
            builder.ToTable("DoctorFeesItemPrices").HasKey(k => k.Id);
            builder.Property(k => k.DoctorFees).IsRequired().HasPrecision(11, 7);
            builder.HasOne(k => k.UnitOfDoctorFees);
            builder.Property(k => k.EffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
