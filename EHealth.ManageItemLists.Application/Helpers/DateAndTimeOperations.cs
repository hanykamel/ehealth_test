using EHealth.ManageItemLists.Application.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Helpers
{
    public class DateAndTimeOperations
    {

        public static bool DoesNotOverlap(IEnumerable<DateRangeDto> DateRangeLst)
        {
            DateTime endPrior = DateTime.MinValue;
            foreach (var item in DateRangeLst.OrderBy(x => x.Start))
            {
                if (item.Start > item.End)
                    return false;
                if (item.Start <= endPrior)
                    return false;

                if (!item.End.HasValue)
                {
                    endPrior = DateTime.MaxValue;
                }
                else
                {
                    endPrior = (DateTime)item.End;
                }
            }
            return true;
        }

        public static bool DoesNotOverlap(DateRangeDto item1 , DateRangeDto item2)
        {
            List<DateRangeDto> DateRangeLst = new List<DateRangeDto>();
            DateRangeLst.Add(item1);
            DateRangeLst.Add(item2);

            DateTime endPrior = DateTime.MinValue;
            foreach (var item in DateRangeLst.OrderBy(x => x.Start))
            {
                if (item.Start > item.End)
                    return false;
                if (item.Start <= endPrior)
                    return false;

                if (!item.End.HasValue)
                {
                    endPrior = DateTime.MaxValue;
                }
                else
                {
                    endPrior = (DateTime)item.End;
                }
            }
            return true;
        }

    }
}
