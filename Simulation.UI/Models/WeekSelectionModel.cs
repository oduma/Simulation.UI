using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.UI.Models
{
    public class WeekSelectionModel
    {

        public WeekSelectionModel(ItemType itemType)
        {
            ItemType  = itemType;
        }

        public IEnumerable<Week> AvailableWeeks { get; set; }

        public WeeklyTop FirstWeekTop { get; set; }

        public ItemType ItemType { get; set; }

        public int NextWeekToProcess { get; set; }

        public ScoreAlgorythm CurrentAlgorythm { get; set; }

        public SettingsModel Settings { get; set; }

    }
}