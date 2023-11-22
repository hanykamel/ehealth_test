using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI
{
    public class ProcedureICHI : EHealthDomainObject, IEntity<Guid>, IValidationModel<ProcedureICHI>
    {
        private ProcedureICHI()
        {

        }
        public Guid Id { get; private set; }
        public string EHealthCode { get; private set; }
        public string UHIAId { get; private set; }

        public string? TitleAr { get; private set; }
        public string TitleEn { get; private set; }

        public int ServiceCategoryId { get; private set; }
        public Category ServiceCategory { get; private set; }
        public int SubCategoryId { get; private set; }
        public SubCategory SubCategory { get; private set; }
        public int ItemListId { get; private set; }
        public ItemList ItemList { get; private set; }
        public int? LocalSpecialtyDepartmentId { get; private set; }
        public LocalSpecialtyDepartment? LocalSpecialtyDepartment { get; private set; }

        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<ItemListPrice> ItemListPrices { get; private set; } = new List<ItemListPrice>();
        public AbstractValidator<ProcedureICHI> Validator => new ProcedureICHIValidator();

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
        public void SetTitleAr(string? titleAr)
        {
            if (TitleAr == titleAr) return;
            TitleAr = titleAr;
        }
        public void SetTitleEn(string titleEn)
        {
            if (TitleEn == titleEn) return;
            TitleEn = titleEn;
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
        public void SetLocalSpecialtyDepartmentId(int? localSpecialtyDepartmentId)
        {
            if (LocalSpecialtyDepartmentId == localSpecialtyDepartmentId) return;
            LocalSpecialtyDepartmentId = localSpecialtyDepartmentId;
        }

        public async Task AddPrices(IProcedureICHIRepository repository, IValidationEngine validationEngine, List<ItemListPrice> pricesLst)
        {
            foreach (var price in pricesLst)
            {
                validationEngine.Validate(price);
                this.ItemListPrices.Add(price);
            }
            await repository.Update(this);
        }

        public void DeletePrices(List<ItemListPrice> pricesLst, string deletedBy)
        {
            foreach (var price in pricesLst)
            {
                price.SoftDelete(deletedBy);
            }
        }

        public async Task<Guid> Create(IProcedureICHIRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IProcedureICHIRepository repository, IValidationEngine validationEngine, string modifiedBy)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTimeOffset.Now;
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IProcedureICHIRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<ProcedureICHI>> Search(IProcedureICHIRepository repository, Expression<Func<ProcedureICHI, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination, orderBy, ascending);
        }

        public static async Task<ProcedureICHI> Get(Guid id, IProcedureICHIRepository repository)
        {
            var dbProcedureICHI = await repository.Get(id);

            if (dbProcedureICHI is null)
            {
                throw new DataNotFoundException();
            }

            return dbProcedureICHI;
        }

        public static ProcedureICHI Create(Guid? id, string eHealthCode, string UHIAId, string? titleAr, string titleEn,
            int serviceCategoryId, int subCategoryId, int itemListId, DateTime dataEffectiveDateFrom, DateTime? dataEffectiveDateTo,
            int? localSpecialtyDepartmentId, string createdBy, string tenantId)
        {
            return new ProcedureICHI
            {
                Id = id == null ? new Guid() : (Guid)id,
                EHealthCode = eHealthCode,
                UHIAId = UHIAId,
                TitleAr = titleAr,
                TitleEn = titleEn,
                SubCategoryId = subCategoryId,
                ItemListId = itemListId,
                ServiceCategoryId = serviceCategoryId,
                DataEffectiveDateFrom = dataEffectiveDateFrom,
                DataEffectiveDateTo = dataEffectiveDateTo,
                LocalSpecialtyDepartmentId = localSpecialtyDepartmentId,
                CreatedBy = createdBy,
                CreatedOn = DateTimeOffset.Now,
                TenantId = tenantId
            };
        }

        private async Task<bool> EnsureNoDuplicates(IProcedureICHIRepository repository, bool throwException = true)
        {
            var dbProcedureICHI = await repository.Search(x =>( x.Id == Id || x.EHealthCode == EHealthCode ||
                                                        x.UHIAId == UHIAId || (x.TitleAr == TitleAr && (!string.IsNullOrEmpty(x.TitleAr))) ||
                                                        x.TitleEn == TitleEn) && x.IsDeleted != true, 1, 1, false, null, null);

            string dublicatedProperties = "";
            if (Id == default)
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();
                if (dbProcedureICHI.Data.Any())
                {
                    if (dbProcedureICHI.Data.Where(x => x.Id == Id).Any())
                    {
                        dublicatedProperties += "Id,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_09",
                            ErrorMessage = "Id is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "UHIAId,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_33",
                            ErrorMessage = "UHIAId is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.TitleAr == TitleAr).Any())
                    {
                        dublicatedProperties += "TitleAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "TitleAr is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.TitleEn == TitleEn).Any())
                    {
                        dublicatedProperties += "TitleEn,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "TitleEn is Duplicated",

                        });
                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);
                }
            }
            else
            {
                List<ValidationFailure>? errors = new List<ValidationFailure>();

                if (dbProcedureICHI.Data.Any(x => x.Id != Id))
                {
                    dbProcedureICHI.Data = dbProcedureICHI.Data.Where(x => x.Id != Id).ToList();

                    if (dbProcedureICHI.Data.Where(x => x.EHealthCode == EHealthCode).Any())
                    {
                        dublicatedProperties += "EHealthCode,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_32",
                            ErrorMessage = "EHealthCode is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.UHIAId == UHIAId).Any())
                    {
                        dublicatedProperties += "UHIAId,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_33",
                            ErrorMessage = "UHIAId is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.TitleAr == TitleAr).Any())
                    {
                        dublicatedProperties += "TitleAr,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_20",
                            ErrorMessage = "TitleAr is Duplicated",

                        });
                    }
                    if (dbProcedureICHI.Data.Where(x => x.TitleEn == TitleEn).Any())
                    {
                        dublicatedProperties += "TitleEn,";
                        errors.Add(new ValidationFailure
                        {
                            ErrorCode = "ItemManagement_MSG_19",
                            ErrorMessage = "TitleEn is Duplicated",

                        });
                    }
                    throw new DataDuplicateException(dublicatedProperties, errors);

                }
            }
            return true;
        }
        public static async Task IsItemListBusy(IProcedureICHIRepository repository, int itemListId, bool throwException = true)
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
        public async Task<string> ValidateObjectForBulkUpload(IProcedureICHIRepository repository, IValidationEngine validationEngine)
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
        private async Task<string?> GetdDuplicatesErrors(IProcedureICHIRepository repository)
        {
            var dbConsAndDevUHIA = await repository.Search(x => (x.Id == Id || x.EHealthCode == EHealthCode ||
                                                        x.UHIAId == UHIAId || (x.TitleAr == TitleAr && (!string.IsNullOrEmpty(x.TitleAr))) ||
                                                        x.TitleEn == TitleEn) && x.IsDeleted != true, 1, 1, false, null, null);
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
                    if (dbConsAndDevUHIA.Data.Where(x => x.TitleAr == TitleAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.TitleEn == TitleEn).Any())
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
                    if (dbConsAndDevUHIA.Data.Where(x => x.TitleAr == TitleAr).Any())
                    {
                        dublicatedProperties += "Duplicated ShortDescAr,";
                    }
                    if (dbConsAndDevUHIA.Data.Where(x => x.TitleEn == TitleEn).Any())
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
