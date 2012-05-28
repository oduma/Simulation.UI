using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Sciendo.Core.Providers
{
    public interface ITopTotalsProvider
    {
        void SaveTotalForItems(WeeklyTop topTotalForModel, Func<int, int> ScoreRule);

        IEnumerable<TopItem> GetTotalItems(ItemType itemType);

        void ClearAll(ItemType itemType);

    }
}
