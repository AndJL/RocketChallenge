using FunctionsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public interface IGetRocketListService
    {
        Task<List<RocketState>> GetRocketList();
    }
}
