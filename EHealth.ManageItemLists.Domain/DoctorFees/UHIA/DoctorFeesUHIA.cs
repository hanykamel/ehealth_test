using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.DoctorFees.UHIA
{
    public class DoctorFeesUHIA : EHealthDomainObject, IEntity<Guid>, IValidationModel<DoctorFeesUHIA>
    {
        private DoctorFeesUHIA()
        {

        }
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string? DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public int ItemListId { get; set; }
        public ItemList ItemList { get; private set; }
        public int PackageComplexityClassificationId { get; private set; }
        public PackageComplexityClassification PackageComplexityClassification { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<DoctorFeesItemPrice> ItemListPrices { get; private set; } = new List<DoctorFeesItemPrice>();
        public AbstractValidator<DoctorFeesUHIA> Validator => new DoctorFeesUHIAValidator();
        public void SetEHealthCode(string etEHealthCode)
        {
            if (Code == etEHealthCode) return;
            Code = etEHealthCode;
        }
        public void SetDescriptorAr(string? descriptorAr)
        {
            if (DescriptorAr == descriptorAr) return;
            DescriptorAr = descriptorAr;
        }
        public void SetDescriptorEn(string descriptorEn)
        {
            if (DescriptorEn == descriptorEn) return;
            DescriptorEn = descriptorEn;
        }
        public void SetPackageComplexityClassificationId(int packageComplexityClassificationId)
        {
            if (PackageComplexityClassificationId == packageComplexityClassificationId) return;
            PackageComplexityClassificationId = packageComplexityClassificationId;
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
        public async Task<Guid> Create(IDoctorFeesUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }
        public async Task<bool> Update(IDoctorFeesUHIARepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IDoctorFeesUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);

        }

        public static async Task<PagedResponse<DoctorFeesUHIA>> Search(IDoctorFeesUHIARepository repository, Expression<Func<DoctorFeesUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }
        public static async Task<DoctorFeesUHIA> Get(Guid id, IDoctorFeesUHIARepository repository)
        {
            var dbDoctorFeesUHIA = await repository.Get(id);

            if (dbDoctorFeesUHIA is null)
            {
                throw new DataNotFoundException();
            }

            return dbDoctorFeesUHIA;
        }
        public static DoctorFeesUHIA Create(Guid? id ,string code, string? descriptorAr, string descriptorEn, int itemListId, int packageComplexityClassificationId,
             DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo, string createdBy, string tenantId)
        {
            return new DoctorFeesUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                Code = code,
                DescriptorAr = descriptorAr,
                DescriptorEn = descriptorEn,
                ItemListId = itemListId,
                PackageComplexityClassificationId = packageComplexityClassificationId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                TenantId = tenantId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IDoctorFeesUHIARepository repository, bool throwException = true)
        {
            var dbDoctorFeesUHIA = await repository.Search(x =>( x.Id == Id || x.Code == Code ||
                                                        (x.DescriptorAr == DescriptorAr && (!string.IsNullOrEmpty(x.DescriptorAr))) ||
                                                        x.DescriptorEn == DescriptorEn)&& x.IsDeleted != true, 1, 1, false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
                if (dbDoctorFeesUHIA.Data.Any())
                {
                    if (dbDoctorFeesUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });
                    
                    }
                    if (dbDoctorFeesUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });

                    }
                    if (dbDoctorFeesUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });
                    }
                    if (dbDoctorFeesUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "ShortDescEn,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "ShortDescEn is Duplicated",

                        });

                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            else
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
               
                    if (dbDoctorFeesUHIA.Data.Any(x => x.Id != Id))
                    {
                        dbDoctorFeesUHIA.Data = dbDoctorFeesUHIA.Data.Where(x => x.Id != Id).ToList();

                        if (dbDoctorFeesUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });

                    }
                    if (dbDoctorFeesUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });
                    }
                    if (dbDoctorFeesUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "ShortDescEn,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "ShortDescEn is Duplicated",

                        });

                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            return true;
        }

        public static async Task IsItemListBusy(IDoctorFeesUHIARepository repository, int itemListId, bool throwException = true)
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
        public async Task<string> ValidateObjectForBulkUpload(IDoctorFeesUHIARepository repository, IValidationEngine validationEngine)
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

        private async Task<string?> GetdDuplicatesErrors(IDoctorFeesUHIARepository repository)
        {
            var dbDevAndAssUHIA = await repository.Search(x => (x.Id == Id || x.Code == Code ||
                                                        (x.DescriptorAr == DescriptorAr && (!string.IsNullOrEmpty(x.DescriptorAr))) ||
                                                        (x.DescriptorEn == DescriptorEn && (!string.IsNullOrEmpty(x.DescriptorEn))))
                                                        && x.IsDeleted != true, 1, 1, false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbDevAndAssUHIA.Data.Any())
                {
                    if (dbDevAndAssUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbDevAndAssUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbDevAndAssUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbDevAndAssUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbDevAndAssUHIA.Data.Any(x => x.Id != Id))
                {
                    dbDevAndAssUHIA.Data = dbDevAndAssUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbDevAndAssUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbDevAndAssUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbDevAndAssUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
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
