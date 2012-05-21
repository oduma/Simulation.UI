using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Sciendo.Core.Providers
{
    public interface ITopTotalsProvider
    {
        Func<int,int> ScoreRule {get;set;}

        void SaveTotalForItems(WeeklyTop topTotalForModel);

        IEnumerable<TopItem> GetTotalItems(ItemType itemType);

    }
}
