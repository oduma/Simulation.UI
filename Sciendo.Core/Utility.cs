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

        public static List<T> DeserializeFromFile<T>(string fileName) where T: class
        {
            if (!File.Exists(fileName))
                return new List<T>();
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                return (xmlSerializer.Deserialize(fs) as List<T>);
            }
        }

        public static void SerializeToFile<T>(List<T> data, string fileName) where T: class
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                xmlSerializer.Serialize(fs, data);
            }

        }
        public static int LastWeekNo(DateTime today)
        {
            DateTime firstSundayOfTheYear = new DateTime(today.Year, 1, 1);
            for (int i = 0; i < 7; i++)
            {
                firstSundayOfTheYear = firstSundayOfTheYear.AddDays(i);
                if (firstSundayOfTheYear.DayOfWeek == DayOfWeek.Sunday)
                {
                    break;
                }
            }

            DateTime endOfWeek = firstSundayOfTheYear.AddDays(6);
            int weekNo = 1;
            while (endOfWeek < today)
            {
                weekNo++;
                firstSundayOfTheYear = firstSundayOfTheYear.AddDays(7);
                endOfWeek = firstSundayOfTheYear.AddDays(6);
            }
            return weekNo;
        }
    }
}
