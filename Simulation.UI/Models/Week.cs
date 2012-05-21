using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation.UI.Models
{
    public class Week:WeekSummary
    {

        public DateTime StartingFrom { get; set; }

        public DateTime EndingIn { get; set; }

        public string DisplayValue { get { return string.Format("({0:dd/MMM/yyyy} - {1:dd/MMM/yyyy})", StartingFrom,EndingIn); } }

        public string SeasonImage { 
            get 
            {
                switch(StartingFrom.Month)
                {
                    case 1:
                    case 2:
                    case 12:
                        return "cloud-snow-icon.png";
                    case 3:
                    case 4:
                    case 5:
                        return "cloud-rainbow-icon.png";
                    case 6:
                    case 7:
                    case 8:
                        return "cloud-sun-icon.png";
                    case 9:
                    case 10:
                    case 11:
                        return "cloud-rain-icon.png";
                }
                return string.Empty;
            } 
        }
    }
}
