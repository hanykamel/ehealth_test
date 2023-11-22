using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Facility.UHIA
{
    public class FacilityUHIA : EHealthDomainObject, IEntity<Guid>, IValidationModel<FacilityUHIA>
    {
        private FacilityUHIA()
        {

        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string? DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public double? OccupancyRate { get; private set; }
        public double OperatingRateInHoursPerDay { get; private set; }
        public double OperatingDaysPerMonth { get; private set; }
        public int ItemListId { get; set; }
        public ItemList ItemList { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int SubCategoryId { get; private set; }
        public SubCategory SubCategory { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }

        public void SetCode(string input)
        {
            if (Code == input) return;
            Code = input;
        }
        public void SetDescriptorAr(string? input)
        {
            if (DescriptorAr == input) return;
            DescriptorAr = input;
        }
        public void SetDescriptorEn(string input)
        {
            if (DescriptorEn == input) return;
            DescriptorEn = input;
        }
        public void SetOccupancyRate(double? input)
        {
            if (OccupancyRate == input) return;
            OccupancyRate = input;
        }
        public void SetOperatingRateInHoursPerDay(double input)
        {
            if (OperatingRateInHoursPerDay == input) return;
            OperatingRateInHoursPerDay = input;
        }

        public void SetOperatingDaysPerMonth(double input)
        {
            if (OperatingDaysPerMonth == input) return;
            OperatingDaysPerMonth = input;
        }

        public void SetCategoryId(int input)
        {
            if (CategoryId == input) return;
            CategoryId = input;
        }
        public void SetSubCategoryId(int input)
        {
            if (SubCategoryId == input) return;
            SubCategoryId = input;
        }
        public void SetDataEffectiveDateFrom(DateTime input)
        {
            if (DataEffectiveDateFrom == input) return;
            DataEffectiveDateFrom = input;
        }
        public void SetDataEffectiveDateTo(DateTime? input)
        {
            if (DataEffectiveDateTo == input) return;
            DataEffectiveDateTo = input;
        }

        public AbstractValidator<FacilityUHIA> Validator => new FacilityUHIAValidator();

        public async Task<Guid> Create(IFacilityUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }
        public async Task<bool> Update(IFacilityUHIARepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IFacilityUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<FacilityUHIA>> Search(IFacilityUHIARepository repository, Expression<Func<FacilityUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }

        public static async Task<FacilityUHIA> Get(Guid id, IFacilityUHIARepository repository)
        {
            var dbFacilityUHIA = await repository.Get(id);

            if (dbFacilityUHIA is null)
            {
                throw new DataNotFoundException();
            }

            return dbFacilityUHIA;
        }

        public static FacilityUHIA Create(Guid? id, string code, string? descriptorAr, string descriptorEn, double? occupancyRate, double operatingRateInHoursPerDay,
           double operatingDaysPerMonth, int categoryId, int subCategoryId, DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo, string createdBy, string tenantId, int itemListId)
        {
            return new FacilityUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                Code = code,
                DescriptorAr = descriptorAr,
                DescriptorEn = descriptorEn,
                OccupancyRate = occupancyRate,
                OperatingRateInHoursPerDay = operatingRateInHoursPerDay,
                OperatingDaysPerMonth = operatingDaysPerMonth,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                DataEffectiveDateFrom = dataEffectiveDateFrom.ToUniversalTime(),
                DataEffectiveDateTo = dataEffectiveDateTo?.ToUniversalTime(),
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                TenantId = tenantId,
                ItemListId = itemListId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IFacilityUHIARepository repository, bool throwException = true)
        {
            var dbFacilityUHIA = await repository.Search(x => (x.Id == Id || x.Code == Code
            || (x.DescriptorAr == DescriptorAr && !string.IsNullOrEmpty(x.DescriptorAr)) || x.DescriptorEn == DescriptorEn)
            && x.IsDeleted!=true, 1, 1, true, null, null);

            string dublicatedProperties = "";
            if (Id == default)
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
                if (dbFacilityUHIA.Data.Any())
                {
                    if (dbFacilityUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "DescriptorAr is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "DescriptorAr is Duplicated",

                        });
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "DescriptorEn is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "DescriptorEn is Duplicated",

                        });
                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            else
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();

                if (dbFacilityUHIA.Data.Any(x => x.Id != Id))
                {
                    dbFacilityUHIA.Data = dbFacilityUHIA.Data.Where(x => x.Id != Id).ToList();

                    if (dbFacilityUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "DescriptorAr is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "DescriptorAr is Duplicated",

                        });
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "DescriptorEn is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "DescriptorEn is Duplicated",

                        });
                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            return true;
        }

        public static async Task IsItemListBusy(IFacilityUHIARepository repository, int itemListId, bool throwException = true)
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
        public async Task<string> ValidateObjectForBulkUpload(IFacilityUHIARepository repository, IValidationEngine validationEngine)
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
        private async Task<string?> GetdDuplicatesErrors(IFacilityUHIARepository repository)
        {
            var dbFacilityUHIA = await repository.Search(x => (x.Id == Id || x.Code == Code
            || (x.DescriptorAr == DescriptorAr && !string.IsNullOrEmpty(x.DescriptorAr)) || x.DescriptorEn == DescriptorEn)
            && x.IsDeleted != true, 1, 1, true, null, null);

            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbFacilityUHIA.Data.Any())
                {
                    if (dbFacilityUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbFacilityUHIA.Data.Any(x => x.Id != Id))
                {
                    dbFacilityUHIA.Data = dbFacilityUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbFacilityUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }

                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbFacilityUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            return null;
        }
    }
}
