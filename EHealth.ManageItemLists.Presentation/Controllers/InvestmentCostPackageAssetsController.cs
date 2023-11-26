using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Application.Facility.UHIA.Commands;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Queries;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Queries;
using EHealth.ManageItemLists.Application.ItemLists.Commands;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.Commands;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentCostPackageAssetsController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;

        public InvestmentCostPackageAssetsController(IMediator mediator,
            IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository,
            IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _mediator = mediator;
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }

        //[Authorize]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateInvestmentCostAssetsDto request)
        {
            Guid id = await _mediator.Send(new CreateInvestmentCostAssetsCommand(request, _investmentCostPackageAssetRepository, _investmentCostPackageComponentRepository, _devicesAndAssetsUHIARepository));
            return Ok(id);
        }

        //[Authorize]
        [HttpPut("Update")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> Update([FromBody] UpdateInvestmentCostAssetsDto request)
        {
            bool res = await _mediator.Send(new UpdateInvestmentCostAssetsCommand(request, _investmentCostPackageAssetRepository, _investmentCostPackageComponentRepository, _devicesAndAssetsUHIARepository));
            return Ok(res);
        }

        //[Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteInvestmentCostAssetsCommand { Id = id }));
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(InvestmentCostSummaryDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<FeesOfResourceSummaryDto>> CalculateFeesOfResourcesSummary([FromQuery] InvestmentCostCalculateSummaryDto request)
        {
            return Ok(await _mediator.Send(new InvestmentCostCalculateSummaryQuery(request, _investmentCostPackageComponentRepository)));
        }
        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<InvestmentCostAssetsDto>), 200)]
        public async Task<ActionResult<PagedResponse<InvestmentCostAssetsDto>>> ViewAddedAssets([FromQuery] InvestmentCostAssetsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
