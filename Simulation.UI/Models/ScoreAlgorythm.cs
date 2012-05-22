using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation.UI.Models
{
    public class ScoreAlgorythm
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Func<int, int> ScoreRule { get; set; }

        public int NoOfItemsConsidered { get; set; }
    }
}
