using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using Sciendo.Core;

namespace Simulation.DummyDataProvider
{
    public class DummyTopTotalsProvider:DummyTopRecordProvider,ITopTotalsProvider
    {
        public DummyTopTotalsProvider()
        {
            ScoreRule = ScoreItem;
        }

        public Func<int, int> ScoreRule { get; set; }

        public void SaveTotalForItems(WeeklyTop topTotalForModel)
        {
            List<TopItem> existingItems = GetTotalItems(topTotalForModel.ItemType).ToList();

            foreach (TopItem topItem in topTotalForModel.TopItems)
            {
                var itemScore = ScoreRule(topItem.Rank);
                if (itemScore > 0)
                {
                    TopItem existingItem = existingItems.FirstOrDefault(i => i.ItemName == topItem.ItemName);
                    if (existingItem == null)
                    {
                        existingItem = new TopItem { ItemName = topItem.ItemName, EntryWeek = topTotalForModel.WeekNo, Score = itemScore,ItemType=topTotalForModel.ItemType };
                        existingItems.Add(existingItem);
                    }
                    else
                        existingItem.Score += itemScore;
                    RecalculatePositions(existingItems);
                }
                else
                    break;
            }
            TrySaveItems(existingItems);
            RecordSavedWeek(topTotalForModel);
        }

        private void RecordSavedWeek(WeeklyTop weeklyTop)
        {
            List<WeekSummary> topRecordedItems = GetTopProcessed().ToList();
            topRecordedItems.Add(new WeekSummary {ItemType=weeklyTop.ItemType, WeekNo = weeklyTop.WeekNo });
            var fileFullPath = ConfigurationManager.AppSettings["TopRecordFile"];

            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<WeekSummary>));
                serializer.Serialize(fs, topRecordedItems);
                fs.Flush();
                fs.Close();
            }

        }

        private void TrySaveItems(List<TopItem> existingItems)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TotalsLocation"] + @"\" + existingItems[0].ItemType.ToString() + ".xml";

            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TopItem>));
                serializer.Serialize(fs, existingItems);
                fs.Flush();
                fs.Close();
            }
        }

        private void RecalculatePositions(List<TopItem> existingItems)
        {
            existingItems.Sort(new TopItemComparer());
            for (int i = 0; i < existingItems.Count; i++)
                existingItems[i].Position = i + 1;
        }

        private int ScoreItem(int rank)
        {
            switch (rank)
            {
                case 1:
                    return 3;
                case 2:
                    return 2;
                case 3:
                    return 1;
                default:
                    return 0;
            }
        }

        public IEnumerable<TopItem> GetTotalItems(ItemType itemType)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TotalsLocation"] + @"\" + itemType.ToString() + ".xml";
            if (!File.Exists(fileFullPath))
                return new List<TopItem>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TopItem>));
                return xmlSerializer.Deserialize(fs) as List<TopItem>;
            }
        }
    }
}
