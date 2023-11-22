using EHealth.ManageItemLists.Application.Drugs.UHIA.Queries;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.ItemLists.Queries.Handlers
{
    public class CreateTemplateItemListSearchQueryHandler : IRequestHandler<CreateTemplateItemListSearchQuery, DataTable>
    {
        private readonly IMediator _mediator;
        public CreateTemplateItemListSearchQueryHandler(IMediator mediator)
        {

            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateItemListSearchQuery request, CancellationToken cancellationToken)
        {
            var searchItemListQuery = new SearchItemListQuery();
            searchItemListQuery.EnablePagination = false;
            var res = await _mediator.Send(searchItemListQuery);
            DataTable dataTable = new DataTable("excel");
            
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("رمز القائمة");
                dataTable.Columns.Add("النوع");
                dataTable.Columns.Add("اسم القائمة انجليزي");
                dataTable.Columns.Add("اسم القائمة عربي");
                dataTable.Columns.Add("عدد العناصر");
                dataTable.Columns.Add("اخر تحديث");
                dataTable.Columns.Add("محدث بواسطة");
            }
            else
            {
                dataTable.Columns.Add("ListCode");
                dataTable.Columns.Add("Type");
                dataTable.Columns.Add("ListNameEn");
                dataTable.Columns.Add("ListNameAr");
                dataTable.Columns.Add("NumberOfItems");
                dataTable.Columns.Add("LastUpdate");
                dataTable.Columns.Add("UpdatedBy");
            }
            

            foreach (var item in res.Data)
            {
                DataRow row = dataTable.NewRow();
                
                if (request.Lang.ToLower() == "ar")
                {
                    row["رمز القائمة"] = item.Code;
                    row["النوع"] = item.itemListType.NameEN + item.itemListSubtype.NameEN;
                    row["اسم القائمة انجليزي"] = item.NameEN;
                    row["اسم القائمة عربي"] = item.NameAr;
                    row["عدد العناصر"] = item.ItemCounts;
                    row["اخر تحديث"] = item.UpdatedOn;
                    row["محدث بواسطة"] = item.UpdatedBy;
                }
                else
                {
                    row["ListCode"] = item.Code;
                    row["Type"] = item.itemListType.NameEN + item.itemListSubtype.NameEN;
                    row["ListNameEn"] = item.NameEN;
                    row["ListNameAr"] = item.NameAr;
                    row["NumberOfItems"] = item.ItemCounts;
                    row["LastUpdate"] = item.UpdatedOn;
                    row["UpdatedBy"] = item.UpdatedBy;
                }
               

                dataTable.Rows.Add(row);
            }

            return dataTable;


        }
    }
}
