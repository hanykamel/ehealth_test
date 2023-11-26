using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentCostPackageComponentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        public InvestmentCostPackageComponentController(IMediator mediator,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository,
            IPackageHeaderRepository packageHeaderRepository,
            IFacilityUHIARepository facilityUHIARepository
            )
        {
            _mediator = mediator;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _packageHeaderRepository = packageHeaderRepository;
            _facilityUHIARepository = facilityUHIARepository;
        }

        [HttpGet("GetPackageFacilityUHIA")]
        [ProducesResponseType(typeof(InvestmentCostPackageFacilityUHIADTO), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<InvestmentCostPackageFacilityUHIADTO>> GetPackageFacilityUHIA([FromQuery] Guid id)
        {
            var res = await _mediator.Send(new InvestmentCostPackageFacilityUHIAQuery { PackageHeaderId = id });
            if (res == null)
            {
                return Ok(null);   
            }
            return Ok(res);
        }

        [HttpGet("GetPackageAssets")]
        [ProducesResponseType(typeof(PagedResponse<InvestmentCostPackageAssetsDTO>), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<PagedResponse<InvestmentCostPackageAssetsDTO>>> GetPackageAssets([FromQuery] InvestmentCostPackageAssetsQuery query)
        {
            var x = await _mediator.Send(query);
            return Ok(x);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<CreatedResult> Create([FromBody] SetInvestmentCostComponentDto request)
        {
            var id = await _mediator.Send(new SetInvestmentCostComponentCommand(request, _facilityUHIARepository, _packageHeaderRepository));
            return Created("api/InvestmentCost/" + id, id);
        }
        [HttpDelete("{packageHeaderId:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> DeleteFacility([FromRoute] Guid packageHeaderId)
        {
            return Ok(await _mediator.Send(new DeleteInvestmentCostFacilityCommand (_packageHeaderRepository) { PackageHeaderId = packageHeaderId }));
        }
    }
}
