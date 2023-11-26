
using ClosedXML.Excel;
using CsvHelper.Configuration;
using CsvHelper;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries;

using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Text;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using Microsoft.AspNetCore.Authorization;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumablesAndDevicesUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;

        public ConsumablesAndDevicesUHIAController(IMediator mediator, IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository)
        {
            _mediator = mediator;
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_view")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ConsAndDevDto>), 200)]
        public async Task<ActionResult<PagedResponse<ConsAndDevDto>>> Search([FromQuery] ConsAndDevUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_details,itemslist_consumables&devices_uhia_update")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ConsAndDevDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<ConsAndDevDto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new ConsAndDevUHIAGetByIdQuery { Id = id }));
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_add")]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateConsAndDevUHIABasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateConsAndDevUHIABasicDataCommand(request));
            return Ok(id);
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_add")]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> PriceCreate([FromBody] CreateConsAndDevUHIAPricesDto request)
        {
            Guid consAndDevUHIAId = await _mediator.Send(new CreateConsAndDevUHIAPricesCommand(request, _consumablesAndDevicesUHIARepository));
            return Ok(consAndDevUHIAId);

        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_update")]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateConsAndDevUHIABasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateConsAndDevUHIABasicDataCommand(request, _consumablesAndDevicesUHIARepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_update")]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateConsAndDevUHIAPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateConsAndDevUHIAPricesCommand(request, _consumablesAndDevicesUHIARepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_delete")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteConsAndDevsUHIACommand { Id = id }));
        }

        [Authorize(Roles = "itemslist_consumables&devices_uhia_export")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ConsAndDevDto>), 200)]
        public async Task<ActionResult<PagedResponse<ConsAndDevDto>>> CreateTemplateConsAndDevUHIA([FromQuery] CreateTemplateConsAndDevUHIASearchQuery request)
        { 
            var lang = Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
           
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "ConsAndDevUHIA.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "ConsAndDevUHIA.csv";
                return GenerateCSV(fileName, res);
            }
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

        [Authorize(Roles = "itemslist_consumables&devices_uhia_bulkupload")]
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadConsumablesAndDevicesBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Consumables And Devices - UHIA.xlsx");
        }


        [Authorize(Roles = "itemslist_consumables&devices_uhia_bulkupload")]
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadConsAndDevsCreateCommand(file));

            if (res != null)
            {
                var fileName = "Consumables&devices-UHIA-With-Errors.xlsx";
                return File(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return Ok(true);
        }
    }
}
 