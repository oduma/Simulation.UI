using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core.Providers;

namespace Simulation.UI.Models
{
    public class WeekSelectionModel
    {

        public WeekSelectionModel(ItemType itemType)
        {
            ItemType  = itemType;
        }

        public IEnumerable<WeekModel> AvailableWeeks { get; set; }

        public WeeklyTop FirstWeekTop { get; set; }

        public ItemType ItemType { get; set; }

        public int NextWeekToProcess { get; set; }

        public IScoreAlgorythm CurrentAlgorythm { get; set; }

        public SettingsModel Settings { get; set; }

    }
}