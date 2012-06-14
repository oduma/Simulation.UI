using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;
using System.Configuration;
using Sciendo.Core;

namespace Simulation.DataAccess.FileSystem
{
    public class XmlSimulationCRUD: ISimulationCRUD
    {
        public List<CurrentScoreAlgorythm>  ListCurrentScoreAlgorythms()
        {
            var fileFullPath = ConfigurationManager.AppSettings["SettingsFile"];
            return Utility.DeserializeFromFile<CurrentScoreAlgorythm>(fileFullPath);
        }



        public void SaveCurrentScoreAlgorythm(CurrentScoreAlgorythm currentScoreAlgorythm)
        {
            var existingSettings = ListCurrentScoreAlgorythms();
            if (existingSettings.FirstOrDefault(r => r.ItemType == currentScoreAlgorythm.ItemType) != null)
                existingSettings.First(r => r.ItemType == currentScoreAlgorythm.ItemType).Name = currentScoreAlgorythm.Name;
            else
                existingSettings.Add(currentScoreAlgorythm);

            var fileFullPath = ConfigurationManager.AppSettings["SettingsFile"];

            Utility.SerializeToFile(existingSettings, fileFullPath);
        }


        public List<WeekSummary> GetTopProcessed()
        {

            var fileFullPath = ConfigurationManager.AppSettings["TopRecordFile"];
            return Utility.DeserializeFromFile<WeekSummary>(fileFullPath);
        }


        public void SaveRecordedWeeks(List<WeekSummary> weekSummaries)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TopRecordFile"];
            Utility.SerializeToFile(weekSummaries, fileFullPath);
        }


        public List<TopItem> ListTotalItems(ItemType itemType)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TotalsLocation"] + @"\" + itemType.ToString() + ".xml";
            return Utility.DeserializeFromFile<TopItem>(fileFullPath);
        }




        public void SaveTotalItems(ItemType itemType, List<TopItem> topItems)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TotalsLocation"] + @"\" + itemType.ToString() + ".xml";
            Utility.SerializeToFile(topItems, fileFullPath);
        }
    }
}
