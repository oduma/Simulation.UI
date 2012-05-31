using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;
using Simulation.LastFmDataProvider.DataTypes;

namespace Simulation.LastFmDataProvider
{
    public static class LfmResponseExtensions
    {
        public static List<Week> TransformToWeeks(this List<ChartWeek> chartWeeks)
        {
            int i = 1;
            var weeks = chartWeeks
                .Where(wr => new DateTime(1970, 1, 1).AddSeconds(wr.To).Year == DateTime.Now.Year)
                .Select(wr =>
                new Week
                {
                    EndingIn = new DateTime(1970, 1, 1).AddSeconds(wr.To),
                    StartingFrom = new DateTime(1970, 1, 1).AddSeconds(wr.From),
                    WeekNo = i++
                });
            return weeks.ToList();
    
        }
    }
}
