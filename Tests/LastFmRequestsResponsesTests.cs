using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using System.Xml.Serialization;
using Simulation.LastFmDataProvider;
using Sciendo.Core;

namespace Tests
{
    [TestFixture]
    public class LastFmRequestsResponsesTests
    {
        [Test]
        public void DeSerialize_TokenOk_Response()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"Data\tokenokresponse.xml");
            string expectedXml = xmlDocument.InnerXml;

            LfmTokenResponse actual = Utility.Deserialize<LfmTokenResponse>(expectedXml);

            Assert.IsNotNull(actual);

            LfmTokenResponse expected = new LfmTokenResponse { Status = "ok", Token = "2d1ac26b51c7365ff14c8768008a4422" };

            Assert.AreEqual(expected.Status, actual.Status);

            Assert.AreEqual(expected.Token, actual.Token);

        }
    }
}
