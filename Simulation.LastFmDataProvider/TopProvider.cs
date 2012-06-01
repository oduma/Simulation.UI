using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core;
using Simulation.LastFmDataProvider.DataTypes;
using Simulation.DummyDataProvider;

namespace Simulation.LastFmDataProvider
{
    public class TopProvider:DummyTopRecordProvider, ITopProvider
    {
        public WeeklyTop GetTopByWeek(int weekNo, int topLength, ItemType itemType)
        {
            var requestedWeek = GetAvailableWeeks().First(w => w.WeekNo == weekNo);
            requestedWeek.TopProcessed=IsWeekProcessed(GetTopProcessed(), weekNo, itemType);
            if (itemType == ItemType.Artist)
                return GetTopByWeekForArtists(requestedWeek, topLength);
            else
                return GetTopbyWeekForTracks(requestedWeek, topLength);
        }

        private WeeklyTop GetTopbyWeekForTracks(Week week, int topLength)
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

        private WeeklyTop GetTopByWeekForArtists(Week week, int topLength)
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

        public static string ApiKey { get { return "5e625305596ba928b8d8664bd2a95b08"; } }

        public IEnumerable<Week> GetAvailableWeeks()
        {
            var url = "http://ws.audioscrobbler.com/2.0/?method=user.getweeklychartlist&user=scentmaster&api_key=" + ApiKey;
            return Utility.Deserialize<LfmGetWeekChartlistResponse>(HttpHelper.Get(url)).ChartWeeks.TransformToWeeks();
        }
    }
}
