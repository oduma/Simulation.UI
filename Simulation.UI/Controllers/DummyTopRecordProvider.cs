using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simulation.UI.Models;
using System.IO;
using System.Xml.Serialization;

namespace Simulation.UI.Controllers
{
    public class DummyTopRecordProvider:ITopRecordProvider
    {
        public List<TopRecordedItem> GetTopProcessed(HttpContextBase context)
        {
            var fileFullPath = context.Request.MapPath(@"..\..") + @"\toprecord.xml";
            if (!File.Exists(fileFullPath))
                return new List<TopRecordedItem>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TopRecordedItem>));
                return xmlSerializer.Deserialize(fs) as List<TopRecordedItem>;
            }
        }
    }
}