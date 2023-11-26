using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;
using EHealth.ManageItemLists.Application.Resource.UHIA.Commands;
using EHealth.ManageItemLists.Application.Resource.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Resource.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Text;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityProvider _identityProvider;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        public ResourceUHIAController(IMediator mediator, IIdentityProvider identityProvider, IResourceUHIARepository resourceUHIARepository)
        {
            _mediator = mediator;
            _identityProvider = identityProvider;
            _resourceUHIARepository = resourceUHIARepository;
        }

        [Authorize(Roles = "itemslist_resource_uhia_view")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ResourceUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<ResourceUHIADto>>> Search([FromQuery] ResourceUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Authorize(Roles = "itemslist_resource_uhia_details,itemslist_resource_uhia_update")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ResourceUHIADto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<ResourceUHIADto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new ResourceUHIAGetByIdQuery { Id = id }));
        }

        [Authorize(Roles = "itemslist_resource_uhia_add")]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateResourceUHIABasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateResourceUHIABasicDataCommand(request));
            return Ok(id);
        }

        [Authorize(Roles = "itemslist_resource_uhia_add")]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceCreate([FromBody] CreateResourceUHIAPriceDto request)
        {
            bool resourceUHIAId = await _mediator.Send(new CreateResourceUHIAPricesCommand(request, _resourceUHIARepository));
            return Ok(resourceUHIAId);
        }

        [Authorize(Roles = "itemslist_resource_uhia_update")]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateResourceUHIABasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateResourceUHIABasicDataCommand(request, _resourceUHIARepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_resource_uhia_update")]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateResourceUHIAPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateResourceUHIAPricesCommand(request, _resourceUHIARepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_resource_uhia_delete")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteResourceUHIACommand { Id = id }));
        }

        [Authorize(Roles = "itemslist_resource_uhia_export")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ResourceUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<ResourceUHIADto>>> CreateTemplateResourceUHIADto([FromQuery] CreateTemplateResourceUHIADtoSearchQuery request)
        {
            var lang = Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
         
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "ResourceUHIADto.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "ResourceUHIADto.csv";
                return GenerateCSV(fileName, res);
            }
        }

        [Authorize(Roles = "itemslist_resource_uhia_bulkupload")]
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadResourceUhiaBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Resource - Uhia.xlsx");
        }
        private FileResult GenerateExcel(string fileName, DataTable dataTable)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                //wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    wb.ColumnWidth = 20;
                    wb.Worksheets.Add(dataTable);
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }

        [Authorize(Roles = "itemslist_resource_uhia_bulkupload")]
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadResourceUhiaCreateCommand(file));

            if (res != null)
            {
                var fileName = "Resource-UHIA-With-Errors.xlsx";
                return File(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return Ok(true);
        }
        private FileResult GenerateCSV(string fileName, DataTable dataTable)
        {
            var csv = new StringBuilder();
            using (var csvWriter = new CsvWriter(new StringWriter(csv), new CsvConfiguration(CultureInfo.InvariantCulture)))
            {

                foreach (DataColumn column in dataTable.Columns)
                {
                    csvWriter.WriteField(column.ColumnName);
                }
                csvWriter.NextRecord();


                foreach (DataRow dataRow in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        csvWriter.WriteField(dataRow[i]);
                    }
                    csvWriter.NextRecord();
                }
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(bytes, "text/csv", fileName);
            }

        }
    }
}
