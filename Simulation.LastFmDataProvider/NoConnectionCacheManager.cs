using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.CacheManager;
using System.Configuration;
using Sciendo.Core;
using System.IO;
using System.Xml.Serialization;

namespace Simulation.LastFmDataProvider
{
    public class NoConnectionCacheManager:ICacheManager
    {
        private string _cachedXmlFolder;

        public NoConnectionCacheManager()
        {
            _cachedXmlFolder = ConfigurationManager.AppSettings["CachedXmlFolder"];
        }

        public void Add<T>(string key, T cacheItem, Type realType) where T: class 
        {
            string fileFullPath = _cachedXmlFolder + @"\" + key + ".xml"; 
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(realType);
                xmlSerializer.Serialize(fs, cacheItem);
            }
            
        }

        public bool TryGet<T>(string cacheItemKey, out T cacheItem, Type knownType) where T: class
        {
            cacheItem= null;
            string fileFullPath = _cachedXmlFolder + @"\" + cacheItemKey + ".xml"; 

            if (!File.Exists(fileFullPath))
                return false;
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(knownType);
                cacheItem = xmlSerializer.Deserialize(fs) as T;
            }

            return true;
        }

        public void Set<T>(string key, T cacheItem) where T: class
        {
            string fileFullPath = _cachedXmlFolder + @"\" + key + ".xml";
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(fs, cacheItem);
            }
            

        }

        public void ForceRefresh()
        {
            throw new NotImplementedException();
        }
    }
}
