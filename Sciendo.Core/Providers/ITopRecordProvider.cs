using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Sciendo.Core.Providers
{
    public interface ITopRecordProvider
    {
        IEnumerable<WeekSummary> GetTopProcessed();
    }
}
