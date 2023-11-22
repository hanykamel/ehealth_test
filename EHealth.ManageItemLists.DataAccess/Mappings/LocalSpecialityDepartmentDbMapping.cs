using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class LocalSpecialityDepartmentDbMapping : IEntityTypeConfiguration<LocalSpecialtyDepartment>
    {
        public void Configure(EntityTypeBuilder<LocalSpecialtyDepartment> builder)
        {
           builder.ToTable("LocalSpecialtyDepartments").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.LocalSpecialityAr).IsRequired();
            builder.Property(k => k.LocalSpecialityENG).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(k => k.Validator);
        }
    }
}
