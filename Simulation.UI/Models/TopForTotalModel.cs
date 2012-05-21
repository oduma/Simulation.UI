using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation.UI.Models
{
    public class TopForTotalModel
    {
        public int WeekNo { get; set; }

        public IEnumerable<TopItem> TopItems { get; set; }
    }
}