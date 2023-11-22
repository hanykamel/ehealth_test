using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Queries;
using EHealth.ManageItemLists.Application.Helpers;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System.Data;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Queries.Handler
{
    public class TemplateDevicesAndAssetUHIASearchQueryHandler : IRequestHandler<CreateTemplateDevicesAndAssetUHIASearchQuery, DataTable>
    {
        private readonly IMediator _mediator;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        public TemplateDevicesAndAssetUHIASearchQueryHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository, IMediator mediator)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _mediator = mediator;
        }
        public async Task<DataTable> Handle(CreateTemplateDevicesAndAssetUHIASearchQuery request, CancellationToken cancellationToken)
        {
            var devicesAndAssetsUHIASearchQuery = new DevicesAndAssetsUHIASearchQuery();
            devicesAndAssetsUHIASearchQuery.ItemListId = request.ItemListId;
            devicesAndAssetsUHIASearchQuery.PageNo = request.PageNo;
            devicesAndAssetsUHIASearchQuery.PageSize = request.PageSize;
            devicesAndAssetsUHIASearchQuery.EnablePagination = false;
            var res = await _mediator.Send(devicesAndAssetsUHIASearchQuery);

            DataTable dataTable = new DataTable("excel");
            

            if (request.Lang.ToLower() == "ar")
            {
                dataTable.Columns.Add("كود أي هيلث");
                dataTable.Columns.Add("الوصف انجليزي");
                dataTable.Columns.Add("الوصف عربي");
                dataTable.Columns.Add("الغرفة انجليزي");
                dataTable.Columns.Add("الغرفة عربي");
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
                dataTable.Columns.Add("Descriptor En");
                dataTable.Columns.Add("Descriptor Ar");
                dataTable.Columns.Add("UnitRoom En");
                dataTable.Columns.Add("UnitRoom Ar");
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
                    row["الوصف انجليزي"] = item.DescriptorEn;
                    row["الوصف عربي"] = item.DescriptorAr;
                    row["الغرفة انجليزي"] = item.UnitRoom?.NameEN;
                    row["الغرفة عربي"] = item.UnitRoom?.NameAr;
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
                    row["Descriptor En"] = item.DescriptorEn;
                    row["Descriptor Ar"] = item.DescriptorAr;
                    row["UnitRoom En"] = item.UnitRoom?.NameEN;
                    row["UnitRoom Ar"] = item.UnitRoom?.NameAr;
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
