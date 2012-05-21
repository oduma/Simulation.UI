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

    }
}
