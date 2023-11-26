using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.Commands;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands.Handlers;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesOfResourcesPerUnitPackageComponentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFeesOfResourcesPerUnitPackageComponentRepository _feesOfResourcesPerUnitPackageComponentRepository ;
        private readonly IFeesOfResourcesPerUnitPackageResourceRepository _feesOfResourcesPerUnitPackageResourceRepository;
        public FeesOfResourcesPerUnitPackageComponentController(IMediator mediator ,
          IFeesOfResourcesPerUnitPackageComponentRepository feesOfResourcesPerUnitPackageComponentRepository,
          IFeesOfResourcesPerUnitPackageResourceRepository feesOfResourcesPerUnitPackageResourceRepository)
        {
            _feesOfResourcesPerUnitPackageComponentRepository = feesOfResourcesPerUnitPackageComponentRepository;
            _feesOfResourcesPerUnitPackageResourceRepository = feesOfResourcesPerUnitPackageResourceRepository;
            _mediator = mediator;
        }
        //[Authorize]
        [HttpPost("Facility/Add")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> AddFacilityItemToPackage([FromBody] CreateFeesOfResourcesPerUnitPackageDto request)
        {
            var id = await _mediator.Send(new CreateFeesOfResourcesPerUnitPackageCommand(request,_feesOfResourcesPerUnitPackageComponentRepository));
            return Ok(id);
        
        }
        //[Authorize]
        [HttpPost("Resource/Add")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)] 
        public async Task<ActionResult<Guid>> AddResourcesItemToPackage([FromBody] CreateFeesOfResourcesPerUnitPackageResourceDto request)
        {
            var id = await _mediator.Send(new CreateFeesOfResourcesPerUnitPackageResourceCommand(request, _feesOfResourcesPerUnitPackageResourceRepository));
            return Ok(id);

        }
        //[Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> UpdateResourcesItemToPackage([FromBody] UpdateFeesOfResourcesPerUnitPackageResourceDto request)
        {
            var id = await _mediator.Send(new UpdateFeesOfResourcesPerUnitPackageResourceCommand(request, _feesOfResourcesPerUnitPackageResourceRepository));
            return Ok(id);

        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(FacilityUHIADto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<FeesOfResourceSummaryDto>> CalculateFeesOfResourcesSummary([FromQuery] FeesOfResourceCalculateSummaryDto request)
        {
            return Ok(await _mediator.Send(new FeesOfResourceCalculateSummaryQuery(request, _feesOfResourcesPerUnitPackageComponentRepository)));
        }
    }
}
