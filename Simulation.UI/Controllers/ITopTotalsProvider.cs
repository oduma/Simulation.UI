using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation.UI.Models;
using System.Web.Mvc;
using System.Web;

namespace Simulation.UI.Controllers
{
    public interface ITopTotalsProvider: ITopRecordProvider
    {
        void SaveTotalForItems(TopForTotalModel topTotalForModel, string typeOfTotal, HttpContextBase context);

        IEnumerable<TotalItem> GetTotalItems(string typeOfTotal,HttpContextBase context);

    }
}
