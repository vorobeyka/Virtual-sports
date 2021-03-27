using System.Threading.Tasks;
using VirtualSports.Lib.Models;

namespace VirtualSports.BLL.Services
{
    public interface IDiceService
    {
        public Task<bool> GetBetResultAsync(int diceRoll, BetType betType);
    }
}
