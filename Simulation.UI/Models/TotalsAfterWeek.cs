using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.UI.Models
{
    public class TotalsAfterWeek
    {
        public int WeekNo { get; set; }

        public IEnumerable<TopItem> TopItems { get; set; }
    }
}