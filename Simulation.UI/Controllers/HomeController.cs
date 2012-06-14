using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simulation.UI.Models;
using Sciendo.Core.Providers;
using Sciendo.Core;
using Sciendo.Core.Providers.DataTypes;
using Simulation.LastFmDataProvider;

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

        public ActionResult Settings(string id)
        {
            ItemType currentItemType;
            if (!Enum.TryParse<ItemType>(id, true, out currentItemType))
                throw new ArgumentException("argument id is not an ItemType");
            IAlgorythmPoolProvider algorythmPoolProvider = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            ITopRecordProvider topRecordProvider = ClientFactory.GetClient<ITopRecordProvider>();
            var topProcessed = topRecordProvider.GetTopProcessed();
            return View("Settings", new SettingsModel());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeRules(string id, string ruleName)
        {
            ItemType currentItemType;
            if (!Enum.TryParse<ItemType>(id, true, out currentItemType))
                throw new ArgumentException("argument id is not an ItemType");
            CurrentScoreAlgorythm rule = new CurrentScoreAlgorythm { ItemType = currentItemType, Name = ruleName };
            IAlgorythmPoolProvider alg = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            alg.SetRule(rule);
            ITopRecordProvider topTotalsProvider = ClientFactory.GetClient<ITopRecordProvider>();
            topTotalsProvider.ClearAll(currentItemType);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

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
            ITopRecordProvider topTotalsProvider = ClientFactory.GetClient<ITopRecordProvider>();
            IAlgorythmPoolProvider algorythmPoolProvider = ClientFactory.GetClient<IAlgorythmPoolProvider>();

            topTotalsProvider.SaveTotalForItems(topForTotalModel,algorythmPoolProvider.GetCurrentAlgorythm(currentItemType).ScoreRule);
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

            ITopRecordProvider topTotalsProvider = ClientFactory.GetClient<ITopRecordProvider>();
            return Json(topTotalsProvider.GetTotalItems(currentItemType), JsonRequestBehavior.AllowGet);

        }
        public ActionResult WeekSelection(string id)
        {
            ItemType currentItemType;
            if (!Enum.TryParse<ItemType>(id, true, out currentItemType))
                throw new ArgumentException("argument id is not an ItemType");

            WeekSelectionModel weekSelectionModel = new WeekSelectionModel(currentItemType);
            ITopProvider topProvider = ClientFactory.GetClient<ITopProvider>();
            var availableWeeks = topProvider.GetAvailableWeeks(Utility.LastWeekNo(DateTime.Now));
            if(availableWeeks!=null)
                weekSelectionModel.AvailableWeeks = availableWeeks.Select(w => new WeekModel { EndingIn = w.EndingIn, ItemType = currentItemType, StartingFrom = w.StartingFrom,  WeekNo = w.WeekNo });
            else
                weekSelectionModel.AvailableWeeks=new WeekModel[] {};
            ITopRecordProvider topRecordProvider = ClientFactory.GetClient<ITopRecordProvider>();
            var topProcessed = topRecordProvider.GetTopProcessed();
            if (topProcessed.FirstOrDefault(p => p.ItemType == weekSelectionModel.ItemType) != null)
                weekSelectionModel.NextWeekToProcess = topProcessed.Where(t => t.ItemType == weekSelectionModel.ItemType).Max(t => t.WeekNo) + 1;
            else
                weekSelectionModel.NextWeekToProcess = 1;
            if (availableWeeks != null)
            {
                var requestedWeek = availableWeeks.FirstOrDefault(a => a.WeekNo == weekSelectionModel.NextWeekToProcess);
                weekSelectionModel.FirstWeekTop = topProvider.GetTopByWeek(requestedWeek, 10, weekSelectionModel.ItemType);
            }
            IAlgorythmPoolProvider algoryhtmProvider=ClientFactory.GetClient<IAlgorythmPoolProvider>();
            weekSelectionModel.Settings = new SettingsModel();
            weekSelectionModel.Settings.ScoreAlgorythms = algoryhtmProvider.GetAvailableScoreAlgorythms(currentItemType);
            weekSelectionModel.CurrentAlgorythm = algoryhtmProvider.GetCurrentAlgorythm(currentItemType);
            return View(weekSelectionModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Top(WeekSummary topWeekRequest)
        {
            ITopProvider topProvider = ClientFactory.GetClient<ITopProvider>();
            var availableWeeks = topProvider.GetAvailableWeeks(Utility.LastWeekNo(DateTime.Now));
            if (availableWeeks == null)
                return null;
            WeeklyTop weeklyTopModel = topProvider.GetTopByWeek(availableWeeks.FirstOrDefault(a=>a.WeekNo==topWeekRequest.WeekNo),
                10,topWeekRequest.ItemType);
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
