using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands;
using EHealth.ManageItemLists.Application.Facility.UHIA.Commands;
using EHealth.ManageItemLists.Application.ItemLists.Commands;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.Application.ItemLists.Queries;
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
    public class ItemListsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IItemListRepository _itemListRepository;
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        public ItemListsController(IMediator mediator, IItemListRepository itemListRepository , IServiceUHIARepository serviceUHIARepository,
         IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
        IProcedureICHIRepository procedureICHIRepository, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository, IFacilityUHIARepository facilityUHIARepository,
            IResourceUHIARepository resourceUHIARepository, IDoctorFeesUHIARepository doctorFeesUHIARepository)
        {
            _mediator = mediator;
            _itemListRepository = itemListRepository;   
            _procedureICHIRepository = procedureICHIRepository;
            _serviceUHIARepository = serviceUHIARepository;
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _facilityUHIARepository = facilityUHIARepository;
            _resourceUHIARepository = resourceUHIARepository;
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
        }
        ////[Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ItemListDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<ItemListDto>> Get([FromRoute] int id)
        {
            return Ok(await _mediator.Send(new GetItemListByIdQuery(id)));
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<CreatedResult> Create([FromBody] CreateItemListDto request)
        {
            

            var itemList = await _mediator.Send(new CreateItemListCommand(request));
            return Created("api/ItemLists/" + itemList.Id, itemList.Id);
        }

        //[Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(ItemListDto), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
        public async Task<ActionResult<ItemListDto>> Update([FromBody] UpdateItemListDto request)
        {
            return Ok(await _mediator.Send(new UpdateItemListCommand(request)));
        }

        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] int id)
        {
            return Ok(await _mediator.Send(new DeleteItemListCommand(new DeleteItemListDTO { Id = id }
           , _itemListRepository, _serviceUHIARepository, _consumablesAndDevicesUHIARepository, _procedureICHIRepository
           , _devicesAndAssetsUHIARepository, _facilityUHIARepository, _resourceUHIARepository, _doctorFeesUHIARepository)));
        }

        //[Authorize]
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ItemListDto>), 200)]
        public async Task<ActionResult<PagedResponse<ItemListDto>>> Search([FromQuery] SearchItemListQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(PagedResponse<ItemListDto>), 200)]
        public async Task<ActionResult<PagedResponse<ItemListDto>>> CreateTemplateItemList([FromQuery] CreateTemplateItemListSearchQuery request)
        {
            var lang = Request.Headers["Lang"];
            request.Lang = lang;
            var res = await _mediator.Send(request);

            if (request.FormatType.ToLower() == "excel")
            {
                var fileName = "ItemList.xlsx";
                return GenerateExcel(fileName, res);
            }
            else
            {
                var fileName = "ItemList.csv";
                return GenerateCSV(fileName, res);
            }
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> DownloadBulkTemplate([FromQuery] DownloadItemListBulkTemplateCommand request)
        {
            var result = await _mediator.Send(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ItemList.xlsx");
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
