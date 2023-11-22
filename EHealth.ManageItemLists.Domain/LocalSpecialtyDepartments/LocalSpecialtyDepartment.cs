using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments
{
    public class LocalSpecialtyDepartment : ItemManagmentBaseClass, IEntity<int>, IValidationModel<LocalSpecialtyDepartment>
    {
        private LocalSpecialtyDepartment()
        {
            //default value is Active
            this.Active = true;
        }
 
        public string LocalSpecialityAr { get; private set; }
        public string LocalSpecialityENG { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionENG { get; private set; }
    
        public AbstractValidator<LocalSpecialtyDepartment> Validator => new LocalSpecialtyDepartmentValidator();
        AbstractValidator<LocalSpecialtyDepartment> IValidationModel<LocalSpecialtyDepartment>.Validator => throw new NotImplementedException();
        public async Task<int> Create(ILocalSpecialtyDepartmentsRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateLocalSpecialtyDepartments(this);
        }

        public async Task<bool> Update(ILocalSpecialtyDepartmentsRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateLocalSpecialtyDepartments(this);
        }

        public async Task<bool> Delete(ILocalSpecialtyDepartmentsRepository repository)
        {
            return await repository.DeleteLocalSpecialtyDepartments(this);
        }

        public static async Task<PagedResponse<LocalSpecialtyDepartment>> Search(ILocalSpecialtyDepartmentsRepository repository, Expression<Func<LocalSpecialtyDepartment, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<LocalSpecialtyDepartment> Get(int id, ILocalSpecialtyDepartmentsRepository repository)
        {
            var dbLocalSpecialtyDepartment = await repository.Get(id);

            if (dbLocalSpecialtyDepartment is null)
            {
                throw new DataNotFoundException();
            }

            return dbLocalSpecialtyDepartment;
        }

        public static LocalSpecialtyDepartment Create(int? id, string code, string localSpecialityAr, string localSpecialityENG, string DefinitionAr, string DefinitionENG, string createdBy)
        {
            return new LocalSpecialtyDepartment
            {
                Id = id ?? 0,
                Code = code,
                LocalSpecialityAr = localSpecialityAr,
                LocalSpecialityENG = localSpecialityENG,
                DefinitionAr = DefinitionAr,
                DefinitionENG = DefinitionENG,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(ILocalSpecialtyDepartmentsRepository repository, bool throwException = true)
        {
            var dbLocalSpecialtyDepartment = await repository.Search(x => x.IsDeleted != true, 1, 1, false);
            if (Id == default)
            {
                if (dbLocalSpecialtyDepartment.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbLocalSpecialtyDepartment.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }

    }
}
