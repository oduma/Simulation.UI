using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation.UI.Models
{
    public class TopItem
    {
        public int Rank { get; set; }

        public int Position { get; set; }

        public int NumberOfPlays { get; set; }

        public string ItemName { get; set; }

        public string ItemImage { get 
        {
            if(ItemType==TopItemType.Artist)
                return "chorus-icon.png";
            if(ItemType==TopItemType.Album)
                return "vinyl-icon.png";
            return "";
        } 
        }

        public TopItemType ItemType { get; set; }
    }

    public enum TopItemType
    {
        None=0,
        Artist,
        Album
    }
}
