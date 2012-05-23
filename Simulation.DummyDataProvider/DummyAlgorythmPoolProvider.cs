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
    public class DummyAlgorythmPoolProvider:IAlgorythmPoolProvider
    {
        public static int Rule321(int rank)
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

        public static int RuleF1(int rank)
        {
            switch (rank)
            {
                case 1:
                    return 10;
                case 2:
                    return 8;
                case 3:
                    return 6;
                case 4:
                    return 5;
                case 5:
                    return 4;
                case 6:
                    return 3;
                case 7:
                    return 2;
                case 8:
                    return 1;
                default:
                    return 0;
            }
        }

        public IEnumerable<ScoreAlgorythm> GetAvailableScoreAlgorythms(ItemType itemType)
        {
            string currentlyInUse = GetCurrentlyInUse(itemType);
            return new ScoreAlgorythm[] {
                    new ScoreAlgorythm{ 
                        Name="Top 3 (3,2,1)",
                        Description="First gets 3 points, Second gets 2 points, Third gets 1 point",
                        NoOfItemsConsidered=3,
                        ScoreRule=DummyAlgorythmPoolProvider.Rule321,
                        InUse=(currentlyInUse=="Top 3 (3,2,1)")},
                    new ScoreAlgorythm{
                        Name="F1",
                        Description="10,8,6,5,4,3,2,1",
                        NoOfItemsConsidered=8,
                        ScoreRule=RuleF1,
                        InUse=(currentlyInUse=="F1")
                    }
            };
        }

        private string GetCurrentlyInUse(ItemType itemType)
        {
            var existingItem = GetAllRulesUsed().FirstOrDefault(s => s.ItemType == itemType);
            return (existingItem!=null) ? existingItem.RuleName : string.Empty;
        }

        private List<NewRule> GetAllRulesUsed()
        {
            var fileFullPath = ConfigurationManager.AppSettings["SettingsFile"];
            if (!File.Exists(fileFullPath))
                return new List<NewRule>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NewRule>));
                return (xmlSerializer.Deserialize(fs) as List<NewRule>);
            }
        }

        public bool SetRule(NewRule rule)
        {
            var existingSettings = GetAllRulesUsed();
            if (existingSettings.FirstOrDefault(r=>r.ItemType==rule.ItemType)!=null)
                existingSettings.First(r=>r.ItemType==rule.ItemType).RuleName = rule.RuleName;
            else
                existingSettings.Add(rule);
            
            var fileFullPath = ConfigurationManager.AppSettings["SettingsFile"];

            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NewRule>));
                xmlSerializer.Serialize(fs,existingSettings);
                return true;
            }
            return false;
        }
    }
}
