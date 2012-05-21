using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Core.Providers.DataTypes
{
    public class TopItem
    {
        public int Rank { get; set; }

        public int Position { get; set; }

        public int NumberOfPlays { get; set; }

        public string ItemName { get; set; }

        public ItemType ItemType { get; set; }

        public int Score { get; set; }

        public int EntryWeek { get; set; }
    }
}
