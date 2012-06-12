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
        public List<CurrentScoreAlgorythm> Repository = new List<CurrentScoreAlgorythm>();

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
    }
}
