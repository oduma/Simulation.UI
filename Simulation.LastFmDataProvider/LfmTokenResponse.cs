using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider
{
    [XmlRoot(ElementName="lfm")]
    public class LfmTokenResponse
    {
        [XmlAttribute(AttributeName="status")]
        public string Status { get; set; }

        [XmlElement(ElementName="token")]
        public string Token { get; set; }
    }
}
