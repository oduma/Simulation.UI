using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation.UI.Models
{
    public class TopWeekRequest:WeekSummary
    {
        public string TypeOfSelection { get; set; }
    }
}