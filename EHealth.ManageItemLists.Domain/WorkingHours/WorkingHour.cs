using EHealth.ManageItemLists.Domain.PublicVacations;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.WorkingHours
{
    public class WorkingHour : EHealthDomainObject, IEntity<int>, IValidationModel<WorkingHour>
    {
        public int Id { get; set; }
        public TimeOnly FromTime { get; private set; }
        public TimeOnly ToTime { get; private set; }
        public DayOfWeek NonWorkingDays { get; private set; }

        public AbstractValidator<WorkingHour> Validator => new WorkingHourValidator();
        AbstractValidator<WorkingHour> IValidationModel<WorkingHour>.Validator => throw new NotImplementedException();
        public async Task<int> Create(IWorkingHourRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(IWorkingHourRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(IWorkingHourRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<WorkingHour>> Search(IWorkingHourRepository repository, int id, TimeOnly fromTime, TimeOnly toTime, DayOfWeek nonWorkingDayes, int pageNumber, int pageSize)
        {
            return await repository.Search(id, fromTime, toTime, nonWorkingDayes, pageNumber, pageSize);
        }

        public static async Task<WorkingHour> Get(int id, IWorkingHourRepository repository)
        {
            var dbWorkingHour = await repository.Get(id);

            if (dbWorkingHour is null)
            {
                throw new DataNotFoundException();
            }

            return dbWorkingHour;
        }

        public static WorkingHour Create(int? id, TimeOnly fromTime, TimeOnly toTime, DayOfWeek nonWorkingDays, string createdBy)
        {
            return new WorkingHour
            {
                Id = id ?? 0,
                FromTime = fromTime,
                ToTime = toTime,
                NonWorkingDays = nonWorkingDays,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(IWorkingHourRepository repository, bool throwException = true)
        {
            var dbWorkingHour = await repository.Search(Id, FromTime, ToTime, NonWorkingDays, 1, 1);
            if (Id == default)
            {
                if (dbWorkingHour.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbWorkingHour.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
