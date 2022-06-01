using FunctionsApp.Models;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public interface IUpdateRocketStateService
    {
        Task UpdateRocketState(RocketMessage rocketMessage);
    }
}
