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
    }
}
