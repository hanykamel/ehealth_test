using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.ConsumablesAndDevices
{
    public class ConsumablesAndDevicesUHIA : EHealthDomainObject, IEntity<Guid>, IValidationModel<ConsumablesAndDevicesUHIA>
    {
        private ConsumablesAndDevicesUHIA()
        {

        }
        public Guid Id { get; private set; }
        public string EHealthCode { get; private set; }
        public string UHIAId { get; private set; }
        public string? ShortDescriptorAr { get; private set; }
        public string ShortDescriptorEn { get; private set; }

        public int UnitOfMeasureId { get; private set; }
        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public int ServiceCategoryId { get; private set; }
        public Category ServiceCategory { get; private set; }
        public int SubCategoryId { get; private set; }
        public SubCategory SubCategory { get; private set; }
        public int ItemListId { get; set; }
        public ItemList ItemList { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<ItemListPrice> ItemListPrices { get; private set; } = new List<ItemListPrice>();


        public AbstractValidator<ConsumablesAndDevicesUHIA> Validator => new ConsumablesAndDevicesUHIAValidator();

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
            if (ShortDescriptorAr == shortDescAr) return;
            ShortDescriptorAr = shortDescAr;
        }
        public void SetShortDescEn(string shortDescEn)
        {
            if (ShortDescriptorEn == shortDescEn) return;
            ShortDescriptorEn = shortDescEn;
        }
        public void SetServiceCategoryId(int serviceCategoryId)
        {
            if (ServiceCategoryId == serviceCategoryId) return;
            ServiceCategoryId = serviceCategoryId;
        }
        public void SetServiceSubCategoryId(int serviceSubCategoryId)
        {
            if (SubCategoryId == serviceSubCategoryId) return;
            SubCategoryId = serviceSubCategoryId;
        }

        public void SetUnitOfMeasureId(int unitOfMeasureId)
        {
            if (UnitOfMeasureId == unitOfMeasureId) return;
            UnitOfMeasureId = unitOfMeasureId;
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



        public async Task<Guid> Create(IConsumablesAndDevicesUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IConsumablesAndDevicesUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IConsumablesAndDevicesUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<ConsumablesAndDevicesUHIA>> Search(IConsumablesAndDevicesUHIARepository repository, Expression<Func<ConsumablesAndDevicesUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }

        public static async Task<ConsumablesAndDevicesUHIA> Get(Guid id, IConsumablesAndDevicesUHIARepository repository)
        {
            var dbConsumablesAndDevicesUSHI = await repository.Get(id);

            if (dbConsumablesAndDevicesUSHI is null)
            {
                throw new DataNotFoundException();
            }

            return dbConsumablesAndDevicesUSHI;
        }

        public static ConsumablesAndDevicesUHIA Create(Guid? id, string eHealthCode, string uHIAId, string? shortDescriptorAr, string shortDescriptorEn,
            int serviceCategoryId, int subCategoryId, int itemListId, int unitOfMeasureId, DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo, string createdBy, string tenantId)
        {
            return new ConsumablesAndDevicesUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                EHealthCode = eHealthCode,
                UHIAId = uHIAId,
                ShortDescriptorAr = shortDescriptorAr,
                ShortDescriptorEn = shortDescriptorEn,
                ServiceCategoryId = serviceCategoryId,
                SubCategoryId = subCategoryId,
                ItemListId = itemListId,
                UnitOfMeasureId = unitOfMeasureId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTimeOffset.Now,
                TenantId = tenantId
            };
        }

        public async Task<bool> EnsureNoDuplicates(IConsumablesAndDevicesUHIARepository repository, bool throwException = true)
        {
            var dbConsumablesAndDevices = await repository.Search(x =>( x.Id == Id || x.EHealthCode == EHealthCode ||
                                                        x.UHIAId == UHIAId || (x.ShortDescriptorAr == ShortDescriptorAr && (!string.IsNullOrEmpty(x.ShortDescriptorAr))) ||
                                                        x.ShortDescriptorEn == ShortDescriptorEn) && x.IsDeleted != true, 1, 1, true, null, null);

            string dublicatedProperties = "";
            List<ValidationFailure>? errors = new List<ValidationFailure>();

            if (Id == default)
            {
                if (dbConsumablesAndDevices.Data.Any())
                {

                    if (dbConsumablesAndDevices.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "UHIAId,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_33",
                            ErrorMessage = "UHIAId is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.ShortDescriptorAr == ShortDescriptorAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.ShortDescriptorEn == ShortDescriptorEn).Any())
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
                
                if (dbConsumablesAndDevices.Data.Any(x => x.Id != Id))
                {
                    dbConsumablesAndDevices.Data = dbConsumablesAndDevices.Data.Where(x => x.Id != Id).ToList();
                    if (dbConsumablesAndDevices.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "UHIAId,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_33",
                            ErrorMessage = "UHIAId is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.ShortDescriptorAr == ShortDescriptorAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });

                    }
                    if (dbConsumablesAndDevices.Data.Where(x => x.ShortDescriptorEn == ShortDescriptorEn).Any())
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
        public static async Task IsItemListBusy(IConsumablesAndDevicesUHIARepository repository, int itemListId, bool throwException = true)
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
        public async Task<string> ValidateObjectForBulkUpload(IConsumablesAndDevicesUHIARepository repository, IValidationEngine validationEngine)
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
        private async Task<string?> GetdDuplicatesErrors(IConsumablesAndDevicesUHIARepository repository)
        {
            var dbConsAndDevUHIA = await repository.Search(x =>( x.Id == Id || x.EHealthCode == EHealthCode ||
                                                        x.UHIAId == UHIAId || (x.ShortDescriptorEn == ShortDescriptorAr && (!string.IsNullOrEmpty(x.ShortDescriptorAr))) ||
                                                        (x.ShortDescriptorEn == ShortDescriptorEn && (!string.IsNullOrEmpty(x.ShortDescriptorEn))))
                                                        && x.IsDeleted != true, 1, 1, false, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbConsAndDevUHIA.Data.Any())
                {
                    if (dbConsAndDevUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "Duplicated UHIAId,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.ShortDescriptorAr == ShortDescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.ShortDescriptorEn == ShortDescriptorEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbConsAndDevUHIA.Data.Any(x => x.Id != Id))
                {
                    dbConsAndDevUHIA.Data = dbConsAndDevUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbConsAndDevUHIA.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "Duplicated UHIAId,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.ShortDescriptorAr == ShortDescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.ShortDescriptorAr == ShortDescriptorAr).Any())
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
