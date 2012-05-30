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

        [Test]
        public void DeSerialize_SessionOk_Response()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"Data\sessionokresponse.xml");
            string expectedXml = xmlDocument.InnerXml;

            LfmSessionResponse actual = Utility.Deserialize<LfmSessionResponse>(expectedXml);

            Assert.IsNotNull(actual);

            LfmSessionResponse expected = new LfmSessionResponse { Status = "ok", Session = new LfmSession { UserName = "MyLastFMUserName", Key = "d580d57f32848f5dcf574d1ce18d78b2", Subscriber = "0" } };

            Assert.AreEqual(expected.Status, actual.Status);

            Assert.AreEqual(expected.Session.UserName, actual.Session.UserName);

            Assert.AreEqual(expected.Session.Key, actual.Session.Key);

            Assert.AreEqual(expected.Session.Subscriber, actual.Session.Subscriber);
        }

        [Test]
        public void GetASessionKey_No_Token_No_API_Defined()
        {
            TopProvider topProvider = new TopProvider();
            var response = topProvider.TryGetASession(string.Empty);
            Assert.IsNotNull(response);
        }
    }
}
