using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using System.Configuration;
using Sciendo.Core.Providers.DataTypes;
using System.IO;
using System.Xml.Serialization;
using Simulation.LastFmDataProvider.DataTypes;
using Simulation.DummyDataProvider;

namespace Simulation.LastFmDataProvider
{
    public class DummyLfmTopProvider : DummyTopRecordProvider,ITopProvider
    {
        public WeeklyTop GetTopByWeek(int weekNo, int topLength, ItemType itemType)
        {
            WeeklyTop weeklyTop = new WeeklyTop();
            weeklyTop.WeekNo = weekNo;
            weeklyTop.TopItems = GetTopItems(topLength, itemType);
            weeklyTop.TopProcessed = IsWeekProcessed(GetTopProcessed(), weekNo, itemType);
            weeklyTop.ItemType = itemType;
            return weeklyTop;
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


        public IEnumerable<Week> GetAvailableWeeks()
        {
            
            var fileFullPath = ConfigurationManager.AppSettings["WeeksDummyFile"];

            if (!File.Exists(fileFullPath))
                return new List<Week>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LfmGetWeekChartlistResponse));
                return (xmlSerializer.Deserialize(fs) as LfmGetWeekChartlistResponse).ChartWeeks.TransformToWeeks();
            }
        }
    }
}
