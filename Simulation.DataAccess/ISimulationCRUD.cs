using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Simulation.DataAccess
{
    public interface ISimulationCRUD
    {
        List<CurrentScoreAlgorythm> ListCurrentScoreAlgorythms();

        void SaveCurrentScoreAlgorythm(CurrentScoreAlgorythm currentScoreAlgorythm);

        List<WeekSummary> GetTopProcessed();

        void SaveRecordedWeeks(List<WeekSummary> weekSummaries);

        List<TopItem> ListTotalItems(ItemType itemType);

        void SaveTotalItems(ItemType itemType, List<TopItem> topItems);
    }
}
