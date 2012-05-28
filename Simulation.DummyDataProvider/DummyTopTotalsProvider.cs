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

        public void SaveTotalForItems(WeeklyTop topTotalForModel,Func<int,int> ScoreRule)
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
            TrySaveItems(existingItems, topTotalForModel.ItemType);
            RecordSavedWeeks(new List<WeekSummary> { new WeekSummary { ItemType = topTotalForModel.ItemType, WeekNo = topTotalForModel.WeekNo } });
        }

        private void TrySaveItems(List<TopItem> existingItems, ItemType itemType)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TotalsLocation"] + @"\" + itemType.ToString() + ".xml";
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


        public void ClearAll(ItemType itemType)
        {
            List<TopItem> topItems = GetTotalItems(itemType).ToList();
            topItems.Clear();
            TrySaveItems(topItems, itemType);

            ClearRecords(itemType);
        }
    }
}
