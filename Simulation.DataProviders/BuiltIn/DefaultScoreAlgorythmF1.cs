using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;

namespace Simulation.DataProviders.BuiltIn
{
    public class DefaultScoreAlgorythmF1:IScoreAlgorythm
    {
        public string Name
        {
            get { return "F1"; }
        }

        public string Description
        {
            get { return "10,8,6,5,4,3,2,1"; }
        }


        public static int RuleF1(int rank)
        {
            switch (rank)
            {
                case 1:
                    return 10;
                case 2:
                    return 8;
                case 3:
                    return 6;
                case 4:
                    return 5;
                case 5:
                    return 4;
                case 6:
                    return 3;
                case 7:
                    return 2;
                case 8:
                    return 1;
                default:
                    return 0;
            }
        }

        public Func<int, int> ScoreRule
        {
            get { return RuleF1; }
        }

        public int NoOfItemsConsidered
        {
            get { return 8; }
        }

        public bool InUse
        {
            get;set;
        }
    }
}
