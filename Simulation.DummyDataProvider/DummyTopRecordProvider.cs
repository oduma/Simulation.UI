using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace Simulation.DummyDataProvider
{
    public class DummyTopRecordProvider:ITopRecordProvider
    {
        public IEnumerable<WeekSummary> GetTopProcessed()
        {
            var fileFullPath = ConfigurationManager.AppSettings["TopRecordFile"];
            if (!File.Exists(fileFullPath))
                return new List<WeekSummary>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WeekSummary>));
                return xmlSerializer.Deserialize(fs) as List<WeekSummary>;
            }
        }

        public void RecordSavedWeeks(List<WeekSummary> weekSumaries)
        {
            List<WeekSummary> topRecordedItems = GetTopProcessed().ToList();
            topRecordedItems.AddRange(weekSumaries);
            Save(topRecordedItems);
        }

        private void Save(List<WeekSummary> weekSummaries)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TopRecordFile"];

            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<WeekSummary>));
                serializer.Serialize(fs, weekSummaries);
                fs.Flush();
                fs.Close();
            }


        }

        public void ClearRecords(ItemType itemType)
        {
            List<WeekSummary> remainingListSummaries = GetTopProcessed().Where(w => w.ItemType != itemType).ToList();
            Save(remainingListSummaries);
        }

        protected bool IsWeekProcessed(IEnumerable<WeekSummary> topRecordedItems, int weekNo, ItemType itemType)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.ItemType.ToString().ToLower() == itemType.ToString().ToLower()) != null;
        }

    }
}
