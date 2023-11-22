using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;
using EHealth.ManageItemLists.Application.Facility.UHIA.Commands;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
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
    public class FacilityUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FacilityUHIAController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<FacilityUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<FacilityUHIADto>>> Search([FromQuery] FacilityUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FacilityUHIADto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<FacilityUHIADto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new FacilityUHIAGetByIdQuery { Id = id}));
        }

        //[Authorize]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateFacilityUHIADto request)
        {
            Guid id = await _mediator.Send(new CreateFacilityUHIACommand(request));
            return Ok(id);
        }

        //[Authorize]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<FacilityUHIADto>> BasicDataUpdate([FromBody] UpdateFacilityUHIADto request)
        {
            return Ok(await _mediator.Send(new UpdateFacilityUHIACommand(request)));
        }

        //[Authorize]
        [HttpDelete("BasicData/Delete/{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataDelete(Guid id)
        {
            bool res = await _mediator.Send(new DeleteFacilityUHIACommand(id));
            return Ok(res);
        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<FacilityUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<FacilityUHIADto>>> CreateTemplateFacilityUHIA([FromQuery] CreateTemplateFacilityUHIASearchQuery request)
        {
            var lang =  Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
            
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "FacilityUHIA.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "FacilityUHIA.csv";
                return GenerateCSV(fileName, res);
            }
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadFacilityUhiaBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Facility - UHIA.xlsx");
        }
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadFacilityCreateCommand(file));

            if (res != null)
            {
                var fileName = "Facility-UHIA-With-Errors.xlsx";
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
