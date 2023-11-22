using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Office2016.Excel;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using System.Data;
using System.Globalization;
using System.Text;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServiceUHIARepository _serviceUHIARepository;

        public ServicesUHIAController(IMediator mediator, IServiceUHIARepository serviceUHIARepository)
        {
            _mediator = mediator;
            _serviceUHIARepository = serviceUHIARepository;
        }

        //[Authorize]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateServicesUHIABasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateServicesUHIABasicDataCommand(request));
            return Ok(id);
        }

        //[Authorize]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> PriceCreate([FromBody] CreateServicesUHIAPriceDto request)
        {
            Guid serviceUHIAId = await _mediator.Send(new CreateServicesUHIAPricesCommand(request, _serviceUHIARepository));
            return Ok(serviceUHIAId);
        }

        //[Authorize]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateServicesUHIABasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateServicesUHIABasicDataCommand(request, _serviceUHIARepository));
            return Ok(res);
        }

        //[Authorize]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateServicesUHIAPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateServicesUHIAPricesCommand(request, _serviceUHIARepository));
            return Ok(res);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ServiceUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<ServiceUHIADto>>> Search([FromQuery] ServiceUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        ////[Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ServiceUHIADto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<ServiceUHIADto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new ServiceUHIAGetByIdQuery { Id = id }));
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteServicesUHIACommand { Id = id }));
        }

        //[Authorize]
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadCreateCommand(file));

            if (res != null)
            {
                var fileName = "Service-UHIA-With-Errors.xlsx";
                return File(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return Ok(true);
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ServiceUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<ServiceUHIADto>>> CreateTemplateServiceUHIA([FromQuery] CreateTemplateServiceUHIASearchQuery request)
        {
            var lang = Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
           

            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "service.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "service.csv";
                return GenerateCSV(fileName, res);
            }

        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Service-UHIA.xlsx");
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
