using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider.DataTypes
{
    [XmlRoot(ElementName="chart")]
    public class ChartWeek
    {
        [XmlAttribute(AttributeName = "from")]
        public long From { get; set; }

        [XmlAttribute(AttributeName = "to")]
        public long To { get; set; }
    }
}
