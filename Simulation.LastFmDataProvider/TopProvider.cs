using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Core.Providers;
using Sciendo.Core.Providers.DataTypes;
using Sciendo.Core;

namespace Simulation.LastFmDataProvider
{
    public class TopProvider:ITopProvider
    {
        public WeeklyTop GetTopByWeek(int weekNo, int topLength, ItemType itemType)
        {
            throw new NotImplementedException();
        }

        public bool TryGetAuthorizationToken(out string token, out string error)
        {
  
            var response = Utility.Deserialize<LfmTokenResponse>(HttpHelper.Get("http://ws.audioscrobbler.com/2.0/?method=auth.gettoken&api_key=5e625305596ba928b8d8664bd2a95b08"));
            
            if (response.Status == "OK")
            {
                token = "token";
                error = "OK";
                return true;
            }
            token = string.Empty;
            error = response.Status;
            return false;
        }
    }
}
