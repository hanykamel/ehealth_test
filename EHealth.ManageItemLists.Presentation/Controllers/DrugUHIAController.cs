using ClosedXML.Excel;
using CsvHelper.Configuration;
using CsvHelper;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs;
using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands;
using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Drugs.UHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Presentation.ExceptionHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Text;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;

namespace EHealth.ManageItemLists.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugUHIAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDrugsUHIARepository _drugsUHIARepository;

        public DrugUHIAController(IMediator mediator, IDrugsUHIARepository drugsUHIARepository)
        {
            _mediator = mediator;
            _drugsUHIARepository = drugsUHIARepository;
        }

        //[Authorize]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateDrugUHIABasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateDrugUHIABasicDataCommand(request));
            return Ok(id);
        }

        //[Authorize]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceCreate([FromBody] CreateDrugUHIAPricesDto request)
        {
            var res = await _mediator.Send(new CreateDrugsUHIAPricesCommand(request, _drugsUHIARepository));
            return Ok(res);
        }

        //[Authorize]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateDrugUHIABasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateDrugUHIABasicDataCommand(request, _drugsUHIARepository));
            return Ok(res);
        }

        //[Authorize]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateDrugUHIAPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateDrugUHIAPricesCommand(request, _drugsUHIARepository));
            return Ok(res);
        }

        //[Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteDrugsUHIACommand { Id = id }));
        }
        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DrugsUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<DrugsUHIADto>>> Search([FromQuery] DrugUHIASearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        //[Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DrugUHIAGetByIdDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<DrugUHIAGetByIdDto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DrugUHIAGetByIdQuery { Id = id }));
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<DrugsUHIADto>), 200)]
        public async Task<ActionResult<PagedResponse<DrugsUHIADto>>> CreateTemplateDrugsUHIA([FromQuery] CreateTemplateDrugsUHIASearchQuery request)
        {
            var lang =  Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
          
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "DrugsUHIA.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "DrugsUHIA.csv";
                return GenerateCSV(fileName, res);
            }
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadDrugsUhiaBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Drugs - UHIA.xlsx");
        }
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadDrugsUhiaCreateCommand(file));

            if (res != null)
            {
                var fileName = "Drug-UHIA-With-Errors.xlsx";
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
