using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation.UI.Models;

namespace Simulation.UI.Controllers
{
    internal class TotalItemComparer: IComparer<TotalItem>
    {
        public int Compare(TotalItem x, TotalItem y)
        {
            if (y == null)
            {
                if (x == null)
                    return 0;
                else
                {
                    return -1;
                }
            }
            else
            {
                if (x == null)
                    return 1;
                return y.Score.CompareTo(x.Score);
            }
        }
    }
}
