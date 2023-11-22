using EHealth.ManageItemLists.Application.DoctorFees.UHIA.Queries;
using EHealth.ManageItemLists.Application.Helpers;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Queries.Handler
{
    public class CreateTemplateFacilityUHIASearchQueryHandler : IRequestHandler<CreateTemplateFacilityUHIASearchQuery, DataTable>
    {
        private readonly IMediator _mediator;

        public CreateTemplateFacilityUHIASearchQueryHandler(IMediator mediator)
        {

            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateFacilityUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var facilityUHIASearchQuery = new FacilityUHIASearchQuery();
            facilityUHIASearchQuery.ItemListId = request.ItemListId;
            facilityUHIASearchQuery.PageNo = request.ItemListId;
            facilityUHIASearchQuery.PageSize = request.ItemListId;
            facilityUHIASearchQuery.EnablePagination = false;

            var res = await _mediator.Send(facilityUHIASearchQuery);
            DataTable dataTable = new DataTable("excel");
            
            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("الوصف انجليزي");
                dataTable.Columns.Add("الوصف عربي");
                dataTable.Columns.Add("نسب الاشغال");
                dataTable.Columns.Add("معدلات التشغيل");
                dataTable.Columns.Add("أيام عمل الوحدة في الشهر");
                dataTable.Columns.Add("الفئه الاساسيه انجليزي");
                dataTable.Columns.Add("الفئه الاساسيه عربي");
                dataTable.Columns.Add("الفئه الفرعيه انجليزي");
                dataTable.Columns.Add("الفئه الفرعيه عربي");
                dataTable.Columns.Add("البيان تاريخ التفعيل من");
                dataTable.Columns.Add("البيان تاريخ التفعيل الي");
            }
            else
            {
                dataTable.Columns.Add("eHealthCode");
                dataTable.Columns.Add("Descriptor En");
                dataTable.Columns.Add("Descriptor Ar");
                dataTable.Columns.Add("Occupancy rate");
                dataTable.Columns.Add("Operating rate in hours per day");
                dataTable.Columns.Add("Operating days per month");
                dataTable.Columns.Add("Category En");
                dataTable.Columns.Add("Category Ar");
                dataTable.Columns.Add("SubCategory En");
                dataTable.Columns.Add("SubCategory Ar");
                dataTable.Columns.Add("Data-Effective Date from");
                dataTable.Columns.Add("Data-Effective Date to");
            }



            foreach (var item in res.Data)
            {
                DataRow row = dataTable.NewRow();
                
                if (request.Lang.ToLower() == "ar")
                {
                    row["كود أي هيلث"] = item.EHealthCode;
                    row["الوصف انجليزي"] = item.DescriptorEn;
                    row["الوصف عربي"] = item.DescriptorAr;
                    row["نسب الاشغال"] = item.OccupancyRate;
                    row["معدلات التشغيل"] = item.OperatingRateInHoursPerDay;
                    row["أيام عمل الوحدة في الشهر"] = item.OperatingDaysPerMonth;
                    row["الفئه الاساسيه انجليزي"] = item.Category?.CategoryEn;
                    row["الفئه الاساسيه عربي"] = item.Category?.CategoryAr;
                    row["الفئه الفرعيه انجليزي"] = item.SubCategory?.SubCategoryEn;
                    row["الفئه الفرعيه عربي"] = item.SubCategory?.SubCategoryAr;
                    row["البيان تاريخ التفعيل من"] = item.DataEffectiveDateFrom;
                    row["البيان تاريخ التفعيل الي"] = item.DataEffectiveDateTo;
                }
                else
                {
                    row["eHealthCode"] = item.EHealthCode;
                    row["Descriptor En"] = item.DescriptorEn;
                    row["Descriptor Ar"] = item.DescriptorAr;
                    row["Occupancy rate"] = item.OccupancyRate;
                    row["Operating rate in hours per day"] = item.OperatingRateInHoursPerDay;
                    row["Operating days per month"] = item.OperatingDaysPerMonth;
                    row["Category En"] = item.Category?.CategoryEn;
                    row["Category Ar"] = item.Category?.CategoryAr;
                    row["SubCategory En"] = item.SubCategory?.SubCategoryEn;
                    row["SubCategory Ar"] = item.SubCategory?.SubCategoryEn;
                    row["Data-Effective Date from"] = item.DataEffectiveDateFrom;
                    row["Data-Effective Date to"] = item.DataEffectiveDateTo;
                }

        

                dataTable.Rows.Add(row);
            }
            return dataTable;

        }
    }
    
}
