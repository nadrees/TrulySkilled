using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrulySkilled.Web.Controllers
{
    public interface IGameController
    {
        void RegisterGame(Guid gameId, IEnumerable<String> players);
        void RecordResults(Dictionary<String, int> playerRanks);
    }
}
