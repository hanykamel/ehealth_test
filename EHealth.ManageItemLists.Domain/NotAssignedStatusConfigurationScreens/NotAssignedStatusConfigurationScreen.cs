using EHealth.ManageItemLists.Domain.Rejectreasons;
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

namespace EHealth.ManageItemLists.Domain.NotAssignedStatusConfigurationScreens
{
    public class NotAssignedStatusConfigurationScreen : EHealthDomainObject, IEntity<int>, IValidationModel<NotAssignedStatusConfigurationScreen>
    {
        private NotAssignedStatusConfigurationScreen()
        {
            
        }
        public int Id { get; set; }
        public int TotalNumberOfDays  { get; set; }
        public int SendNotificationEvery { get; set; }

        public AbstractValidator<NotAssignedStatusConfigurationScreen> Validator => new NotAssignedStatusConfigurationScreenValidator();
        AbstractValidator<NotAssignedStatusConfigurationScreen> IValidationModel<NotAssignedStatusConfigurationScreen>.Validator => throw new NotImplementedException();
        public async Task<int> Create(INotAssignedStatusConfigurationScreenRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Create(this);
        }

        public async Task<bool> Update(INotAssignedStatusConfigurationScreenRepository repository, IValidationEngine validationEngine)
        {
            validationEngine.Validate(this);
            await EnsureNoDuplicates(repository);
            return await repository.Update(this);
        }

        public async Task<bool> Delete(INotAssignedStatusConfigurationScreenRepository repository)
        {
            return await repository.Delete(this);
        }

        public static async Task<PagedResponse<NotAssignedStatusConfigurationScreen>> Search(INotAssignedStatusConfigurationScreenRepository repository, int id, int totalNumberOfDays, int sendNotificationEvery, int pageNumber, int pageSize)
        {
            return await repository.Search(id, totalNumberOfDays, sendNotificationEvery, pageNumber, pageSize);
        }

        public static async Task<NotAssignedStatusConfigurationScreen> Get(int id, INotAssignedStatusConfigurationScreenRepository repository)
        {
            var dbNotAssignedStatusConfigurationScreen = await repository.Get(id);

            if (dbNotAssignedStatusConfigurationScreen is null)
            {
                throw new DataNotFoundException();
            }

            return dbNotAssignedStatusConfigurationScreen;
        }

        public static NotAssignedStatusConfigurationScreen Create(int? id, int totalNumberOfDays, int sendNotificationEvery, string createdBy)
        {
            return new NotAssignedStatusConfigurationScreen
            {
                Id = id ?? 0,
                TotalNumberOfDays = totalNumberOfDays,
                SendNotificationEvery = sendNotificationEvery,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };
        }

        private async Task<bool> EnsureNoDuplicates(INotAssignedStatusConfigurationScreenRepository repository, bool throwException = true)
        {
            var dbNotAssignedStatusConfigurationScreen = await repository.Search(Id, TotalNumberOfDays, SendNotificationEvery, 1, 1);
            if (Id == default)
            {
                if (dbNotAssignedStatusConfigurationScreen.Data.Any())
                {
                    throw new DataDuplicateException();
                }
            }
            else
            {
                if (dbNotAssignedStatusConfigurationScreen.Data.Any(x => x.Id != Id))
                {
                    throw new DataDuplicateException();
                }
            }
            return true;
        }
    }
}
