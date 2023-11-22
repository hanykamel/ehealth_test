using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Text;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorFeesUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public DoctorFeesUHIAController(IMediator mediator, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _mediator = mediator;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }
        //[Authorize]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateDoctorFeesUHIABasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateDoctorFeesUHIABasicDataCommand(request));
            return Ok(id);
        }
        //[Authorize]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> PriceCreate([FromBody] CreateDoctorFeesUHIAPriceDto request)
        {
            return Ok(await _mediator.Send(new CreateDoctorFeesUHIAPricesCommand(request, _doctorFeesUHIARepository)));

        }
        //[Authorize]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateDoctoerFeesUHIABasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateDoctorFeesUHIABasicDataCommand(request, _doctorFeesUHIARepository));
            return Ok(res);
        }
        //[Authorize]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateDoctorFeesUHIAPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateDoctorFeesUHIAPricesCommand(request, _doctorFeesUHIARepository));
            return Ok(res);
        }
        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DoctorFeesUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<DoctorFeesUHIADto>>> Search([FromQuery] DoctorFeesUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        //[Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DoctorFeesUHIADto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<DoctorFeesUHIADto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DoctorFeesUHIAGetByIdQuery { Id = id }));
        }

        //[Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteDoctorFeesUHIACommand { Id = id }));
        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DoctorFeesUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<DoctorFeesUHIADto>>> CreateTemplateDoctorFeesUHIA([FromQuery] CreateTemplateDoctorFeesUHIASearchQuery request)
        {
            var lang =  Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
           
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "DoctorFeesUHIA.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "DoctorFeesUHIA.csv";
                return GenerateCSV(fileName, res);
            }
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadDoctorFeesUhiaBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doctor Fees - UHIA.xlsx");
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadDrFeesCreateCommand(file));

            if (res != null)
            {
                var fileName = "Doctor's Fees-UHIA-With-Errors.xlsx";
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
