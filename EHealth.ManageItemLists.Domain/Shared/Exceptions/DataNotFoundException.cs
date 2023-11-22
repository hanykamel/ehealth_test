using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public int StatusCode { get; set; } 
        public string? HttpResponseMessage { get; set; }
        public DataNotFoundException(string message = "The data was not found") : base(message)
        {
            HttpResponseMessage = message;
            StatusCode = 404;
        }
    }
}
