using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simulation.UI.Models;
using Sciendo.Core.Providers;
using Sciendo.Core;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddToTotals(string id)
        {
            ItemType currentItemType;
            if(!Enum.TryParse<ItemType>(id,true,out currentItemType))
                throw new ArgumentException("argument id is not an ItemType");

            WeeklyTop topForTotalModel = new WeeklyTop()
            {
                WeekNo = Convert.ToInt32(Request.Form["WeekNo"])
            };
            int noOfItems = (Request.Form.AllKeys.Length - 1) / 2;
            var topItems = new List<TopItem>();
            for (int i = 0; i < noOfItems; i++)
            {
                topItems.Add(
                    new TopItem { ItemName = Request.Form[string.Format("TopItems[{0}][ItemName]", i)], 
                    Rank = Convert.ToInt32(Request.Form[string.Format("TopItems[{0}][Rank]", i)]),ItemType= currentItemType});
            }
            topForTotalModel.TopItems = topItems;
            topForTotalModel.ItemType = currentItemType;

            TotalsAfterWeek totalsAfterWeek = new TotalsAfterWeek();
            ITopTotalsProvider topTotalsProvider = ClientFactory.GetClient<ITopTotalsProvider>();
            topTotalsProvider.SaveTotalForItems(topForTotalModel);
            totalsAfterWeek.TopItems = topTotalsProvider.GetTotalItems(currentItemType);
            totalsAfterWeek.WeekNo = topForTotalModel.WeekNo;
            return Json(totalsAfterWeek/*,JsonRequestBehavior.AllowGet*/);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTotals(string id, string sidx, string sord, int? page, int? rows)
        {
            ItemType currentItemType;
            if (!Enum.TryParse<ItemType>(id, true, out currentItemType))
                throw new ArgumentException("argument id is not an ItemType");

            ITopTotalsProvider topTotalsProvider = ClientFactory.GetClient<ITopTotalsProvider>();
            return Json(topTotalsProvider.GetTotalItems(currentItemType), JsonRequestBehavior.AllowGet);

        }
        public ActionResult WeekSelection(string id)
        {
            ItemType currentItemType;
            if (!Enum.TryParse<ItemType>(id, true, out currentItemType))
                throw new ArgumentException("argument id is not an ItemType");

            WeekSelectionModel weekSelectionModel = new WeekSelectionModel(currentItemType);
            weekSelectionModel.AvailableWeeks = GetWeeks(DateTime.Now);
            ITopRecordProvider topRecordProvider = ClientFactory.GetClient<ITopRecordProvider>();
            var topProcessed = topRecordProvider.GetTopProcessed();
            if (topProcessed.FirstOrDefault(p => p.ItemType == weekSelectionModel.ItemType) != null)
                weekSelectionModel.NextWeekToProcess = topProcessed.Where(t => t.ItemType == weekSelectionModel.ItemType).Max(t => t.WeekNo) + 1;
            else
                weekSelectionModel.NextWeekToProcess = 1;
            ITopProvider topProvider = ClientFactory.GetClient<ITopProvider>();
            weekSelectionModel.FirstWeekTop = topProvider.GetTopByWeek(weekSelectionModel.NextWeekToProcess, 10, weekSelectionModel.ItemType);
            return View(weekSelectionModel);
        }

        private IEnumerable<Week> GetWeeks(DateTime tillNow)
        {
            DateTime firstSundayOfTheYear = new DateTime(tillNow.Year, 1, 1);
            for(int i=0;i<7;i++)
            {
                firstSundayOfTheYear = firstSundayOfTheYear.AddDays(i);
                if(firstSundayOfTheYear.DayOfWeek == DayOfWeek.Sunday)
                {
                    break;
                }
            }
            
            DateTime endOfWeek=firstSundayOfTheYear.AddDays(6);
            int weekNo = 1;
            while (endOfWeek < tillNow)
            {
                yield return new Week { WeekNo=weekNo++,StartingFrom=firstSundayOfTheYear,EndingIn=endOfWeek};
                firstSundayOfTheYear = firstSundayOfTheYear.AddDays(7);
                endOfWeek = firstSundayOfTheYear.AddDays(6);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Top(WeekSummary topWeekRequest)
        {
            ITopProvider topProvider = ClientFactory.GetClient<ITopProvider>();
            WeeklyTop weeklyTopModel = topProvider.GetTopByWeek(topWeekRequest.WeekNo,10,topWeekRequest.ItemType);
            return Json(weeklyTopModel, JsonRequestBehavior.AllowGet);
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Options()
        {
            List<OptionsModel> optionsModel = new List<OptionsModel> ();
            LoadWithDefaultValues(optionsModel);
            return Json(optionsModel,JsonRequestBehavior.AllowGet);
        }

        private void LoadWithDefaultValues(List<OptionsModel> optionsModel)
        {
            optionsModel.AddRange(new List<OptionsModel>{
                new OptionsModel{Value="One",DisplayName="One"},
                new OptionsModel{Value="Two",DisplayName="Two"},
                new OptionsModel{Value="Three",DisplayName="Three"},
                new OptionsModel{Value="Four",DisplayName="Four"}});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Options(OptionsModel newOption)
        {
            List<OptionsModel> optionsModel = new List<OptionsModel>();
            LoadWithDefaultValues(optionsModel);
            optionsModel.Add(newOption);
            return Json(optionsModel);
        }
    }
}
