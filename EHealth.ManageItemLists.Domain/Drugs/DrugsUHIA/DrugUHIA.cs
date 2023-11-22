using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA
{
    public class DrugUHIA : EHealthDomainObject, IEntity<Guid>, IValidationModel<DrugUHIA>
    {
        private DrugUHIA()
        {

        }
        public Guid Id { get; private set; }
        public string? EHealthDrugCode { get; private set; }
        public string? LocalDrugCode { get; private set; }
        public string? InternationalNonProprietaryName { get; private set; }
        public string ProprietaryName { get; private set; }
        public string DosageForm { get; private set; }
        public string? RouteOfAdministration { get; private set; }
        public string? Manufacturer { get; private set; }
        public string? MarketAuthorizationHolder { get; private set; }
        public int? RegistrationTypeId { get; private set; }
        public RegistrationType? RegistrationType { get; private set; }
        public int? DrugsPackageTypeId { get; private set; }
        public DrugsPackageType? DrugsPackageType { get; private set; }
        public int? MainUnitId { get; private set; }
        public UnitsType? MainUnit { get; private set; }
        public int? NumberOfMainUnit { get; private set; }
        public int? SubUnitId { get; private set; }
        public UnitsType? SubUnit { get; private set; }
        public int? NumberOfSubunitPerMainUnit { get; private set; }
        public int? TotalNumberSubunitsOfPack { get; private set; }
        public int? ReimbursementCategoryId { get; private set; }
        public ReimbursementCategory? ReimbursementCategory { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<DrugPrice> DrugPrices { get; private set; } = new List<DrugPrice>();
        public int ItemListId { get; private set; }
        public ItemList ItemList { get; private set; }

        public AbstractValidator<DrugUHIA> Validator => new DrugUHIAValidator();
        public async Task<Guid> Create(IDrugsUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IDrugsUHIARepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IDrugsUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);

        }

        public static async Task<PagedResponse<DrugUHIA>> Search(IDrugsUHIARepository repository, Expression<Func<DrugUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }

        public async Task AddDrugPrices(IDrugsUHIARepository repository, IValidationEngine validationEngine, List<DrugPrice> DrugPrices)
        {
            foreach (var price in DrugPrices)
            {
                validationEngine.Validate(price);
                this.DrugPrices.Add(price);
            }
            await repository.Update(this);
        }

        public void DeleteDrugPrices(List<DrugPrice> DrugPrices, string deletedBy)
        {
            foreach (var price in DrugPrices)
            {
                price.SoftDelete(deletedBy);
            }
        }

        public static async Task<DrugUHIA> Get(Guid id, IDrugsUHIARepository repository)
        {
            var dbDrugUHIA = await repository.Get(id);

            if (dbDrugUHIA is null)
            {
                throw new DataNotFoundException();
            }

            return dbDrugUHIA;
        }

        public static DrugUHIA Create(Guid? id, int itemListId, string? eHealthDrugCode, string? localDrugCode, string? internationalNonProprietaryName, string proprietaryName, string dosageForm,
          string? routeOfAdministration, string? manufacturer, string? marketAuthorizationHolder, int? registrationTypeId, int? drugsPackageTypeId, int? mainUnitId,
          int? numberOfMainUnit, int? subUnitId, int? numberOfSubunitPerMainUnit, int? totalNumberSubunitsOfPack, int? reimbursementCategoryId, DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo, string createdBy, string tenantId)
        {
            return new DrugUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                ItemListId = itemListId,
                EHealthDrugCode = eHealthDrugCode,
                LocalDrugCode = localDrugCode,
                InternationalNonProprietaryName = internationalNonProprietaryName,
                ProprietaryName = proprietaryName,
                DosageForm = dosageForm,
                RouteOfAdministration = routeOfAdministration,
                Manufacturer = manufacturer,
                MarketAuthorizationHolder = marketAuthorizationHolder,
                RegistrationTypeId = registrationTypeId,
                DrugsPackageTypeId = drugsPackageTypeId,
                MainUnitId = mainUnitId,
                NumberOfMainUnit = numberOfMainUnit,
                SubUnitId = subUnitId,
                NumberOfSubunitPerMainUnit = numberOfSubunitPerMainUnit,
                TotalNumberSubunitsOfPack = totalNumberSubunitsOfPack,
                ReimbursementCategoryId = reimbursementCategoryId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTimeOffset.Now,
                TenantId = tenantId
            };
        }

        public void SetEHealthDrugCode(string? eHealthDrugCode)
        {
            if (EHealthDrugCode == eHealthDrugCode) return;
            EHealthDrugCode = eHealthDrugCode;
        }
        public void SetLocalDrugCode(string? localDrugCode)
        {
            if (LocalDrugCode == localDrugCode) return;
            LocalDrugCode = localDrugCode;
        }
        public void SetInternationalNonProprietaryName(string? internationalNonProprietaryName)
        {
            if (InternationalNonProprietaryName == internationalNonProprietaryName) return;
            InternationalNonProprietaryName = internationalNonProprietaryName;
        }
        public void SetProprietaryName(string proprietaryName)
        {
            if (ProprietaryName == proprietaryName) return;
            ProprietaryName = proprietaryName;
        }
        public void SetDosageForm(string dosageForm)
        {
            if (DosageForm == dosageForm) return;
            DosageForm = dosageForm;
        }
        public void SetRouteOfAdministration(string? routeOfAdministration)
        {
            if (RouteOfAdministration == routeOfAdministration) return;
            RouteOfAdministration = routeOfAdministration;
        }
        public void SetManufacturer(string? manufacturer)
        {
            if (Manufacturer == manufacturer) return;
            Manufacturer = manufacturer;
        }
        public void SetMarketAuthorizationHolder(string? marketAuthorizationHolder)
        {
            if (MarketAuthorizationHolder == marketAuthorizationHolder) return;
            MarketAuthorizationHolder = marketAuthorizationHolder;
        }
        public void SetRegistrationTypeId(int? registrationTypeId)
        {
            if (RegistrationTypeId == registrationTypeId) return;
            RegistrationTypeId = registrationTypeId;
        }
        public void SetDrugsPackageTypeId(int? drugsPackageTypeId)
        {
            if (DrugsPackageTypeId == drugsPackageTypeId) return;
            DrugsPackageTypeId = drugsPackageTypeId;
        }
        public void SetMainUnitId(int? mainUnitId)
        {
            if (MainUnitId == mainUnitId) return;
            MainUnitId = mainUnitId;
        }
        public void SetNumberOfMainUnit(int? numberOfMainUnit)
        {
            if (NumberOfMainUnit == numberOfMainUnit) return;
            NumberOfMainUnit = numberOfMainUnit;
        }
        public void SetSubUnitId(int? subUnitId)
        {
            if (SubUnitId == subUnitId) return;
            SubUnitId = subUnitId;
        }
        public void SetNumberOfSubunitPerMainUnit(int? numberOfSubunitPerMainUnit)
        {
            if (NumberOfSubunitPerMainUnit == numberOfSubunitPerMainUnit) return;
            NumberOfSubunitPerMainUnit = numberOfSubunitPerMainUnit;
        }
        public void SetTotalNumberSubunitsOfPack(int? totalNumberSubunitsOfPack)
        {
            if (TotalNumberSubunitsOfPack == totalNumberSubunitsOfPack) return;
            TotalNumberSubunitsOfPack = totalNumberSubunitsOfPack;
        }
        public void SetReimbursementCategoryId(int? reimbursementCategoryId)
        {
            if (ReimbursementCategoryId == reimbursementCategoryId) return;
            ReimbursementCategoryId = reimbursementCategoryId;
        }
        public void SetDataEffectiveDateFrom(DateTime dataEffectiveDateFrom)
        {
            if (DataEffectiveDateFrom == dataEffectiveDateFrom) return;
            DataEffectiveDateFrom = dataEffectiveDateFrom;
        }
        public void SetDataEffectiveDateTo(DateTime? dataEffectiveDateTo)
        {
            if (DataEffectiveDateTo == dataEffectiveDateTo) return;
            DataEffectiveDateTo = dataEffectiveDateTo;
        }
        private async Task<bool> EnsureNoDuplicates(IDrugsUHIARepository repository, bool throwException = true)
        {
            var dbDrugUHIA = await repository.Search(x => (x.Id == Id || x.LocalDrugCode == LocalDrugCode || x.EHealthDrugCode == EHealthDrugCode)
                                                                                                 && x.IsDeleted != true, 1, 1, false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbDrugUHIA.Data.Any())
                {
                    List<ValidationFailure>? errors = new List<ValidationFailure>();
                    if (dbDrugUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });
                    }
                    if (dbDrugUHIA.Data.Where(x => x.EHealthDrugCode == EHealthDrugCode).Any())
                    {
                        dublicatedProperties += "EHealthDrugCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }

                    if (dbDrugUHIA.Data.Where(x => x.LocalDrugCode == LocalDrugCode).Any())
                    {
                        dublicatedProperties += "LocalDrugCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "LocalDrugCode is Duplicated",

                        });
                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);

                }
            }
            else
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();

                if (dbDrugUHIA.Data.Any(x => x.Id != Id))
                {
                    dbDrugUHIA.Data = dbDrugUHIA.Data.Where(x => x.Id != Id).ToList();

                    if (dbDrugUHIA.Data.Where(x => x.EHealthDrugCode == EHealthDrugCode).Any())
                    {
                        dublicatedProperties += "EHealthDrugCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }

                    if (dbDrugUHIA.Data.Where(x => x.LocalDrugCode == LocalDrugCode).Any())
                    {
                        dublicatedProperties += "LocalDrugCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "LocalDrugCode is Duplicated",

                        });
                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            return true;
        }

        public static async Task IsItemListBusy(IDrugsUHIARepository repository, int itemListId, bool throwException = true)
        {
            bool isBusy = (await repository.IsItemLIstBusy(itemListId));

            if (isBusy)
            {
                string message = "The data not valid";
                List<ValidationFailure> errors = new List<ValidationFailure>();
                errors.Add(new ValidationFailure
                {
                    ErrorCode = "ItemManagement_MSG_06",
                    ErrorMessage = "This Record is locked by another user.",
                });
                throw new DataNotValidException(message, errors);
            }
        }
        public async Task<string> ValidateObjectForBulkUpload(IDrugsUHIARepository repository, IValidationEngine validationEngine)
        {
            var errorsList = "";
            var errors = validationEngine.Validate(this, false);
            var duplicateErrors = await GetdDuplicatesErrors(repository);

            if (errors != null)
            {
                foreach (var item in errors)
                {
                    errorsList += item.ErrorMessage + "\r\n";
                }
            }

            if (duplicateErrors != null)
            {
                errorsList += duplicateErrors + "\r\n";
            }
            return errorsList;
        }
        private async Task<string?> GetdDuplicatesErrors(IDrugsUHIARepository repository)
        {
            var dbDrugUHIA = await repository.Search(x => (x.Id == Id || x.LocalDrugCode == LocalDrugCode || x.EHealthDrugCode == EHealthDrugCode)
                                                                                                 && x.IsDeleted != true, 1, 1, false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbDrugUHIA.Data.Any())
                {
                    if (dbDrugUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbDrugUHIA.Data.Where(x => x.EHealthDrugCode == EHealthDrugCode).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthDrugCode,";
                    }
                    if (dbDrugUHIA.Data.Where(x => x.LocalDrugCode == LocalDrugCode).Any())
                    {
                        dublicatedProperties += "Duplicated LocalDrugCode,";
                    }

                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbDrugUHIA.Data.Any(x => x.Id != Id))
                {
                    dbDrugUHIA.Data = dbDrugUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbDrugUHIA.Data.Where(x => x.EHealthDrugCode == EHealthDrugCode).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthDrugCode,";
                    }
                    if (dbDrugUHIA.Data.Where(x => x.LocalDrugCode == LocalDrugCode).Any())
                    {
                        dublicatedProperties += "Duplicated LocalDrugCode,";
                    }

                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            return null;
        }
    }
}
