using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using Simulation.DataAccess;
using Sciendo.Core;

namespace Simulation.DataProviders
{
    public class TopRecordProvider : ITopRecordProvider
    {
        ISimulationCRUD _simulationCRUD;
        public TopRecordProvider()
        {
            _simulationCRUD = ClientFactory.GetClient<ISimulationCRUD>();
        }

        public IEnumerable<WeekSummary> GetTopProcessed()
        {
            return _simulationCRUD.GetTopProcessed();
        }

        protected bool IsWeekProcessed(IEnumerable<WeekSummary> topRecordedItems, int weekNo, ItemType itemType)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.ItemType.ToString().ToLower() == itemType.ToString().ToLower()) != null;
        }



        public void SaveTotalForItems(WeeklyTop topTotalForModel, Func<int, int> ScoreRule)
        {
            List<TopItem> existingItems = GetTotalItems(topTotalForModel.ItemType);

            foreach (TopItem topItem in topTotalForModel.TopItems)
            {
                var itemScore = ScoreRule(topItem.Rank);
                if (itemScore > 0)
                {
                    TopItem existingItem = existingItems.FirstOrDefault(i => i.ItemName == topItem.ItemName);
                    if (existingItem == null)
                    {
                        existingItem = new TopItem { ItemName = topItem.ItemName, EntryWeek = topTotalForModel.WeekNo, Score = itemScore, ItemType = topTotalForModel.ItemType, NumberOfPlays = topItem.NumberOfPlays };
                        existingItems.Add(existingItem);
                    }
                    else
                    {
                        existingItem.Score += itemScore;
                        existingItem.NumberOfPlays += topItem.NumberOfPlays;
                    }
                    RecalculateRanks(existingItems);
                }
                else
                    break;
            }
            _simulationCRUD.SaveTotalItems(topTotalForModel.ItemType, existingItems);

            List<WeekSummary> topRecordedItems = GetTopProcessed().ToList();
            topRecordedItems.AddRange(new List<WeekSummary> { new WeekSummary { ItemType = topTotalForModel.ItemType, WeekNo = topTotalForModel.WeekNo } });
            _simulationCRUD.SaveRecordedWeeks(topRecordedItems);

        }

        private void RecalculateRanks(List<TopItem> existingItems)
        {
            existingItems.Sort(new TopItemComparer());
            for (int i = 0; i < existingItems.Count; i++)
                existingItems[i].Rank = i + 1;
        }

        public List<TopItem> GetTotalItems(ItemType itemType)
        {
            return _simulationCRUD.ListTotalItems(itemType);
        }

        public void ClearAll(ItemType itemType)
        {
            List<TopItem> topItems = GetTotalItems(itemType);
            topItems.Clear();
            _simulationCRUD.SaveTotalItems(itemType,topItems);

            List<WeekSummary> remainingListSummaries = GetTopProcessed().Where(w => w.ItemType != itemType).ToList();
            _simulationCRUD.SaveRecordedWeeks(remainingListSummaries);
        }
    }
}
