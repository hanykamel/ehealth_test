using EHealth.ManageItemLists.Application.Helpers;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Queries.Handlers
{
    public class CreateTemplateProcedureICHISearchHandler : IRequestHandler<CreateTemplateProcedureICHISearchQuery, DataTable>
    {
        private readonly IMediator _mediator;

        public CreateTemplateProcedureICHISearchHandler(IMediator mediator)
        {

            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateProcedureICHISearchQuery request, CancellationToken cancellationToken)
        {
            var procedureICHISearchQuery = new ProcedureICHISearchQuery();
            procedureICHISearchQuery.ItemListId = request.ItemListId;
            procedureICHISearchQuery.PageSize = request.PageSize;
            procedureICHISearchQuery.PageNo = request.PageNo;
            procedureICHISearchQuery.EnablePagination = false;


            var res = await _mediator.Send(procedureICHISearchQuery);
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
                dataTable.Columns.Add("التخصص المحلي انجليزي");
                dataTable.Columns.Add("التخصص المحلي عربي");
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
                dataTable.Columns.Add("Title En");
                dataTable.Columns.Add("Title Ar");
                dataTable.Columns.Add("Service Category En");
                dataTable.Columns.Add("Service Category Ar");
                dataTable.Columns.Add("SubCategory En");
                dataTable.Columns.Add("SubCategory Ar");
                dataTable.Columns.Add("Local Speciality En");
                dataTable.Columns.Add("Local Speciality Ar");
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
                    row["الوصف انجليزي"] = item.TitleEn;
                    row["الوصف عربي"] = item.TitleAr;
                    row["الفئه الاساسيه انجليزي"] = item.Category?.CategoryEn;
                    row["الفئه الاساسيه عربي"] = item.Category?.CategoryAr;
                    row["الفئه الفرعيه انجليزي"] = item.SubCategory?.SubCategoryEn;
                    row["الفئه الفرعيه عربي"] = item.SubCategory?.SubCategoryAr;
                    row["التخصص المحلي انجليزي"] = item.LocalSpecialtyDepartment?.LocalSpecialityEn;
                    row["التخصص المحلي عربي"] = item.LocalSpecialtyDepartment?.LocalSpecialityAr;
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
                    row["Title En"] = item.TitleEn;
                    row["Title Ar"] = item.TitleAr;
                    row["Service Category En"] = item.Category?.CategoryEn;
                    row["Service Category Ar"] = item.Category?.CategoryAr;
                    row["SubCategory En"] = item.SubCategory?.SubCategoryEn;
                    row["SubCategory Ar"] = item.SubCategory?.SubCategoryEn;
                    row["Local Speciality En"] = item.LocalSpecialtyDepartment?.LocalSpecialityEn;
                    row["Local Speciality Ar"] = item.LocalSpecialtyDepartment?.LocalSpecialityAr;
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
