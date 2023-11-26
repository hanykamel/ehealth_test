using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
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
    public class DevicesAndAssetsUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;

        public DevicesAndAssetsUHIAController(IMediator mediator, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _mediator = mediator;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_view")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DevicesAndAssetsUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<DevicesAndAssetsUHIADto>>> Search([FromQuery] DevicesAndAssetsUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_details,itemslist_devices&assets_uhia_update")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DevicesAndAssetsUHIADto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<DevicesAndAssetsUHIADto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DevicesAndAssetsUHIAGetByIdQuery { Id = id }));
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_delete")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteDevicesAndAssetsUHIACommand { Id = id }));
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_add")]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateDevicesAndAssetsUHIABasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateDevicesAndAssetsUHIABasicDataCommand(request));
            return Ok(id);
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_add")]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> PriceCreate([FromBody] CreateDevicesAndAssetsUHIAPriceDto request)
        {
            Guid devicesAndAssetsUHIAId = await _mediator.Send(new CreateDevicesAndAssetsUHIAPricesCommand(request, _devicesAndAssetsUHIARepository));
            return Ok(devicesAndAssetsUHIAId);
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_update")]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateDevicesAndAssetsUHIABasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateDevicesAndAssetsUHIABasicDataCommand(request, _devicesAndAssetsUHIARepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_update")]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateDevicesAndAssetsUHIAPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateDevicesAndAssetsUHIAPricesCommand(request, _devicesAndAssetsUHIARepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_export")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DevicesAndAssetsUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<DevicesAndAssetsUHIADto>>> CreateTemplateDeviceAndAssetUHIA([FromQuery] CreateTemplateDevicesAndAssetUHIASearchQuery request)
        {
            var lang = Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
            
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "DeviceAndAssetUHIA.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "DeviceAndAssetUHIA.csv";
                return GenerateCSV(fileName, res);
            }
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_bulkupload")]
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadDevicesndAssetsUhiaBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Devices And Assets - UHIA.xlsx");
        }

        [Authorize(Roles = "itemslist_devices&assets_uhia_bulkupload")]
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadDevicesAndAssetsCreateCommand(file));

            if (res != null)
            {
                var fileName = "Devices&Assets-UHIA-With-Errors.xlsx";
                return File(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return Ok(true);
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
    }
}
