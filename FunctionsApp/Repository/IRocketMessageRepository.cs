using FunctionsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionsApp.Repository
{
    public interface IRocketMessageRepository
    {
        Task<IEnumerable<RocketMessage>> GetRocketMessages(string rocketId);
    }
}