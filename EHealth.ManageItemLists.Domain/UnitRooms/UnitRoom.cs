using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.UnitRooms
{
    public class UnitRoom : ItemManagmentBaseClass, IEntity<int>, IValidationModel<UnitRoom>
    {
        private UnitRoom()
        {
            //default value is Active
            this.Active = true;
        }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }
        public AbstractValidator<UnitRoom> Validator => new UnitRoomValidator();

        public async Task<int> Create(IUnitRoomRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.CreateUnitRoom(this);
        }

        public async Task<bool> Update(IUnitRoomRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.UpdateUnitRoom(this);
        }

        public async Task<bool> Delete(IUnitRoomRepository repository)
        {
            return await repository.DeleteUnitRoom(this);
        }

        public static async Task<PagedResponse<UnitRoom>> Search(IUnitRoomRepository repository, Expression<Func<UnitRoom, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            return await repository.Search(repository, predicate, pageNumber, pageSize, enablePagination);
        }

        public static async Task<UnitRoom> Get(int id, IUnitRoomRepository repository)
        {
            var dbUnitRoom = await repository.Get(id);

            if (dbUnitRoom is null)
            {
                throw new DataNotFoundException();
            }

            return dbUnitRoom;
        }

        public static UnitRoom Create(int? id, string code, string nameAr, string nameEN, string DefinitionAr, string DefinitionEN, string createdBy)
        {
            return new UnitRoom
            {
                Id = id ?? 0,
                Code = code,
                NameAr = nameAr,
                NameEN = nameEN,
                DefinitionAr = DefinitionAr,
                DefinitionEN = DefinitionEN,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,

            };
        }

        private async Task<bool> EnsureNoDuplicates(IUnitRoomRepository repository, bool throwException = true)
        {
            var dbUnitRoom = await repository.Search(repository, r => r.Id == Id && r.NameAr == NameAr && r.NameEN == NameEN && r.IsDeleted == true,1,1,true);
            if (Id == default)
            {
                if (dbUnitRoom.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbUnitRoom.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
