using FunctionsApp.Models;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public interface IRocketMessageService
    {
        Task SaveRocketMessageToEventStore(RocketMessage rocketMessage);
    }
}