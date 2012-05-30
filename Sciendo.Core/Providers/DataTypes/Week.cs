using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.Providers.DataTypes
{
    public class Week:WeekSummary
    {

        public DateTime StartingFrom { get; set; }

        public DateTime EndingIn { get; set; }
    }
}
