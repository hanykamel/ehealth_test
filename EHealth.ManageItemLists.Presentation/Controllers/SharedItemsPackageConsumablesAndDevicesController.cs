using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedItemsPackageConsumablesAndDevicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;

        public SharedItemsPackageConsumablesAndDevicesController(IMediator mediator, ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository)
        {
            _mediator = mediator;
            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
        }

        //[Authorize]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateSharedItemsPackageConsumablesAndDevicesDto request)
        {
            Guid id = await _mediator.Send(new CreateSharedItemsPackageConsumablesAndDevicesCommand(request, _sharedItemsPackageConsumableAndDeviceRepository));
            return Ok(id);
        }

        //[Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteSharedItemsPackageConsumablesAndDevicesCommand { Id = id }));
        }

        //[Authorize]
        [HttpPost("Update")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateSharedItemsPackageConsumablesAndDevicesDto request)
        {
            bool res = await _mediator.Send(new UpdateSharedItemsPackageConsumablesAndDevicesCommand(request, _sharedItemsPackageConsumableAndDeviceRepository));
            return Ok(res);
        }
    }
}
