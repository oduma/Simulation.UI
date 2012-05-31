using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider.DataTypes
{
    [XmlRoot(ElementName = "lfm")]
    public class LfmGetChartArtistsResponse
    {
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }

        [XmlArrayItem(ElementName = "artist", Type = typeof(Artist))]
        [XmlArray(ElementName = "weeklyartistchart")]
        public List<Artist> Artists { get; set; }

    }
}
