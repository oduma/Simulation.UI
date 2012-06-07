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
        WeeklyTop GetTopByWeek(int weekNo, int topLength, ItemType itemType);

        [CacheKey(true,"lastWeekNo")]
        IEnumerable<Week> GetAvailableWeeks(int lastWeekNo);
    }
}
