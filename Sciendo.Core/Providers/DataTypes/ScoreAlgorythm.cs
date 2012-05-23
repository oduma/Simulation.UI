using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.Providers.DataTypes
{
    public class ScoreAlgorythm
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Func<int, int> ScoreRule { get; set; }

        public int NoOfItemsConsidered { get; set; }

        public bool InUse { get; set; }
    }
}
