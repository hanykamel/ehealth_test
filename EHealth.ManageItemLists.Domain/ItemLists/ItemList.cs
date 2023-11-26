using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EHealth.ManageItemLists.Domain.ItemLists
{
    public class ItemList : ItemManagmentBaseClass, IEntity<int>, IValidationModel<ItemList>
    {
        private ItemList()
        {

        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public int ItemListSubtypeId { get; private set; }
        [NotMapped]
        public int? ItemCounts { get; set; }
        public bool IsBusy { get; set; }

        public IList<ServiceUHIA> serviceUHIAlist { get; private set; } = new List<ServiceUHIA>();
        public IList<ConsumablesAndDevicesUHIA> ConsumablesAndDevicesUHIAlist { get; private set; } = new List<ConsumablesAndDevicesUHIA>();
        public IList<ProcedureICHI> ProcedureICHIlist { get; private set; } = new List<ProcedureICHI>();
        public IList<DevicesAndAssetsUHIA> DevicesAndAssetsUHIAlist { get; private set; } = new List<DevicesAndAssetsUHIA>();
        public IList<FacilityUHIA> FacilityUHIAlist { get; private set; } = new List<FacilityUHIA>();
        public IList<ResourceUHIA> ResourceUHIAlist { get; private set; } = new List<ResourceUHIA>();
        public IList<DoctorFeesUHIA> DoctorFeesUHIAlist { get; private set; } = new List<DoctorFeesUHIA>();
        public IList<DrugUHIA> DrugUHIAlist { get; private set; } = new List<DrugUHIA>();

        public ItemListSubtype ItemListSubtype { get; private set; }
        public AbstractValidator<ItemList> Validator => new ItemListValidator();

        public void SetIsBusy(bool isBusy)
        {
            if (IsBusy == isBusy) return;
            IsBusy = isBusy;
        }
        public static async Task IsListBusy(IItemListRepository repository, int Id, bool throwException = true)
        {
            bool isBusy = (await repository.IsListBusy(Id));

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
        public void SetNameAr(string nameAr)
        {
            if (NameAr == nameAr) return;
            NameAr = nameAr;
        }
        public void SetNameEn(string nameEn)
        {
            if (NameEN == nameEn) return;
            NameEN = nameEn;
        }
        public void SetItemListSubtype(ItemListSubtype itemListSubtype)
        {
            if (ItemListSubtype == itemListSubtype) return;
            ItemListSubtype = itemListSubtype;
        }
        public void SetItemListSubtypeId(int itemListSubtypeId)
        {
            if (ItemListSubtypeId == itemListSubtypeId) return;
            ItemListSubtypeId = itemListSubtypeId;
        }
        public async Task<int> Create(IItemListRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository); // TODO after create getll repo
            return await repository.Create(this);
        }

        public async Task<bool> Update(IItemListRepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IItemListRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<ItemList>> Search(IItemListRepository repository, Expression<Func<ItemList, bool>> predicate, int pageNumber, int pageSize, string? orderBy, bool? ascending, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, orderBy, ascending, enablePagination);
        }

        public static async Task<ItemList> Get(int id, IItemListRepository repository)
        {
            var dbCategory = await repository.Get(id);

            if (dbCategory is null)
            {
                throw new DataNotFoundException();
            }

            return dbCategory;
        }

        public static ItemList Create(int? id, string code, string nameAr, string nameEn, int ItemListSubtypeId, string? createdBy, string tenantId)
        {
            return new ItemList
            {
                Id = id ?? 0,
                Code = code,
                NameAr = nameAr,
                NameEN = nameEn,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                ItemListSubtypeId = ItemListSubtypeId,
                TenantId = tenantId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IItemListRepository repository, bool throwException = true)
        {
            var dbItemList = await repository.Search(i => (i.Id == Id || i.Code == Code || i.NameAr == NameAr || i.NameEN == NameEN) && i.IsDeleted != true, 1, 1, null, null, false);
            string dublicatedProperties = "";
            if (Id == default)
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();

                if (dbItemList.Data.Any())
                {
                    if (dbItemList.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Code,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "Code is Duplicated",

                        });
                    }
                    if (dbItemList.Data.Where(x => x.NameAr == NameAr).Any())
                    {
                        dublicatedProperties += "NameAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "NameAr is Duplicated",

                        });

                    }
                    if (dbItemList.Data.Where(x => x.NameEN == NameEN).Any())
                    {
                        dublicatedProperties += "NameEN,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "NameEN is Duplicated",

                        });

                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            else
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
                if (dbItemList.Data.Any(x => x.Id != Id))
                {

                    dbItemList.Data = dbItemList.Data.Where(x => x.Id != Id).ToList();

                    if (dbItemList.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Code,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "Code is Duplicated",

                        });
                    }
                    if (dbItemList.Data.Where(x => x.NameAr == NameAr).Any())
                    {
                        dublicatedProperties += "NameAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "NameAr is Duplicated",

                        });

                    }
                    if (dbItemList.Data.Where(x => x.NameEN == NameEN).Any())
                    {
                        dublicatedProperties += "NameEN,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "NameEN is Duplicated",

                        });

                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            return true;
        }
        public async Task<string> ValidateObjectForBulkUpload(IItemListRepository repository, IValidationEngine validationEngine)
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
        private async Task<string?> GetdDuplicatesErrors(IItemListRepository repository)
        {
            var dbItemList = await repository.Search(i => (i.Id == Id || i.Code == Code || i.NameAr == NameAr || i.NameEN == NameEN) && i.IsDeleted != true, 1, 1, null, null, false);
            string dublicatedProperties = "";
            if (Id == default)
            {
                if (dbItemList.Data.Any())
                {
                    if (dbItemList.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Duplicated Id,";
                    }
                    if (dbItemList.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated Code,";
                    }
        
                    if (dbItemList.Data.Where(x => x.NameAr == NameAr).Any())
                    {
                        dublicatedProperties += "Duplicated NameAr,";
                    }
                    if (dbItemList.Data.Where(x => x.NameEN == NameEN).Any())
                    {
                        dublicatedProperties += "Duplicated NameEN,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            else
            {
                if (dbItemList.Data.Any(x => x.Id != Id))
                {
                    dbItemList.Data = dbItemList.Data.Where(x => x.Id != Id).ToList();
                    if (dbItemList.Data.Where(x => x.Code == Code).Any())
                    {
                        dublicatedProperties += "Duplicated Code,";
                    }

                    if (dbItemList.Data.Where(x => x.NameAr == NameAr).Any())
                    {
                        dublicatedProperties += "Duplicated NameAr,";
                    }
                    if (dbItemList.Data.Where(x => x.NameEN == NameEN).Any())
                    {
                        dublicatedProperties += "Duplicated NameEN,";
                    }
                    return dublicatedProperties;
                    //return "These fields are duplicated (" + dublicatedProperties + ")";
                }
            }
            return null;
        }
    }
}
