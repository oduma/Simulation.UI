using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using Simulation.DataAccess;

namespace Simulation.DataProviders
{
    public class AlgorythmPoolProvider:IAlgorythmPoolProvider
    {
        private IEnumerable<IScoreAlgorythm> _scoreAlgorythms;

        public IEnumerable<IScoreAlgorythm> GetAvailableScoreAlgorythms(ItemType itemType)
        {
            string currentlyInUse = GetCurrentlyInUse(itemType);
            if(_scoreAlgorythms==null)
                _scoreAlgorythms =  ClientFactory.GetClients<IScoreAlgorythm>().SetInUse(currentlyInUse);
            return _scoreAlgorythms;
        }

        private string GetCurrentlyInUse(ItemType itemType)
        {
            ISimulationCRUD simulationCRUD = ClientFactory.GetClient<ISimulationCRUD>();
            var existingItem = simulationCRUD.ListCurrentScoreAlgorythms().FirstOrDefault(s => s.ItemType == itemType);
            return (existingItem != null) ? existingItem.Name : string.Empty;
        }

        public void SetRule(CurrentScoreAlgorythm rule)
        {
            ISimulationCRUD simulationCRUD = ClientFactory.GetClient<ISimulationCRUD>();
            simulationCRUD.SaveCurrentScoreAlgorythm(rule);
        }

        public IScoreAlgorythm GetCurrentAlgorythm(ItemType currentItemType)
        {
            var availableAlgorhytms = (_scoreAlgorythms) ?? GetAvailableScoreAlgorythms(currentItemType);
            return (availableAlgorhytms.FirstOrDefault(a => a.InUse)) ?? availableAlgorhytms.First();
        }
    }
}
