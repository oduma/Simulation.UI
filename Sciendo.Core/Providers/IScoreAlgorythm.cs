using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.Providers
{
    public interface IScoreAlgorythm
    {
        string Name { get;}

        string Description { get; }

        Func<int, int> ScoreRule { get; }

        int NoOfItemsConsidered { get; }

        bool InUse { get; set; }

    }
}
