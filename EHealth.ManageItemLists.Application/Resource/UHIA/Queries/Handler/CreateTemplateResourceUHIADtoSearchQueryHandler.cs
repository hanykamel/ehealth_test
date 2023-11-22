using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Queries.Handler
{
    public class CreateTemplateResourceUHIADtoSearchQueryHandler : IRequestHandler<CreateTemplateResourceUHIADtoSearchQuery, DataTable>
    {
        private readonly IMediator _mediator;

        public CreateTemplateResourceUHIADtoSearchQueryHandler(IMediator mediator)
        {

            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateResourceUHIADtoSearchQuery request, CancellationToken cancellationToken)
        {
            var resourceUHIASearchQuery = new ResourceUHIASearchQuery();
            resourceUHIASearchQuery.ItemListId = request.ItemListId;
            resourceUHIASearchQuery.PageNo = request.PageNo;
            resourceUHIASearchQuery.PageSize = request.PageSize;
            resourceUHIASearchQuery.EnablePagination =false;


            var res = await _mediator.Send(resourceUHIASearchQuery);
            DataTable dataTable = new DataTable("excel");
            
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("الوصف انجليزي");
                dataTable.Columns.Add("الوصف عربي");
                dataTable.Columns.Add("الفئه الاساسيه انجليزي");
                dataTable.Columns.Add("الفئه الاساسيه عربي");
                dataTable.Columns.Add("الفئه الفرعيه انجليزي");
                dataTable.Columns.Add("الفئه الفرعيه عربي");
                dataTable.Columns.Add("البيان تاريخ التفعيل من");
                dataTable.Columns.Add("البيان تاريخ التفعيل الي");
                dataTable.Columns.Add("السعر");
                dataTable.Columns.Add("وحدة السعر انجليزي");
                dataTable.Columns.Add("وحدة السعر عربي");
                dataTable.Columns.Add("السعر تاريخ التفعيل من");
                dataTable.Columns.Add("السعر تاريخ التفعيل الي");
            }
            else
            {
                dataTable.Columns.Add("eHealthCode");
                dataTable.Columns.Add("Descriptor En");
                dataTable.Columns.Add("Descriptor Ar");
                dataTable.Columns.Add("Category En");
                dataTable.Columns.Add("Category Ar");
                dataTable.Columns.Add("SubCategory En");
                dataTable.Columns.Add("SubCategory Ar");
                dataTable.Columns.Add("Data-Effective Date from");
                dataTable.Columns.Add("Data-Effective Date to");
                dataTable.Columns.Add("Price");
                dataTable.Columns.Add("PriceUnit En");
                dataTable.Columns.Add("PriceUnit Ar");
                dataTable.Columns.Add("Price Data-Effective Date from");
                dataTable.Columns.Add("Price Data-Effective Date to");
            }


      

            foreach (var item in res.Data)
            {
                DataRow row = dataTable.NewRow();
               
                if (request.Lang.ToLower() == "ar")
                {
                    row["كود أي هيلث"] = item.EHealthCode;
                    row["الوصف انجليزي"] = item.DescriptorEn;
                    row["الوصف عربي"] = item.DescriptorAr;
                    row["الفئه الاساسيه انجليزي"] = item.Category?.CategoryEn;
                    row["الفئه الاساسيه عربي"] = item.Category?.CategoryAr;
                    row["الفئه الفرعيه انجليزي"] = item.SubCategory?.SubCategoryEn;
                    row["الفئه الفرعيه عربي"] = item.SubCategory?.SubCategoryAr;
                    row["البيان تاريخ التفعيل من"] = item.DataEffectiveDateFrom;
                    row["البيان تاريخ التفعيل الي"] = item.DataEffectiveDateTo;
                    row["السعر"] = item.ItemListPrice?.Price;
                    row["وحدة السعر انجليزي"] = item.ItemListPrice?.PriceUnit?.NameEN;
                    row["وحدة السعر عربي"] = item.ItemListPrice?.PriceUnit?.NameAr;
                    row["السعر تاريخ التفعيل من"] = item.ItemListPrice?.EffectiveDateFrom;
                    row["السعر تاريخ التفعيل الي"] = item.ItemListPrice?.EffectiveDateTo;
                }
                else
                {
                    row["eHealthCode"] = item.EHealthCode;
                    row["Descriptor En"] = item.DescriptorEn;
                    row["Descriptor Ar"] = item.DescriptorAr;
                    row["Category En"] = item.Category?.CategoryEn;
                    row["Category Ar"] = item.Category?.CategoryAr;
                    row["SubCategory En"] = item.SubCategory?.SubCategoryEn;
                    row["SubCategory Ar"] = item.SubCategory?.SubCategoryEn;
                    row["Data-Effective Date from"] = item.DataEffectiveDateFrom;
                    row["Data-Effective Date to"] = item.DataEffectiveDateTo;
                    row["Price"] = item.ItemListPrice?.Price;
                    row["PriceUnit En"] = item.ItemListPrice?.PriceUnit?.NameEN;
                    row["PriceUnit Ar"] = item.ItemListPrice?.PriceUnit?.NameAr;
                    row["Price Data-Effective Date from"] = item.ItemListPrice?.EffectiveDateFrom;
                    row["Price Data-Effective Date to"] = item.ItemListPrice?.EffectiveDateTo;
                }


                dataTable.Rows.Add(row);
            }
            return dataTable;

        }
    }
}
