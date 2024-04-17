using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightAcademy.ApplicationLayer.Models
{
    public class Pager<T>
    {
        public IEnumerable<T> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public string ParmId { get; private set; }

        public Pager(IEnumerable<T> items, int pageNumber, int pageSize,string paramId="")
        {
            TotalItems = items.Count();
            PageSize = pageSize;
            PageNumber = pageNumber;
            ParmId=paramId;
            Items = items.Skip((PageNumber - 1) * PageSize)
                         .Take(PageSize)
                         .ToList();
        }
    }

}
