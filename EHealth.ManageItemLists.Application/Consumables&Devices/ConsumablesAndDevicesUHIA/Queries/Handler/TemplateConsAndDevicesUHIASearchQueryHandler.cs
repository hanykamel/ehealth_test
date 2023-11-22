using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries.Handler
{
    public class TemplateConsAndDevicesUHIASearchQueryHandler : IRequestHandler<CreateTemplateConsAndDevUHIASearchQuery, DataTable>
    {
        private readonly IMediator _mediator;
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        public TemplateConsAndDevicesUHIASearchQueryHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository, IMediator mediator)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateConsAndDevUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var consAndDevUHIASearchQuery = new ConsAndDevUHIASearchQuery();
            consAndDevUHIASearchQuery.ItemListId = request.ItemListId;
            consAndDevUHIASearchQuery.PageNo = request.PageNo;
            consAndDevUHIASearchQuery.PageSize = request.PageSize;
            consAndDevUHIASearchQuery.EnablePagination = false;
            var res = await _mediator.Send(consAndDevUHIASearchQuery);
            DataTable dataTable = new DataTable("excel");
     
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("الكود الخاص بهيئه التأمين الصحي");
                dataTable.Columns.Add("وحدة القياس المحليه انجليزي");
                dataTable.Columns.Add("وحدة القياس المحليه عربي");
                dataTable.Columns.Add("وصف قصير انجليزي");
                dataTable.Columns.Add("وصف قصير عربي");
                dataTable.Columns.Add("الفئه الاساسيه انجليزي");
                dataTable.Columns.Add("الفئه الاساسيه عربي");
                dataTable.Columns.Add("الفئه الفرعيه انجليزي");
                dataTable.Columns.Add("الفئه الفرعيه عربي");
                dataTable.Columns.Add("البيان تاريخ التفعيل من");
                dataTable.Columns.Add("البيان تاريخ التفعيل الي");
                dataTable.Columns.Add("السعر");
                dataTable.Columns.Add("السعر تاريخ التفعيل من");
                dataTable.Columns.Add("السعر تاريخ التفعيل الي");
            }
            else
            {
                dataTable.Columns.Add("eHealthCode");
                dataTable.Columns.Add("UHIA Id");
                dataTable.Columns.Add("unit of measure En");
                dataTable.Columns.Add("unit of measure Ar");
                dataTable.Columns.Add("Short Description En");
                dataTable.Columns.Add("Short Description Ar");
                dataTable.Columns.Add("Service Category En");
                dataTable.Columns.Add("Service Category Ar");
                dataTable.Columns.Add("SubCategory En");
                dataTable.Columns.Add("SubCategory Ar");
                dataTable.Columns.Add("Data-Effective Date from");
                dataTable.Columns.Add("Data-Effective Date to");
                dataTable.Columns.Add("Price");
                dataTable.Columns.Add("Price Data-Effective Date from");
                dataTable.Columns.Add("Price Data-Effective Date to");
            }



            foreach (var item in res.Data)
            {
                DataRow row = dataTable.NewRow();
              
                if (request.Lang.ToLower() == "ar")
                {
                    row["كود أي هيلث"] = item.EHealthCode;
                    row["الكود الخاص بهيئه التأمين الصحي"] = item.UHIAId;
                    row["وحدة القياس المحليه انجليزي"] = item.LocalUnitOfMeasure.MeasureTypeEn;
                    row["وحدة القياس المحليه عربي"] = item.LocalUnitOfMeasure.MeasureTypeAr;
                    row["وصف قصير انجليزي"] = item.ShortDescriptionEn;
                    row["وصف قصير عربي"] = item.ShortDescriptionAr;
                    row["الفئه الاساسيه انجليزي"] = item.Category?.CategoryEn;
                    row["الفئه الاساسيه عربي"] = item.Category?.CategoryAr;
                    row["الفئه الفرعيه انجليزي"] = item.SubCategory?.SubCategoryEn;
                    row["الفئه الفرعيه عربي"] = item.SubCategory?.SubCategoryAr;
                    row["البيان تاريخ التفعيل من"] = item.DataEffectiveDateFrom;
                    row["البيان تاريخ التفعيل الي"] = item.DataEffectiveDateTo;
                    row["السعر"] = item.ItemListPrice?.Price;
                    row["السعر تاريخ التفعيل من"] = item.ItemListPrice?.EffectiveDateFrom;
                    row["السعر تاريخ التفعيل الي"] = item.ItemListPrice?.EffectiveDateTo;
                }
                else
                {
                    row["eHealthCode"] = item.EHealthCode;
                    row["UHIA Id"] = item.UHIAId;
                    row["unit of measure En"] = item.LocalUnitOfMeasure.MeasureTypeEn;
                    row["unit of measure Ar"] = item.LocalUnitOfMeasure.MeasureTypeAr;
                    row["Short Description En"] = item.ShortDescriptionEn;
                    row["Short Description Ar"] = item.ShortDescriptionAr;
                    row["Service Category En"] = item.Category?.CategoryEn;
                    row["Service Category Ar"] = item.Category?.CategoryAr;
                    row["SubCategory En"] = item.SubCategory?.SubCategoryEn;
                    row["SubCategory Ar"] = item.SubCategory?.SubCategoryAr;
                    row["Data-Effective Date from"] = item.DataEffectiveDateFrom;
                    row["Data-Effective Date to"] = item.DataEffectiveDateTo;
                    row["Price"] = item.ItemListPrice?.Price;
                    row["Price Data-Effective Date from"] = item.ItemListPrice?.EffectiveDateFrom;
                    row["Price Data-Effective Date to"] = item.ItemListPrice?.EffectiveDateTo;
                }

             
                dataTable.Rows.Add(row);
            }

            return dataTable;




        }
    }

}
