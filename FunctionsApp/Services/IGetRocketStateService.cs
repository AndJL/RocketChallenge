using FunctionsApp.Models;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public interface IGetRocketStateService
    {
        Task<RocketStateExtended> GetRocketState(string rocketId, bool extended = false);
    }
}