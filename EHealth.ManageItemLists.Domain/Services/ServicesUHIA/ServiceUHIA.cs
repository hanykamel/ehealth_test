using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Services.ServicesUHIA
{
    public class ServiceUHIA : EHealthDomainObject, IEntity<Guid>, IValidationModel<ServiceUHIA>
    {
        private ServiceUHIA()
        {

        }
        public Guid Id { get; private set; }
        public string EHealthCode { get; private set; }
        public string UHIAId { get; private set; }
        public string? ShortDescAr { get; private set; }
        public string? ShortDescEn { get; private set; }

        public int ServiceCategoryId { get; private set; }
        public Category ServiceCategory { get; private set; }
        public int ServiceSubCategoryId { get; private set; }
        public SubCategory ServiceSubCategory { get; private set; }
        public int ItemListId { get; private set; }
        public ItemList ItemList { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<ItemListPrice> ItemListPrices { get; private set; } = new List<ItemListPrice>();
        public AbstractValidator<ServiceUHIA> Validator => new ServiceUHIAValidator();

        public void SetEHealthCode(string etEHealthCode)
        {
            if (EHealthCode == etEHealthCode) return;
            EHealthCode = etEHealthCode;
        }
        public void SetUHIAId(string uHIAId)
        {
            if (UHIAId == uHIAId) return;
            UHIAId = uHIAId;
        }
        public void SetShortDescAr(string? shortDescAr)
        {
            if (ShortDescAr == shortDescAr) return;
            ShortDescAr = shortDescAr;
        }
        public void SetShortDescEn(string? shortDescEn)
        {
            if (ShortDescEn == shortDescEn) return;
            ShortDescEn = shortDescEn;
        }
        public void SetServiceCategoryId(int serviceCategoryId)
        {
            if (ServiceCategoryId == serviceCategoryId) return;
            ServiceCategoryId = serviceCategoryId;
        }
        public void SetServiceSubCategoryId(int serviceSubCategoryId)
        {
            if (ServiceSubCategoryId == serviceSubCategoryId) return;
            ServiceSubCategoryId = serviceSubCategoryId;
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

        public async Task<Guid> Create(IServiceUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IServiceUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IServiceUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);

        }

        public static async Task<PagedResponse<ServiceUHIA>> Search(IServiceUHIARepository repository, Expression<Func<ServiceUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize,enablePagination, orderBy, ascending);
        }

        public static async Task<ServiceUHIA> Get(Guid id, IServiceUHIARepository repository)
        {
            var dbServiceUHIA = await repository.Get(id);

            if (dbServiceUHIA is null)
            {
                throw new DataNotFoundException();
            }

            return dbServiceUHIA;
        }

        public static ServiceUHIA Create(Guid? id, string eHealthCode, string UHIAId, string? shortDescAr, string? shortDescEn, int serviceCategoryId,
            int serviceSubCategoryId, int itemListId, DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo, string createdBy, string tenantId)
        {
            return new ServiceUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                EHealthCode = eHealthCode,
                UHIAId = UHIAId,
                ShortDescAr = shortDescAr,
                ShortDescEn = shortDescEn,
                ServiceCategoryId = serviceCategoryId,
                ServiceSubCategoryId = serviceSubCategoryId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                CreatedBy = createdBy,
                ItemListId = itemListId,
                CreatedOn = DateTimeOffset.Now,
                TenantId = tenantId
            };
        }

        public async Task<string> ValidateObjectForBulkUpload(IServiceUHIARepository repository, IValidationEngine validationEngine)
        {
            var errorsList = "";
            var errors = validationEngine.Validate(this, false);
            var duplicateErrors = await GetdDuplicatesErrors(repository);

            if(errors != null)
            {
                foreach (var item in errors)
                {
                    errorsList += item.ErrorMessage + "\r\n";
                }
            }
           
            if(duplicateErrors != null)
            {
                errorsList += duplicateErrors + "\r\n";
            }
            return errorsList;
        }
        public static async Task IsItemListBusy(IServiceUHIARepository repository, int itemListId, bool throwException = true)
        {
            bool isBusy =  (await repository.IsItemLIstBusy(itemListId));

            if(isBusy)
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

        private async Task<bool> EnsureNoDuplicates(IServiceUHIARepository repository, bool throwException = true)
        {
            var dbServiceUHIA = await repository.Search(x => x.Id == Id || x.EHealthCode == EHealthCode ||
                                                        x.UHIAId == UHIAId || (x.ShortDescAr == ShortDescAr && (!string.IsNullOrEmpty(x.ShortDescAr))) ||
                                                        ( x.ShortDescEn == ShortDescEn && (!string.IsNullOrEmpty(x.ShortDescEn))), 1, 1,false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
                if (dbServiceUHIA.Data.Any())
                {
                    if (dbServiceUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "EHealthCode is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "UHIAId is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_33",
                            ErrorMessage = "UHIAId is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescAr == ShortDescAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescEn == ShortDescEn).Any())
                    {
                        dublicatedProperties += "ShortDescEn is duplicated,";
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
                if (dbServiceUHIA.Data.Any(x => x.Id != Id))
                {
                    dbServiceUHIA.Data = dbServiceUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbServiceUHIA.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "EHealthCode is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "UHIAId is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_33",
                            ErrorMessage = "UHIAId is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescAr == ShortDescAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescEn == ShortDescEn).Any())
                    {
                        dublicatedProperties += "ShortDescEn is duplicated,";
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

        private async Task<string?> GetdDuplicatesErrors(IServiceUHIARepository repository)
        {
            var dbServiceUHIA = await repository.Search(x =>( x.Id == Id || x.EHealthCode == EHealthCode ||
                                                        x.UHIAId == UHIAId || (x.ShortDescAr == ShortDescAr && (!string.IsNullOrEmpty(x.ShortDescAr))) ||
                                                        (x.ShortDescEn == ShortDescEn && (!string.IsNullOrEmpty(x.ShortDescEn)))) && x.IsDeleted != true, 1, 1, false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbServiceUHIA.Data.Any())
                {
                    if (dbServiceUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "Duplicated UHIAId,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescAr == ShortDescAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescEn == ShortDescEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbServiceUHIA.Data.Any(x => x.Id != Id))
                {
                    dbServiceUHIA.Data = dbServiceUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbServiceUHIA.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "Duplicated UHIAId,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescAr == ShortDescAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbServiceUHIA.Data.Where(x => x.ShortDescEn == ShortDescEn).Any())
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
