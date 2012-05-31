using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider.DataTypes
{
    [XmlRoot(ElementName="artist")]
    public class Artist
    {
        [XmlAttribute(AttributeName="rank")]
        public int Rank { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "playcount")]
        public int PlayCount { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

    }
}
