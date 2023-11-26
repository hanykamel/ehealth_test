using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using EHealth.ManageItemLists.Application.Procedure.ICHI.Commands;
using EHealth.ManageItemLists.Application.Procedure.ICHI.DTOs;
using EHealth.ManageItemLists.Application.Procedure.ICHI.Queries;
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
    public class ProcedureICHIController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProcedureICHIRepository _procedureICHIRepository;

        public ProcedureICHIController(IMediator mediator, IProcedureICHIRepository procedureICHIRepository)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _mediator = mediator;
            _procedureICHIRepository = procedureICHIRepository;
        }

        [Authorize(Roles = "itemslist_procedure_ichi_view")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ProcedureICHIDto>), 200)]
        public async Task<ActionResult<PagedResponse<ProcedureICHIDto>>> Search([FromQuery] ProcedureICHISearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Authorize(Roles = "itemslist_procedure_ichi_details,itemslist_procedure_ichi_update")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProcedureICHIDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<ProcedureICHIDto>> Get([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new ProcedureICHIGetByIdQuery { Id = id }));
        }

        [Authorize(Roles = "itemslist_procedure_ichi_delete")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteProcedureICHICommand { Id = id }));
        }

        [Authorize(Roles = "itemslist_procedure_ichi_add")]
        [HttpPost("BasicData/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<Guid>> BasicDataCreate([FromBody] CreateProcedureICHIBasicDataDto request)
        {
            Guid id = await _mediator.Send(new CreateProcedureICHIBasicDataCommand(request));
            return Ok(id);
        }

        [Authorize(Roles = "itemslist_procedure_ichi_add")]
        [HttpPost("Prices/Create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<Guid>> PriceCreate([FromBody] CreateProcedureICHIPriceDto request)
        {
            Guid procedureICHIId = await _mediator.Send(new CreateProcedureICHIPricesCommand(request, _procedureICHIRepository));
            return Ok(procedureICHIId);
        }

        [Authorize(Roles = "itemslist_procedure_ichi_update")]
        [HttpPut("BasicData/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
        public async Task<ActionResult<bool>> BasicDataUpdate([FromBody] UpdateProcedureICHIBasicDataDto request)
        {
            bool res = await _mediator.Send(new UpdateProcedureICHIBasicDataCommand(request, _procedureICHIRepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_procedure_ichi_update")]
        [HttpPut("Prices/Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<bool>> PriceUpdate([FromBody] UpdateProcedureICHIPriceDto request)
        {
            bool res = await _mediator.Send(new UpdateProcedureICHIPricesCommand(request, _procedureICHIRepository));
            return Ok(res);
        }

        [Authorize(Roles = "itemslist_procedure_ichi_export")]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ProcedureICHIDto>), 200)]
        public async Task<ActionResult<PagedResponse<ProcedureICHIDto>>> CreateTemplateProcedureICHI([FromQuery] CreateTemplateProcedureICHISearchQuery request)
        {
            var lang =  Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);
           
            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "ProcedureICHI.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "ProcedureICHI.csv";
                return GenerateCSV(fileName, res);
            }
        }

        [Authorize(Roles = "itemslist_procedure_ichi_bulkupload")]
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadProcedureICHIBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Procedure - ICHI.xlsx");
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

        [Authorize(Roles = "itemslist_procedure_ichi_bulkupload")]
        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<IActionResult> BulkUpload([FromForm] IFormFile file)
        {
            var res = await _mediator.Send(new BulkUploadProcedureICHICreateCommand(file));

            if (res != null)
            {
                var fileName = "Procedure-ICHI-With-Errors.xlsx";
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
