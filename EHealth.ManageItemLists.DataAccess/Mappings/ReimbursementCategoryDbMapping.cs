using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ReimbursementCategoryDbMapping : IEntityTypeConfiguration<ReimbursementCategory>
    {
        public void Configure(EntityTypeBuilder<ReimbursementCategory> builder)
        {
            builder.ToTable("ReimbursementCategories").HasKey(k => k.Id);
            builder.Property(k => k.NameAr).IsRequired();
            builder.Property(k => k.NameENG).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(x => x.Validator);
        }
    }
}
