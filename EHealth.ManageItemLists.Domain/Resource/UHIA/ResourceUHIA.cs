using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Resource.UHIA
{
    public class ResourceUHIA : EHealthDomainObject, IEntity<Guid>, IGetPrice, IValidationModel<ResourceUHIA>
    {
        private ResourceUHIA()
        {

        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string? DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public int ItemListId { get; set; }
        public ItemList ItemList { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int SubCategoryId { get; private set; }
        public SubCategory SubCategory { get; private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<ResourceItemPrice> ItemListPrices { get; private set; } = new List<ResourceItemPrice>();

        public AbstractValidator<ResourceUHIA> Validator => new ResourceUHIAValidator();

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

        public async Task<Guid> Create(IResourceUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }
        public async Task<bool> Update(IResourceUHIARepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IResourceUHIARepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);

        }

        public static async Task<PagedResponse<ResourceUHIA>> Search(IResourceUHIARepository repository, Expression<Func<ResourceUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }

        public static async Task<ResourceUHIA> Get(Guid id, IResourceUHIARepository repository)
        {
            var dbResourceUHIA = await repository.Get(id);

            if (dbResourceUHIA is null)
            {
                throw new DataNotFoundException();
            }

            return dbResourceUHIA;
        }

        public static ResourceUHIA Create(Guid? id, string code, string? descriptorAr, string descriptorEn,
           int categoryId, int subCategoryId, DateTime dataEffectiveDateFrom, int itemList, DateTime? dataEffectiveDateTo, string createdBy, string tenantId)
        {
            return new ResourceUHIA
            {
                Id = id == null ? new Guid() : (Guid)id,
                Code = code,
                DescriptorAr = descriptorAr,
                DescriptorEn = descriptorEn,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                ItemListId = itemList,
                TenantId = tenantId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IResourceUHIARepository repository, bool throwException = true)
        {
            var dbResourceUHIA = await repository.Search(x => (x.Id == Id || x.Code == Code || x.DescriptorAr == DescriptorAr ||
                                                                              x.DescriptorEn == DescriptorEn) && IsDeleted != true, 1, 1, true, null, null);

            string dublicatedProperties = "";
            if (Id == default)
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
                if (dbResourceUHIA.Data.Any())
                {
                    if (dbResourceUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });
                    }
                    if (dbResourceUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbResourceUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "DescriptorAr is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "DescriptorAr is Duplicated",

                        });
                    }
                    if (dbResourceUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
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
                if (dbResourceUHIA.Data.Any(x => x.Id != Id))
                {
                    dbResourceUHIA.Data = dbResourceUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbResourceUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbResourceUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "DescriptorAr is duplicated,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "DescriptorAr is Duplicated",

                        });
                    }
                    if (dbResourceUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
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

        public async Task AddResourcePrices(IResourceUHIARepository repository, IValidationEngine validationEngine, List<ResourceItemPrice> ResourcePrices)
        {
            foreach (var price in ResourcePrices)
            {
                this.ItemListPrices.Add(price);
            }
            await repository.Update(this);
        }

        public void DeleteResourcePrices(List<ResourceItemPrice> ResourcePrices, string deletedBy)
        {
            foreach (var price in ResourcePrices)
            {
                price.SoftDelete(deletedBy);
            }
        }
        public static async Task IsItemListBusy(IResourceUHIARepository repository, int itemListId, bool throwException = true)
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
        public async Task<string> ValidateObjectForBulkUpload(IResourceUHIARepository repository, IValidationEngine validationEngine)
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
        private async Task<string?> GetdDuplicatesErrors(IResourceUHIARepository repository)
        {
            var dbResourceUHIA = await repository.Search(x => (x.Id == Id || x.Code == Code || x.DescriptorAr == DescriptorAr ||
                                                                              x.DescriptorEn == DescriptorEn) && IsDeleted != true, 1, 1, true, null, null);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbResourceUHIA.Data.Any())
                {
                    if (dbResourceUHIA.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbResourceUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }

                    if (dbResourceUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbResourceUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbResourceUHIA.Data.Any(x => x.Id != Id))
                {
                    dbResourceUHIA.Data = dbResourceUHIA.Data.Where(x => x.Id != Id).ToList();
                    if (dbResourceUHIA.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated EHealthCode,";
                    }

                    if (dbResourceUHIA.Data.Where(x => x.DescriptorAr == DescriptorAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbResourceUHIA.Data.Where(x => x.DescriptorEn == DescriptorEn).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescEn,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            return null;
        }

        public async Task<ResourceItemPrice?> GetPriceByDate(DateTime? date)
        {
            ResourceItemPrice? price;
            if (date.HasValue)
            {
                price = ItemListPrices.Where(p => p.EffectiveDateFrom <= date && (p.EffectiveDateTo >= date || p.EffectiveDateTo is null)).FirstOrDefault();
                if (price is null)
                {
                    price = ItemListPrices.OrderByDescending(p => p.EffectiveDateFrom).FirstOrDefault();
                }
            }
            else
            {
                price = ItemListPrices.OrderByDescending(p => p.EffectiveDateFrom).FirstOrDefault();
            }
            return price;
        }
    }
}
