using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedItemsPackageComponentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly ISharedItemsPackageDrugRepository _sharedItemsPackageDrugRepository;

        public SharedItemsPackageComponentController(IMediator mediator,
            IPackageHeaderRepository packageHeaderRepository,
            ILocationsRepository locationsRepository,
            IDrugsUHIARepository drugsUHIARepository,
            ISharedItemsPackageDrugRepository sharedItemsPackageDrugRepository)
        {
            _mediator = mediator;
            _packageHeaderRepository = packageHeaderRepository;
            _locationsRepository = locationsRepository;
            _drugsUHIARepository = drugsUHIARepository;
            _sharedItemsPackageDrugRepository = sharedItemsPackageDrugRepository;
        }
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<Guid> CreateDrugItem([FromBody] CreateDrugItemDTO request)
        {
            return await _mediator.Send(new CreateDrugItemCommand(request, _packageHeaderRepository, _drugsUHIARepository, _locationsRepository));
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<bool> UpdateDrugItem([FromBody] UpdateDrugItemDTO request)
        {
            return await _mediator.Send(new UpdateDrugItemCommand(request, _packageHeaderRepository, _drugsUHIARepository, _sharedItemsPackageDrugRepository, _locationsRepository));
        }
        [HttpDelete("{sharedItemsPackageDrugId:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<bool> DeleteDrugItem([FromRoute] Guid sharedItemsPackageDrugId)
        {
            return await _mediator.Send(new DeleteDrugItemCommand(sharedItemsPackageDrugId));
        }
    }
}
