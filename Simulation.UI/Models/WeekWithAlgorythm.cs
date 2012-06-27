using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.UI.Models
{
    public class WeekWithAlgorythm
    {
        public int WeekNo { get; set; }

        public string StartingFrom { get; set; }

        public string EndingIn { get; set; }

        public string ScoreAlgorythmName { get; set; }

        public ItemType ItemType { get; set; }
    }
}