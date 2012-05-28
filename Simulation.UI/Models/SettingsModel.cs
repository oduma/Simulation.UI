using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.UI.Models
{
    public class SettingsModel
    {
        public IEnumerable<ScoreAlgorythm> ScoreAlgorythms { get; set; }

        public TopProviderSettings TopProviderSettings { get; set; }

        

    }
}