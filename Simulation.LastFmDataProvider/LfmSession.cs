using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider
{
    [XmlRoot(ElementName="session")]
    public class LfmSession
    {
        [XmlElement(ElementName="name")]
        public string UserName { get; set; }

        [XmlElement(ElementName = "key")]
        public string Key { get; set; }

        [XmlElement(ElementName = "subscriber")]
        public string Subscriber { get; set; }


    }
}
