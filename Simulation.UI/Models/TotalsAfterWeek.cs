using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation.UI.Models
{
    public class TotalsAfterWeek
    {
        public int WeekNo { get; set; }

        public IEnumerable<TotalItem> TotalItems { get; set; }
    }
}