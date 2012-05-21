using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation.UI.Models
{
    public class WeeklyTopModel:WeekSummary
    {
        public IEnumerable<TopItem> TopItems { get; set; }
    }
}
