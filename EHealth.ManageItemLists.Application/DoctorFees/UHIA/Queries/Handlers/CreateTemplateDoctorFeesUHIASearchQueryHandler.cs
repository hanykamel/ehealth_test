using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries;
using EHealth.ManageItemLists.Application.Helpers;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries.Handlers
{
    public class CreateTemplateDoctorFeesUHIASearchQueryHandler : IRequestHandler<CreateTemplateDoctorFeesUHIASearchQuery, DataTable>
    {
        private readonly IMediator _mediator;

        public CreateTemplateDoctorFeesUHIASearchQueryHandler(IMediator mediator)
        {

            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateDoctorFeesUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var doctorFeesUHIASearchQuery = new DoctorFeesUHIASearchQuery();
            doctorFeesUHIASearchQuery.ItemListId = request.ItemListId;
            doctorFeesUHIASearchQuery.PageSize = request.PageSize;
            doctorFeesUHIASearchQuery.PageNo = request.PageNo;
            doctorFeesUHIASearchQuery.EnablePagination = false;


            var res = await _mediator.Send(doctorFeesUHIASearchQuery);
            DataTable dataTable = new DataTable("excel");
           
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("الوصف انجليزي");
                dataTable.Columns.Add("الوصف عربي");
                dataTable.Columns.Add("توصيف العمليه انجليزي");
                dataTable.Columns.Add("توصيف العمليه عربي");
                dataTable.Columns.Add("البيان تاريخ التفعيل من");
                dataTable.Columns.Add("البيان تاريخ التفعيل الي");
                dataTable.Columns.Add("اتعاب الطبيب");
                dataTable.Columns.Add("وحدة اتعاب الطبيب انجليزي");
                dataTable.Columns.Add("وحدة اتعاب الطبيب عربي");
                dataTable.Columns.Add("السعر تاريخ التفعيل من");
                dataTable.Columns.Add("السعر تاريخ التفعيل الي");
            }
            else
            {
                dataTable.Columns.Add("eHealthCode");
                dataTable.Columns.Add("Descriptor En");
                dataTable.Columns.Add("Descriptor Ar");
                dataTable.Columns.Add("ComplexityClassificationCode En");
                dataTable.Columns.Add("ComplexityClassificationCode Ar");
                dataTable.Columns.Add("Data-Effective Date from");
                dataTable.Columns.Add("Data-Effective Date to");
                dataTable.Columns.Add("Doctor Fees");
                dataTable.Columns.Add("Unit Doctor Fees En");
                dataTable.Columns.Add("Unit Doctor Fees Ar");
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
                    row["توصيف العمليه انجليزي"] = item.PackageComplexityClassification?.ComplexityEn;
                    row["توصيف العمليه عربي"] = item.PackageComplexityClassification?.ComplexityAr;
                    row["البيان تاريخ التفعيل من"] = item.DataEffectiveDateFrom;
                    row["البيان تاريخ التفعيل الي"] = item.DataEffectiveDateTo;
                    row["اتعاب الطبيب"] = item.ItemListPrices?.DoctorFees;
                    row["وحدة اتعاب الطبيب انجليزي"] = item.ItemListPrices?.UnitOfDoctorFees?.NameEN;
                    row["وحدة اتعاب الطبيب عربي"] = item.ItemListPrices?.UnitOfDoctorFees?.NameAr;
                    row["السعر تاريخ التفعيل من"] = item.ItemListPrices?.EffectiveDateFrom;
                    row["السعر تاريخ التفعيل الي"] = item.ItemListPrices?.EffectiveDateTo;
                }
                else
                {
                    row["eHealthCode"] = item.EHealthCode;
                    row["Descriptor En"] = item.DescriptorEn;
                    row["Descriptor Ar"] = item.DescriptorEn;
                    row["ComplexityClassificationCode En"] = item.PackageComplexityClassification?.ComplexityEn;
                    row["ComplexityClassificationCode Ar"] = item.PackageComplexityClassification?.ComplexityAr;
                    row["Data-Effective Date from"] = item.DataEffectiveDateFrom;
                    row["Data-Effective Date to"] = item.DataEffectiveDateTo;
                    row["Doctor Fees"] = item.ItemListPrices?.DoctorFees;
                    row["Unit Doctor Fees En"] = item.ItemListPrices?.UnitOfDoctorFees?.NameEN;
                    row["Unit Doctor Fees Ar"] = item.ItemListPrices?.UnitOfDoctorFees?.NameAr;
                    row["Price Data-Effective Date from"] = item.ItemListPrices?.EffectiveDateFrom;
                    row["Price Data-Effective Date to"] = item.ItemListPrices?.EffectiveDateTo;
                }

        

                dataTable.Rows.Add(row);
            }
            return dataTable;

        }
    }
}
