using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core.Providers;

namespace Simulation.UI.Models
{
    public class Settings
    {
        public IEnumerable<IScoreAlgorythm> ScoreAlgorythms { get; set; }

        public ItemType ItemType { get; set; }

    }
}