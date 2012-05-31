using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider.DataTypes
{
    [XmlRoot(ElementName="lfm")]
    public class LfmGetWeekChartlistResponse
    {
        [XmlAttribute(AttributeName="status")]
        public string Status { get; set; }

        [XmlArrayItem(ElementName="chart", Type=typeof(ChartWeek))]
        [XmlArray(ElementName="weeklychartlist")]
        public List<ChartWeek> ChartWeeks{ get; set;}

    }
}
