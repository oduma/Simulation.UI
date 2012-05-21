using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.Providers.DataTypes
{
    public class WeekSummary
    {
        public int WeekNo { get; set; }

        public bool TopProcessed { get; set; }

        public ItemType ItemType { get; set; }
    }
}
