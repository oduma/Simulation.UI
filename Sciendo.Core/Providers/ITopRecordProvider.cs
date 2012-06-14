using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Sciendo.Core.Providers
{
    public interface ITopRecordProvider
    {
        IEnumerable<WeekSummary> GetTopProcessed();

        void SaveTotalForItems(WeeklyTop topTotalForModel, Func<int, int> ScoreRule);

        List<TopItem> GetTotalItems(ItemType itemType);

        void ClearAll(ItemType itemType);

    }
}
