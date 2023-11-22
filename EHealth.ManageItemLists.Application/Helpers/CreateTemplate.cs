using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using System.Net.Http;
using System.Net;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs;
using System.Data;
using System.Reflection;

namespace EHealth.ManageItemLists.Application.Helpers
{
    public class CreateTemplate
    {
        private DataTable GenerateExcel(IEnumerable<ServiceUHIADto> services)
        {
            DataTable dataTable = new DataTable("Service");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name")
            });

            foreach (var service in services)
            {
                dataTable.Rows.Add(service.Id, service.ShortDescAr);
            }

            return dataTable;

        }

        public static DataTable ToDataTable<T>(IList<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
