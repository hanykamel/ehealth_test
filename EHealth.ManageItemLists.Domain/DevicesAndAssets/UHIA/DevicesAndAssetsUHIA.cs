using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA
{
    public class DevicesAndAssetsUHIA : EHealthDomainObject, IEntity<Guid>, IValidationModel<DevicesAndAssetsUHIA>
    {
        private DevicesAndAssetsUHIA()
        {

        }
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public int ItemListId { get; set; }
        public ItemList ItemList { get; private set; }
        public int? UnitRoomId { get; private set; }
        public UnitRoom? UnitRoom { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int SubCategoryId { get; private set; }
        public SubCategory SubCategory { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<ItemListPrice> ItemListPrices { get; private set; } = new List<ItemListPrice>();
        public AbstractValidator<DevicesAndAssetsUHIA> Validator => new DevicesAndAssetsUHIAValidator();

        public void SetEHealthCode(string etEHealthCode)
        {
            if (Code == etEHealthCode) return;
            Code = etEHealthCode;
        }
        public void SetDescriptorAr(string descriptorAr)
        {
            if (DescriptorAr == descriptorAr) return;
            DescriptorAr = descriptorAr;
        }
        public void SetDescriptorEn(string descriptorEn)
        {
            if (DescriptorEn == descriptorEn) return;
            DescriptorEn = descriptorEn;
        }
        public void SetCategoryId(int categoryId)
        {
            if (CategoryId == categoryId) return;
            CategoryId = categoryId;
        }
        public void SetSubCategoryId(int subCategoryId)
        {
            if (SubCategoryId == subCategoryId) return;
            SubCategoryId = subCategoryId;
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

        public void SetItemListId(int itemListId)
        {
            if (ItemListId == itemListId) return;
            ItemListId = itemListId;
        }
        public void SetUnitRoomId(int? unitRoomId)
        {
            if (UnitRoomId == unitRoomId) return;
            UnitRoomId = unitRoomId;
        }

        public async Task<Guid> Create(IDevicesAndAssetsUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IDevicesAndAssetsUHIARepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IDevicesAndAssetsUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);
        }
        public static async Task<PagedResponse<DevicesAndAssetsUHIA>> Search(IDevicesAndAssetsUHIARepository repository, Expression<Func<DevicesAndAssetsUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }

        public static async Task<DevicesAndAssetsUHIA> Get(Guid id, IDevicesAndAssetsUHIARepository repository)
        {
            var dbDevicesAndAssetsUHIA = await repository.Get(id);

            if (dbDevicesAndAssetsUHIA is null)
            {
                throw new DataNotFoundException();
            }

            return dbDevicesAndAssetsUHIA;
        }

        public static DevicesAndAssetsUHIA Create(Guid?id, string code, string descriptorAr, string descriptorEn, int? unitRoomId,
            int categoryId, int subCategoryId, DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo, int itemListId, string createdBy, string tenantId)
        {
            return new DevicesAndAssetsUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                Code = code,
                DescriptorAr = descriptorAr,
                DescriptorEn = descriptorEn,
                UnitRoomId = unitRoomId,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                TenantId = tenantId,
                ItemListId = itemListId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IDevicesAndAssetsUHIARepository repository, bool throwException = true)
        {
            var dbDevicesAndAssetsUHIA = await repository.Search(x =>( x.Id == Id || x.Code == Code
                                                         || (x.DescriptorAr == DescriptorAr && (!string.IsNullOrEmpty(x.DescriptorAr))) ||
                                                        x.DescriptorEn == DescriptorEn) && x.IsDeleted != true, 1, 1, true, null, null);
            string dublicatedProperties = "";
            List<ValidationFailure>? errors = new List<ValidationFailure>();
            if (Id == default)
            {
                if (dbDevicesAndAssetsUHIA.Data.Any())
                {
                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });

                    }
                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });

                    }

                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });

                    }
                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
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
                if (dbDevicesAndAssetsUHIA.Data.Any(x => x.Id != Id))
                {
                    dbDevicesAndAssetsUHIA.Data = dbDevicesAndAssetsUHIA.Data.Where(x => x.Id != Id).ToList();


                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });

                    }
                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "ShortDescAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "ShortDescAr is Duplicated",

                        });

                    }
                    if (dbDevicesAndAssetsUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
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
        public static async Task IsItemListBusy(IDevicesAndAssetsUHIARepository repository, int itemListId, bool throwException = true)
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
        public async Task<string> ValidateObjectForBulkUpload(IDevicesAndAssetsUHIARepository repository, IValidationEngine validationEngine)
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

        private async Task<string?> GetdDuplicatesErrors(IDevicesAndAssetsUHIARepository repository)
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
