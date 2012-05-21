using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simulation.UI.Models;

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
            TopForTotalModel topForTotalModel = new TopForTotalModel()
            {
                WeekNo = Convert.ToInt32(Request.Form["WeekNo"])
            };
            int noOfItems = (Request.Form.AllKeys.Length - 1) / 2;
            var topItems = new List<TopItem>();
            for (int i = 0; i < noOfItems; i++)
            {
                topItems.Add(new TopItem { ItemName = Request.Form[string.Format("TopItems[{0}][ItemName]", i)], Rank = Convert.ToInt32(Request.Form[string.Format("TopItems[{0}][Rank]", i)]) });
            }
            topForTotalModel.TopItems = topItems;

            TotalsAfterWeek totalsAfterWeek = new TotalsAfterWeek();
            ITopTotalsProvider topTotalsProvider = new DummyTopTotalsProvider();
            topTotalsProvider.SaveTotalForItems(topForTotalModel, id,HttpContext);
            totalsAfterWeek.TotalItems = topTotalsProvider.GetTotalItems(id, HttpContext);
            totalsAfterWeek.WeekNo = topForTotalModel.WeekNo;
            return Json(totalsAfterWeek/*,JsonRequestBehavior.AllowGet*/);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTotals(string id, string sidx, string sord, int? page, int? rows)
        {
            ITopTotalsProvider topTotalsProvider = new DummyTopTotalsProvider();
            return Json(topTotalsProvider.GetTotalItems(id,HttpContext), JsonRequestBehavior.AllowGet);

        }
        public ActionResult WeekSelection(string id)
        {
            WeekSelectionModel weekSelectionModel= new WeekSelectionModel(id);
            weekSelectionModel.AvailableWeeks = GetWeeks(DateTime.Now);
            ITopRecordProvider topRecordProvider = new DummyTopRecordProvider();
            weekSelectionModel.NextWeekToProcess = topRecordProvider.GetTopProcessed(HttpContext).Where(t => t.TypeOfSelection.ToLower() == weekSelectionModel.TypeOfSelection.ToLower()).Max(t => t.WeekNo) + 1;
            ITopProvider topProvider = new DummyTopProvider();
            weekSelectionModel.FirstWeekTop = topProvider.GetTopByWeek(weekSelectionModel.NextWeekToProcess, weekSelectionModel.TypeOfSelection, this.HttpContext);
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
        private IEnumerable<Week> GetWeeks()
        {
            return new List<Week> 
            {
                new Week{ WeekNo=1},
                new Week {WeekNo=2},
                new Week {WeekNo=3},
                new Week {WeekNo=4}
            };
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Top(TopWeekRequest topWeekRequest)
        {
            ITopProvider topProvider = new DummyTopProvider();
            WeeklyTopModel weeklyTopModel = topProvider.GetTopByWeek(topWeekRequest.WeekNo,topWeekRequest.TypeOfSelection,HttpContext);
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
