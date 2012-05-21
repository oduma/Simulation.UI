using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation.UI.Models
{
    public class WeekSelectionModel
    {

        public WeekSelectionModel(string typeOfSelection)
        {
            TypeOfSelection  = typeOfSelection;
        }
        public IEnumerable<Week> AvailableWeeks { get; set; }

        public WeeklyTopModel FirstWeekTop { get; set; }

        public string TypeOfSelection { get; set; }

        public int NextWeekToProcess { get; set; }

    }
}