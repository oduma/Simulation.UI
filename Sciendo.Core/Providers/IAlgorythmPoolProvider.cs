using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers.DataTypes;

namespace Sciendo.Core.Providers
{
    public interface IAlgorythmPoolProvider
    {
        IEnumerable<ScoreAlgorythm> GetAvailableScoreAlgorythms(ItemType itemType);

        bool SetRule(NewRule rule);

        ScoreAlgorythm GetCurrentAlgorythm(ItemType currentItemType);
    }
}
