using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Sciendo.Core
{
    public static class Utility
    {
        public static T Deserialize<T>(string xmlString) where T: class 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(new UTF8Encoding().GetBytes(xmlString));
            return xmlSerializer.Deserialize(ms) as T;
        }

        public static string Serialize<T>(T data) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            xmlSerializer.Serialize(ms, data);

            return new UTF8Encoding().GetString(ms.GetBuffer());
        }
    }
}
