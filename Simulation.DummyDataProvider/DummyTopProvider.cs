using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.DummyDataProvider
{
    public class DummyTopProvider:DummyTopRecordProvider,ITopProvider
    {
        public WeeklyTop GetTopByWeek(Week requestedWeek, int topLength, ItemType itemType)
        {
            throw new NotImplementedException();
        }

        private bool IsWeekProcessed(IEnumerable<WeekSummary> topRecordedItems, int weekNo, ItemType itemType)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.ItemType.ToString().ToLower() == itemType.ToString().ToLower()) != null;
        }

        private IEnumerable<TopItem> GetTopItems(int maxRank, ItemType itemType)
        {
            for (int i = 1; i < maxRank + 1; i++)
                yield return new TopItem { Rank = i, Position = i, ItemName = itemType.ToString() + i, NumberOfPlays = 20 - i };
        }


        public List<Week> GetAvailableWeeks(int lastWeekNo)
        {
            throw new NotImplementedException();
        }

    }
}
