using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation.UI.Models;
using System.Web;

namespace Simulation.UI.Controllers
{
    public interface ITopProvider
    {
        WeeklyTopModel GetTopByWeek(int weekNo, string typeOfSelection,HttpContextBase context);
    }
}
