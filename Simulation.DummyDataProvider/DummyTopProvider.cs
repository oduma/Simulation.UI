﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.DummyDataProvider
{
    public class DummyTopProvider:DummyTopRecordProvider,ITopProvider
    {
        public WeeklyTop GetTopByWeek(int weekNo, int topLength, ItemType itemType)
        {
            WeeklyTop weeklyTop = new WeeklyTop();
            weeklyTop.WeekNo = weekNo;
            weeklyTop.TopItems = GetTopItems(topLength, itemType);
            weeklyTop.TopProcessed = IsWeekProcessed(GetTopProcessed(), weekNo, itemType);
            weeklyTop.ItemType = itemType;
            return weeklyTop;
        }

        private bool IsWeekProcessed(IEnumerable<WeekSummary> topRecordedItems, int weekNo, ItemType itemType)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.ItemType.ToString().ToLower() == itemType.ToString().ToLower()) != null;
        }

        private IEnumerable<TopItem> GetTopItems(int maxRank, ItemType itemType)
        {
            for (int i = 1; i < maxRank + 1; i++)
                yield return new TopItem { Rank = i, Position = i, ItemName = itemType.ToString() + i, NumberOfPlays = 20 - i };
        }


        public IEnumerable<Week> GetAvailableWeeks()
        {
            DateTime tillNow = DateTime.Now;

            DateTime firstSundayOfTheYear = new DateTime(tillNow.Year, 1, 1);
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
            while (endOfWeek < tillNow)
            {
                yield return new Week { WeekNo = weekNo++, StartingFrom = firstSundayOfTheYear, EndingIn = endOfWeek };
                firstSundayOfTheYear = firstSundayOfTheYear.AddDays(7);
                endOfWeek = firstSundayOfTheYear.AddDays(6);
            }
        }

    }
}
