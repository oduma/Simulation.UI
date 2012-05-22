using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simulation.UI.Models
{
    public class SettingsModel
    {
        IEnumerable<ScoreAlgorythm> ScoreAlgorythms { get; set; }
    }
}