using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.PackageItems.DTOs;
using EHealth.ManageItemLists.Application.PackageItems.Queries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PackageItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllFacilities")]
        [ProducesResponseType(typeof(GetAllFacilitiesDTO), 200)]
        public async Task<ActionResult<List<GetAllFacilitiesDTO>>> GetAllFacilities([FromQuery] GetAllFacilitiesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(GetAllAssetsAndMedicalEquipmentsDTO), 200)]
        public async Task<ActionResult<PagedResponse<GetAllAssetsAndMedicalEquipmentsDTO>>> GetAllAssetsAndMedicalEquipments([FromQuery] GetAllAssetsAndMedicalEquipmentsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(GetAllResourceItemsDTO),200)]
        public async Task<ActionResult<PagedResponse<GetAllResourceItemsDTO>>> GetAllResourceItems([FromQuery]GetAllResourceItemsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(GetAllDoctorsFeesDTO), 200)]
        public async Task<ActionResult<PagedResponse<GetAllDoctorsFeesDTO>>> GetAllDoctorFees([FromQuery]GetAllDoctorsFeesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(GetAllDrugsDTO), 200)]
        public async Task<ActionResult<PagedResponse<GetAllDrugsDTO>>> GetAllDrugs([FromQuery] GetAllDrugsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(GetAllConsumablesAndDevicesDTO), 200)]
        public async Task<ActionResult<PagedResponse<GetAllConsumablesAndDevicesDTO>>> GetAllConsumablesAndDevices([FromQuery] GetAllConsumablesAndDevicesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
