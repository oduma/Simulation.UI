using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core.CacheManager;

namespace Sciendo.Core.Providers
{
    public interface ITopProvider
    {
        [CacheKey(true,"itemType","requestedWeek")]
        WeeklyTop GetTopByWeek(Week requestedWeek, int topLength, ItemType itemType);

        [CacheKey(false,"lastWeekNo")]
        List<Week> GetAvailableWeeks(int lastWeekNo);
    }
}
