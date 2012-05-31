using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core;

namespace Simulation.LastFmDataProvider
{
    public class TopProvider:ITopProvider
    {
        public WeeklyTop GetTopByWeek(int weekNo, int topLength, ItemType itemType)
        {
            throw new NotImplementedException();
        }

        public static string ApiKey { get { return "5e625305596ba928b8d8664bd2a95b08"; } }

        public IEnumerable<Week> GetAvailableWeeks()
        {
            throw new NotImplementedException();
        }
    }
}
