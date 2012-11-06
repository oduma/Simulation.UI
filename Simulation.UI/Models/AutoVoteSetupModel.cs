using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.UI.Models
{
    public class AutoVoteSetupModel
    {
        public IEnumerable<Settings> SettingsCollection { get; set; }
    }
}