using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation.DataAccess;
using Sciendo.Core.Providers.DataTypes;

namespace Tests.Mocks
{
    public class MockSimulationCRUD:ISimulationCRUD
    {
        public MockSimulationCRUD()
        {
            Repository.Clear();
            RecordRepository.Clear();
            TotalRepository.Clear();
        }

        public List<CurrentScoreAlgorythm> Repository = new List<CurrentScoreAlgorythm>();

        public List<WeekSummary> RecordRepository = new List<WeekSummary>();

        public List<TopItem> TotalRepository = new List<TopItem>();

        public List<CurrentScoreAlgorythm> ListCurrentScoreAlgorythms()
        {
            return Repository;
        }

        public void SaveCurrentScoreAlgorythm(CurrentScoreAlgorythm currentScoreAlgorythm)
        {
            if (Repository.FirstOrDefault(r => r.ItemType == currentScoreAlgorythm.ItemType) != null)
                Repository.First(r => r.ItemType == currentScoreAlgorythm.ItemType).Name = currentScoreAlgorythm.Name;
            else
                Repository.Add(currentScoreAlgorythm);
        }


        public List<WeekSummary> GetTopProcessed()
        {
            return RecordRepository;
        }

        public void SaveRecordedWeeks(List<WeekSummary> weekSummaries)
        {
            RecordRepository = weekSummaries;
        }


        public List<TopItem> ListTotalItems(ItemType itemType)
        {
            return TotalRepository.Where(t => t.ItemType == itemType).ToList();
        }

        public void SaveTotalItems(ItemType itemType, List<TopItem> topItems)
        {
            TotalRepository = topItems;
        }
    }
}
