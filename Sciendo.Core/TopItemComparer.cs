using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Sciendo.Core
{
    public class TopItemComparer:IComparer<TopItem>
    {
        public int Compare(TopItem x, TopItem y)
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
