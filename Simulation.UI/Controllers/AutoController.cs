using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sciendo.Core.Providers;
using Simulation.UI.Models;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core;

namespace Simulation.UI.Controllers
{
    public class AutoController : Controller
    {
        //
        // GET: /Auto/

        public ActionResult Index()
        {
            var model = new AutoVoteSetupModel();
            IAlgorythmPoolProvider algorithmPoolProvider = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            model.SettingsCollection = new Settings[] {
                new Settings{ItemType=ItemType.Artist,ScoreAlgorythms=algorithmPoolProvider.GetAvailableScoreAlgorythms(ItemType.Artist)},
                new Settings{ItemType=ItemType.Track,ScoreAlgorythms=algorithmPoolProvider.GetAvailableScoreAlgorythms(ItemType.Track)}
            };
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTotals(string sidx, string sord, int? page, int? rows)
        {
            ITopRecordProvider topTotalsProvider = ClientFactory.GetClient<ITopRecordProvider>();
            var result = new[] { topTotalsProvider.GetTotalItems(ItemType.Artist), topTotalsProvider.GetTotalItems(ItemType.Track) };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetWeeksToVote()
        {
            ITopProvider topProvider = ClientFactory.GetClient<ITopProvider>();
            var availableWeeks = topProvider.GetAvailableWeeks(Utility.LastWeekNo(DateTime.Now));
            ITopRecordProvider topRecordProvider = ClientFactory.GetClient<ITopRecordProvider>();
            var weeksProcessed = topRecordProvider.GetTopProcessed();
            var result=new []
                {new 
                    { ItemType = ItemType.Artist, 
                        Weeks = availableWeeks.Where(a => weeksProcessed.Where(w => w.ItemType == ItemType.Artist)
                            .FirstOrDefault(w => w.WeekNo == a.WeekNo) == null)} ,
                new 
                    { ItemType = ItemType.Track, 
                        Weeks = availableWeeks.Where(a => weeksProcessed.Where(w => w.ItemType == ItemType.Track)
                            .FirstOrDefault(w => w.WeekNo == a.WeekNo) == null) }};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopItems(WeekWithAlgorythm requestedWeek)
        {
            ITopProvider topProvider = ClientFactory.GetClient<ITopProvider>();
            IScoreAlgorythm scoreAlgotyhm = ClientFactory.GetClients<IScoreAlgorythm>().FirstOrDefault(a=>a.Name==requestedWeek.ScoreAlgorythmName);
            if(scoreAlgotyhm==null)
                return null;
            var weeklyTop = topProvider.GetTopByWeek(new Week { WeekNo = requestedWeek.WeekNo, 
                StartingFrom = (new DateTime(1970, 1, 1)).AddMilliseconds(Convert.ToDouble(requestedWeek.StartingFrom.Replace("/Date(","").Replace(")/",""))), 
                EndingIn = (new DateTime(1970, 1, 1)).AddMilliseconds(Convert.ToDouble(requestedWeek.EndingIn.Replace("/Date(","").Replace(")/",""))), 
                ItemType = requestedWeek.ItemType }
                , scoreAlgotyhm.NoOfItemsConsidered, requestedWeek.ItemType);
            return Json(weeklyTop);
        }

    }
}
