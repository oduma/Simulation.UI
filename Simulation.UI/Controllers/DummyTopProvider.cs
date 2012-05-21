using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation.UI.Models;
using System.Web;

namespace Simulation.UI.Controllers
{
    public class DummyTopProvider: DummyTopRecordProvider,ITopProvider
    {
        public WeeklyTopModel GetTopByWeek(int weekNo, string typeOfSelection,HttpContextBase context)
        {
            WeeklyTopModel weeklyTopModel = new WeeklyTopModel();
            weeklyTopModel.WeekNo = weekNo;
            weeklyTopModel.TopItems = GetTopItems(10, typeOfSelection);
            weeklyTopModel.TopProcessed = IsWeekProcessed(GetTopProcessed(context), weekNo, typeOfSelection);
            return weeklyTopModel;
        }

        private bool IsWeekProcessed(List<TopRecordedItem> topRecordedItems, int weekNo, string typeOfSelection)
        {
            return topRecordedItems.FirstOrDefault(r => r.WeekNo == weekNo && r.TypeOfSelection.ToLower() == typeOfSelection.ToLower()) != null;
        }

        private IEnumerable<TopItem> GetTopItems(int maxRank, string typeOfSelection)
        {
            for (int i = 1; i < maxRank + 1; i++)
                yield return new TopItem { Rank = i,Position=i,ItemName=typeOfSelection + i, NumberOfPlays=20-i };
        }
    }
}
