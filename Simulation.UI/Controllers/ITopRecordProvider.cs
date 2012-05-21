using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation.UI.Models;
using System.Web;

namespace Simulation.UI.Controllers
{
    public interface ITopRecordProvider
    {
        List<TopRecordedItem> GetTopProcessed(HttpContextBase context);
    }
}
