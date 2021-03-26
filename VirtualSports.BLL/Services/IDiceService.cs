using System.Threading.Tasks;
using VirtualSports.DAL.Models;

namespace VirtualSports.BLL.Services
{
    public interface IDiceService
    {
        public Task<bool> GetBetResultAsync(int diceRoll, BetType betType);
    }
}
