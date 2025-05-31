using Core.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Common
{
    public class PaginationModel<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalPge { get; set; }
        public int CurrentPge { get; set; }
        public int ItemPerPage { get; set; }
    }
}
