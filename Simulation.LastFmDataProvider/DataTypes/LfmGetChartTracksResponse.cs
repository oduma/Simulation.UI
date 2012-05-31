using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider.DataTypes
{
    [XmlRoot(ElementName = "lfm")]
    public class LfmGetChartTracksResponse
    {
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }

        [XmlArrayItem(ElementName = "track", Type = typeof(Track))]
        [XmlArray(ElementName = "weeklytrackchart")]
        public List<Track> Tracks { get; set; }

    }
}
