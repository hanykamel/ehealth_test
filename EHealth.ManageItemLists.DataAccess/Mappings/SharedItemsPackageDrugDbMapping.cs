using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class SharedItemsPackageDrugDbMapping : IEntityTypeConfiguration<SharedItemsPackageDrug>
    {
        public void Configure(EntityTypeBuilder<SharedItemsPackageDrug> builder)
        {
            builder.ToTable("SharedItemsPackageDrugs").HasKey(k => k.Id);
            builder.HasOne(k => k.SharedItemsPackageComponent);
            builder.HasOne(k => k.DrugUHIA);
            builder.Property(k => k.Quantity).IsRequired();
            builder.Property(k => k.NumberOfCasesInTheUnit).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
