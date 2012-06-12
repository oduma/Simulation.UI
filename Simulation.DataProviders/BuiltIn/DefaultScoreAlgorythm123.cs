using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;

namespace Simulation.DataProviders.BuiltIn
{
    public class DefaultScoreAlgorythm123:IScoreAlgorythm
    {

        public string Name
        {
            get { return "Top 3 (3,2,1)"; }
        }

        public string Description
        {
            get { return "First gets 3 points, Second gets 2 points, Third gets 1 point"; }
        }


        public static int Rule321(int rank)
        {
            switch (rank)
            {
                case 1:
                    return 3;
                case 2:
                    return 2;
                case 3:
                    return 1;
                default:
                    return 0;
            }
        }

        public Func<int, int> ScoreRule
        {
            get { return Rule321; }
        }

        public int NoOfItemsConsidered
        {
            get { return 3; }
        }

        public bool InUse
        {
            get;
            set;
        }
    }
}
