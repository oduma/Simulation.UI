using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;

namespace Simulation.DataProviders
{
    public static class Extensions
    {
        public static IEnumerable<IScoreAlgorythm> SetInUse(this IEnumerable<IScoreAlgorythm> scoreAlgorythms, string currentlyInUse)
        {
            foreach (IScoreAlgorythm scorealgorythm in scoreAlgorythms)
            {
                scorealgorythm.InUse = (currentlyInUse == scorealgorythm.Name);
                yield return scorealgorythm;
            }
        }
    }
}
