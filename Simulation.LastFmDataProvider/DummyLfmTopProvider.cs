﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using System.Configuration;
using Sciendo.Core.Providers.DataTypes;
using System.IO;
using System.Xml.Serialization;
using Simulation.LastFmDataProvider.DataTypes;
using Sciendo.Core;

namespace Simulation.LastFmDataProvider
{
    public class DummyLfmTopProvider : ITopProvider
    {
        ITopRecordProvider _topRecordProvider;

        public DummyLfmTopProvider()
        {
            _topRecordProvider = ClientFactory.GetClient<ITopRecordProvider>();
        }

        public WeeklyTop GetTopByWeek(Week requestedWeek, int topLength, ItemType itemType)
        {

            if (itemType == ItemType.Artist)
                return GetTopByWeekForArtists(requestedWeek.WeekNo, topLength, IsWeekProcessed(_topRecordProvider.GetTopProcessed(), requestedWeek.WeekNo, itemType));
            else
                return GetTopbyWeekForTracks(requestedWeek.WeekNo, topLength, IsWeekProcessed(_topRecordProvider.GetTopProcessed(), requestedWeek.WeekNo, itemType));
        }

        private bool IsWeekProcessed(IEnumerable<WeekSummary> topRecordedItems, int weekNo, ItemType itemType)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.ItemType.ToString().ToLower() == itemType.ToString().ToLower()) != null;
        }

        private WeeklyTop GetTopbyWeekForTracks(int weekNo, int topLength, bool isWeekProcessed)
        {
            var fileFullPath = ConfigurationManager.AppSettings["TracksDummyFile"];

            if (!File.Exists(fileFullPath))
                return new WeeklyTop();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LfmGetChartTracksResponse));
                return new WeeklyTop
                {
                    ItemType = ItemType.Track,
                    TopProcessed = isWeekProcessed,
                    WeekNo = weekNo,
                    TopItems = (xmlSerializer.Deserialize(fs) as LfmGetChartTracksResponse)
                        .Tracks.TransformToTopItems(topLength)
                };
            }


        }

        private WeeklyTop GetTopByWeekForArtists(int weekNo, int topLength, bool isWeekProcessed)
        {
            var fileFullPath = ConfigurationManager.AppSettings["ArtistsDummyFile"];

            if (!File.Exists(fileFullPath))
                return new WeeklyTop();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LfmGetChartArtistsResponse));
                return new WeeklyTop { ItemType = ItemType.Artist,TopProcessed=isWeekProcessed, WeekNo = weekNo, TopItems = (xmlSerializer.Deserialize(fs) as LfmGetChartArtistsResponse)
                    .Artists.TransformToTopItems(topLength) };
            }


        }



        public List<Week> GetAvailableWeeks(int lastWeekNo)
        {
            
            var fileFullPath = ConfigurationManager.AppSettings["WeeksDummyFile"];

            if (!File.Exists(fileFullPath))
                return new List<Week>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LfmGetWeekChartlistResponse));
                return (xmlSerializer.Deserialize(fs) as LfmGetWeekChartlistResponse).ChartWeeks.TransformToWeeks();
            }
        }
    }
}
