using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core;
using Simulation.LastFmDataProvider.DataTypes;
using Sciendo.Core.CacheManager;

namespace Simulation.LastFmDataProvider
{
    public class TopProvider: ITopProvider
    {
        private static readonly int _defaultTopLength = 10;
        ITopRecordProvider _topRecordProvider;

        public TopProvider()
        {
            _topRecordProvider = ClientFactory.GetClient<ITopRecordProvider>();
        }

        public WeeklyTop GetTopByWeek(Week requestedWeek, int topLength, ItemType itemType)
        {
            if (topLength < _defaultTopLength)
                topLength = _defaultTopLength;

            requestedWeek.TopProcessed = IsWeekProcessed(_topRecordProvider.GetTopProcessed(), requestedWeek.WeekNo, itemType);
            if (itemType == ItemType.Artist)
                return GetTopByWeekForArtists(requestedWeek, topLength);
            else
                return GetTopbyWeekForTracks(requestedWeek, topLength);
        }

        private bool IsWeekProcessed(IEnumerable<WeekSummary> topRecordedItems, int weekNo, ItemType itemType)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.ItemType.ToString().ToLower() == itemType.ToString().ToLower()) != null;
        }

        private WeeklyTop GetTopbyWeekForTracks(Week week, int topLength)
        {
            try
            {
                return new WeeklyTop
                {
                    WeekNo = week.WeekNo,
                    TopProcessed = week.TopProcessed,
                    ItemType = ItemType.Track,
                    TopItems = Utility.Deserialize<LfmGetChartTracksResponse>(
                        HttpHelper.Get(@"http://ws.audioscrobbler.com/2.0/?method=user.getweeklytrackchart" +
                            @"&user=scentmaster&from=" +
                            week.StartingFrom.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() +
                            "&to=" +
                            week.EndingIn.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() +
                            "&api_key=" + ApiKey))
                                .Tracks
                                .TransformToTopItems(topLength)
                };
            }
            catch (Exception ex)
            {
                //should log something in here;
                return null;
            }
        }

        private WeeklyTop GetTopByWeekForArtists(Week week, int topLength)
        {
            try
            {
                return new WeeklyTop
                    {
                        WeekNo = week.WeekNo,
                        TopProcessed = week.TopProcessed,
                        ItemType = ItemType.Artist,
                        TopItems = Utility.Deserialize<LfmGetChartArtistsResponse>(
                            HttpHelper.Get(@"http://ws.audioscrobbler.com/2.0/?method=user.getweeklyartistchart" +
                                @"&user=scentmaster&from=" +
                                week.StartingFrom.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() +
                                "&to=" +
                                week.EndingIn.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() +
                                "&api_key=" + ApiKey))
                                    .Artists
                                    .TransformToTopItems(topLength)
                    };
            }
            catch (Exception ex)
            {
                //Should log
                return null;
            }
        }

        public static string ApiKey { get { return "5e625305596ba928b8d8664bd2a95b08"; } }

        public List<Week> GetAvailableWeeks(int lastWeekNo)
        {
            var url = "http://ws.audioscrobbler.com/2.0/?method=user.getweeklychartlist&user=scentmaster&api_key=" + ApiKey;
            var lfmString = HttpHelper.Get(url);
            if (string.IsNullOrEmpty(lfmString))
                return null;
            return Utility.Deserialize<LfmGetWeekChartlistResponse>(lfmString).ChartWeeks.TransformToWeeks();
        }
    }
}
