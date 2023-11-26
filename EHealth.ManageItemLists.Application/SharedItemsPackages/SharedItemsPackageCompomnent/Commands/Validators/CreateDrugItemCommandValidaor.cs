using DocumentFormat.OpenXml.InkML;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Locations;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.Validators
{
    public class CreateDrugItemCommandValidaor : AbstractValidator<CreateDrugItemCommand>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly ILocationsRepository _locationsRepository;

        public CreateDrugItemCommandValidaor(IPackageHeaderRepository packageHeaderRepository,
            IDrugsUHIARepository drugsUHIARepository,
            ILocationsRepository  locationsRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
            _drugsUHIARepository = drugsUHIARepository;
            _locationsRepository = locationsRepository;

            RuleFor(x => x.PackageHeaderId).MustAsync(async (PackageHeaderId, CancellationToken) =>
            {
                try
                {
                    var packageHeader = await PackageHeader.Get(PackageHeaderId, _packageHeaderRepository);
                    if (packageHeader is not null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("PackageHeaderNotExist").WithMessage("PackageHeader with PackageHeaderId not exist.")
                .When(x => !string.IsNullOrEmpty(x.PackageHeaderId.ToString()));

            RuleFor(x => x.DrugUHIAId).MustAsync(async (DrugUHIAId, CancellationToken) =>
            {

                try
                {
                    var drugUHIA = await DrugUHIA.Get(DrugUHIAId, _drugsUHIARepository);
                    if (drugUHIA is not null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("DrugUHIANotExist").WithMessage("DrugUHIA with DrugUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.DrugUHIAId.ToString()));

            RuleFor(x => x.LocationId).MustAsync(async (LocationId, CancellationToken) =>
            {

                try
                {
                    var location = await Location.Get(LocationId.Value, _locationsRepository);
                    if (location is not null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("LocationNotExist").WithMessage("Location with LocationId not exist.")
                .When(x => !string.IsNullOrEmpty(x.LocationId.ToString()));

            RuleFor(x => x.Quantity).NotNull().NotEmpty();
            RuleFor(x => x.NumberOfCasesInTheUnit).NotNull().NotEmpty();
            RuleFor(x => x.TotalCost).MustAsync(async (context, totalCost, CancellationToken) =>
            {
                try
                {
                    var drugUHIA = await DrugUHIA.Get(context.DrugUHIAId, _drugsUHIARepository);
                    var latestPrice = drugUHIA.DrugPrices?.OrderByDescending(x => x.EffectiveDateFrom).FirstOrDefault()?.SubUnitPrice;
                    var expectedTotalCost = context.Quantity * latestPrice;
                    return totalCost == expectedTotalCost;
                }
                catch (Exception)
                {
                    return false;
                }
            }).WithMessage("TotalCost should be equal to Quantity * SubUnitPrice.");
            RuleFor(x => x.DrugPerCase).MustAsync(async (context, drugPerCase, CancellationToken) =>
            {
                try
                {
                    return drugPerCase == context.TotalCost / context.NumberOfCasesInTheUnit;
                }
                catch (Exception)
                {
                    return false;
                }
            }).WithMessage("DrugPerCase should be equal to TotalCost / NumberOfCasesInTheUnit.");
        }
    
    }
}