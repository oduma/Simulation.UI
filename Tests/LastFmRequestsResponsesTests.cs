using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using System.Xml.Serialization;
using Simulation.LastFmDataProvider;
using Sciendo.Core;
using Simulation.LastFmDataProvider.DataTypes;

namespace Tests
{
    [TestFixture]
    public class LastFmRequestsResponsesTests
    {
        [Test]
        public void Deserialize_WeeklyChartlist_Ok()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"Data\getweeklychartsok.xml");
            string expectedXml = xmlDocument.InnerXml;
            LfmGetWeekChartlistResponse actual = Utility.Deserialize<LfmGetWeekChartlistResponse>(expectedXml);
            Assert.IsNotNull(actual);

            LfmGetWeekChartlistResponse expected = new LfmGetWeekChartlistResponse { Status = "ok", ChartWeeks = new List<ChartWeek> { new ChartWeek { From = 1108296002, To = 1108900802 }, new ChartWeek { From = 1108900801, To = 1109505601 } } };

            Assert.AreEqual(expected.Status, actual.Status);

            Assert.IsNotNull(actual.ChartWeeks);

            Assert.AreEqual(2, actual.ChartWeeks.Count);

            Assert.AreEqual(expected.ChartWeeks[0].From, actual.ChartWeeks[0].From);

            Assert.AreEqual(expected.ChartWeeks[1].From, actual.ChartWeeks[1].From);

            Assert.AreEqual(expected.ChartWeeks[0].To, actual.ChartWeeks[0].To);

            Assert.AreEqual(expected.ChartWeeks[1].To, actual.ChartWeeks[1].To);

        }
    }
}
