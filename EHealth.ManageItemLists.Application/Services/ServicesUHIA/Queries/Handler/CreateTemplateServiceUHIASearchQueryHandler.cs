using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Queries.Handler
{
    public class CreateTemplateServiceUHIASearchQueryHandler : IRequestHandler<CreateTemplateServiceUHIASearchQuery, DataTable>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IMediator _mediator;
        public CreateTemplateServiceUHIASearchQueryHandler(IServiceUHIARepository serviceUHIARepository, IMediator mediator)
        {
            _mediator = mediator;
            _serviceUHIARepository = serviceUHIARepository;
        }
        public async Task<DataTable> Handle(CreateTemplateServiceUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var serviceUHIASearchQuery = new ServiceUHIASearchQuery();

            serviceUHIASearchQuery.PageNo = request.PageNo;
            serviceUHIASearchQuery.ItemListId = request.ItemListId;
            serviceUHIASearchQuery.PageSize = request.PageSize;
            serviceUHIASearchQuery.EnablePagination =false;
           

            var res = await _mediator.Send(serviceUHIASearchQuery);
            DataTable dataTable = new DataTable("excel");
            
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("الكود الخاص بهيئه التأمين الصحي");
                dataTable.Columns.Add("الوصف انجليزي");
                dataTable.Columns.Add("الوصف عربي");
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
                    row["الوصف انجليزي"] = item.ShortDescEn;
                    row["الوصف عربي"] = item.ShortDescAr;
                    row["الفئه الاساسيه انجليزي"] = item.ServiceCategory?.CategoryEn;
                    row["الفئه الاساسيه عربي"] = item.ServiceCategory?.CategoryAr;
                    row["الفئه الفرعيه انجليزي"] = item.ServiceSubCategory?.SubCategoryEn;
                    row["الفئه الفرعيه عربي"] = item.ServiceSubCategory?.SubCategoryAr;
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
                    row["Short Description En"] = item.ShortDescEn;
                    row["Short Description Ar"] = item.ShortDescAr;
                    row["Service Category En"] = item.ServiceCategory?.CategoryEn;
                    row["Service Category Ar"] = item.ServiceCategory?.CategoryAr;
                    row["SubCategory En"] = item.ServiceSubCategory?.SubCategoryEn;
                    row["SubCategory Ar"] = item.ServiceSubCategory?.SubCategoryAr;
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
