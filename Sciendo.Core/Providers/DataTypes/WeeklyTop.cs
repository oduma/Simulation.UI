using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.Providers.DataTypes
{
    public class WeeklyTop:WeekSummary
    {
        public List<TopItem> TopItems { get; set; }
    }
}
