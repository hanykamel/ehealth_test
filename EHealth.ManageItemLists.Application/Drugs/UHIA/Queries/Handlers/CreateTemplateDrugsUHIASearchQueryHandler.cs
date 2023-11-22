using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Application.Helpers;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Queries.Handlers
{
    public class CreateTemplateDrugsUHIASearchQueryHandler : IRequestHandler<CreateTemplateDrugsUHIASearchQuery, DataTable>
    {
        private readonly IMediator _mediator;

        public CreateTemplateDrugsUHIASearchQueryHandler(IMediator mediator)
        {

            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateDrugsUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var drugUHIASearchQuery = new DrugUHIASearchQuery();
            drugUHIASearchQuery.ItemListId = request.ItemListId;
            drugUHIASearchQuery.PageNo = request.PageNo;
            drugUHIASearchQuery.PageSize = request.PageSize;
            drugUHIASearchQuery.EnablePagination = false;


            var res = await _mediator.Send(drugUHIASearchQuery);
            DataTable dataTable = new DataTable("excel");
        
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("كود الدواء المحلي");
                dataTable.Columns.Add("المواد الفعالة");
                dataTable.Columns.Add("الاسم التجاري");
                dataTable.Columns.Add("الشكل الدوائي");
                dataTable.Columns.Add("طريقة الاعطاء");
                dataTable.Columns.Add("الشركة المصنعه");
                dataTable.Columns.Add("صاحب ترخيص السوق");
                dataTable.Columns.Add("نوع التسجيل انجليزي");
                dataTable.Columns.Add("نوع التسجيل عربي");
                dataTable.Columns.Add("نوع العبوة انجليزي");
                dataTable.Columns.Add("نوع العبوة عربي");
                dataTable.Columns.Add("الوحدة الاساسيه انجليزي");
                dataTable.Columns.Add("الوحدة الاساسيه عربي");
                dataTable.Columns.Add("عدد الوحدة الاساسيه");
                dataTable.Columns.Add("الوحدة الفرعيه انجليزي");
                dataTable.Columns.Add("الوحدة الفرعيه عربي");
                dataTable.Columns.Add("عدد الوحدة الصغري");
                dataTable.Columns.Add("اجمالي عدد الوحدة الصغري");
                dataTable.Columns.Add("فئة التعويضات انجليزي");
                dataTable.Columns.Add("فئة التعويضات عربي");
                dataTable.Columns.Add("البيان تاريخ التفعيل من");
                dataTable.Columns.Add("البيان تاريخ التفعيل الي");
                dataTable.Columns.Add("سعر مشاركة التامين للوحدة الأساسية");
                dataTable.Columns.Add("سعر مشاركة التامين للعبوة الكاملة");
                dataTable.Columns.Add("سعر مشاركة التامين للوحدة الصغرى");
                dataTable.Columns.Add("السعر تاريخ التفعيل من");
                dataTable.Columns.Add("السعر تاريخ التفعيل الي");
            }
            else
            {
                dataTable.Columns.Add("eHealthCode");
                dataTable.Columns.Add("Local Drug Code");
                dataTable.Columns.Add("International non-proprietary name (INN)");
                dataTable.Columns.Add("Proprietary (Trade) name");
                dataTable.Columns.Add("Dosage Form");
                dataTable.Columns.Add("Route of administration");
                dataTable.Columns.Add("Manufacturer");
                dataTable.Columns.Add("Market authorization holder");
                dataTable.Columns.Add("Registration Type En");
                dataTable.Columns.Add("Registration Type Ar");
                dataTable.Columns.Add("Package Type En");
                dataTable.Columns.Add("Package Type Ar");
                dataTable.Columns.Add("Main Unit En");
                dataTable.Columns.Add("Main Unit Ar");
                dataTable.Columns.Add("Number of main units");
                dataTable.Columns.Add("Sub-unit En");
                dataTable.Columns.Add("Sub-unit Ar");
                dataTable.Columns.Add("Number of Subunit Per main unit");
                dataTable.Columns.Add("Total number of subunits in the pack");
                dataTable.Columns.Add("Reimbursement category En");
                dataTable.Columns.Add("Reimbursement category Ar");
                dataTable.Columns.Add("Data-Effective Date from");
                dataTable.Columns.Add("Data-Effective Date to");
                dataTable.Columns.Add("Main Unit Price");
                dataTable.Columns.Add("Full Pack Price");
                dataTable.Columns.Add("Subunit Price");
                dataTable.Columns.Add("Price Data-Effective Date from");
                dataTable.Columns.Add("Price Data-Effective Date to");
            }


        
            foreach (var item in res.Data)
            {
                DataRow row = dataTable.NewRow();
             
                if (request.Lang.ToLower() == "ar")
                {
                    row["كود أي هيلث"] = item.EHealthCode;
                    row["كود الدواء المحلي"] = item.LocalDrugCode;
                    row["المواد الفعالة"] = item.InternationalNonProprietaryName;
                    row["الاسم التجاري"] = item.ProprietaryName;
                    row["الشكل الدوائي"] = item.DosageForm;
                    row["طريقة الاعطاء"] = item.RouteOfAdministration;
                    row["الشركة المصنعه"] = item.Manufacturer;
                    row["صاحب ترخيص السوق"] = item.MarketAuthorizationHolder;
                    row["نوع التسجيل انجليزي"] = item.RegistrationType?.RegistrationTypeEn;
                    row["نوع التسجيل عربي"] = item.RegistrationType?.RegistrationTypeAr;
                    row["نوع العبوة انجليزي"] = item.DrugsPackageType?.NameEn;
                    row["نوع العبوة عربي"] = item.DrugsPackageType?.NameAr;
                    row["الوحدة الاساسيه انجليزي"] = item.MainUnit?.UnitEn;
                    row["الوحدة الاساسيه عربي"] = item.MainUnit?.UnitAr;
                    row["عدد الوحدة الاساسيه"] = item.NumberOfMainUnit;
                    row["الوحدة الفرعيه انجليزي"] = item.SubUnit?.UnitEn;
                    row["الوحدة الاساسيه عربي"] = item.SubUnit?.UnitAr;
                    row["عدد الوحدة الصغري"] = item.NumberOfSubunitPerMainUnit;
                    row["اجمالي عدد الوحدة الصغري"] = item.TotalNumberSubunitsOfPack;
                    row["فئة التعويضات انجليزي"] = item.ReimbursementCategory?.NameEn;
                    row["فئة التعويضات عربي"] = item.ReimbursementCategory?.NameAr;
                    row["البيان تاريخ التفعيل من"] = item.DataEffectiveDateFrom;
                    row["البيان تاريخ التفعيل الي"] = item.DataEffectiveDateTo;
                    row["سعر مشاركة التامين للوحدة الأساسية"] = item.DrugPrice?.MainUnitPrice;
                    row["سعر مشاركة التامين للعبوة الكاملة"] = item.DrugPrice?.FullPackPrice;
                    row["سعر مشاركة التامين للوحدة الصغرى"] = item.DrugPrice?.SubUnitPrice;
                    row["السعر تاريخ التفعيل من"] = item.DrugPrice?.EffectiveDateFrom;
                    row["السعر تاريخ التفعيل الي"] = item.DrugPrice?.EffectiveDateTo;
                }
                else
                {
                    row["eHealthCode"] = item.EHealthCode;
                    row["Local Drug Code"] = item.LocalDrugCode;
                    row["International non-proprietary name (INN)"] = item.InternationalNonProprietaryName;
                    row["Proprietary (Trade) name"] = item.ProprietaryName;
                    row["Dosage Form"] = item.DosageForm;
                    row["Route of administration"] = item.RouteOfAdministration;
                    row["Manufacturer"] = item.Manufacturer;
                    row["Market authorization holder"] = item.MarketAuthorizationHolder;
                    row["Registration Type En"] = item.RegistrationType?.RegistrationTypeEn;
                    row["Registration Type Ar"] = item.RegistrationType?.RegistrationTypeAr;
                    row["Package Type En"] = item.DrugsPackageType?.NameEn;
                    row["Package Type Ar"] = item.DrugsPackageType?.NameAr;
                    row["Main Unit En"] = item.MainUnit?.UnitEn;
                    row["Main Unit Ar"] = item.MainUnit?.UnitAr;
                    row["Number of main units"] = item.NumberOfMainUnit;
                    row["Sub-unit En"] = item.SubUnit?.UnitEn;
                    row["Sub-unit Ar"] = item.SubUnit?.UnitAr;
                    row["Number of Subunit Per main unit"] = item.NumberOfSubunitPerMainUnit;
                    row["Total number of subunits in the pack"] = item.TotalNumberSubunitsOfPack;
                    row["Reimbursement category En"] = item.ReimbursementCategory?.NameEn;
                    row["Reimbursement category Ar"] = item.ReimbursementCategory?.NameAr;
                    row["Data-Effective Date from"] = item.DataEffectiveDateFrom;
                    row["Data-Effective Date to"] = item.DataEffectiveDateTo;
                    row["Main Unit Price"] = item.DrugPrice?.MainUnitPrice;
                    row["Full Pack Price"] = item.DrugPrice?.FullPackPrice;
                    row["Subunit Price"] = item.DrugPrice?.SubUnitPrice;
                    row["Price Data-Effective Date from"] = item.DrugPrice?.EffectiveDateFrom;
                    row["Price Data-Effective Date to"] = item.DrugPrice?.EffectiveDateTo;
                }

              


                dataTable.Rows.Add(row);
            }

            return dataTable;


        }
    }
}
