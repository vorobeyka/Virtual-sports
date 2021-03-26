using System.Threading.Tasks;

namespace VirtualSports.BLL.Services
{
    public class DiceService : IDiceService
    {
        public Task<bool> GetBetResultAsync(int diceRoll, BetType betType)
        {
            switch (betType)
            {
                case BetType.EVEN:
                    return diceRoll % 2 == 0
                        ? Task.FromResult(true)
                        : Task.FromResult(false);

                case BetType.ODD:
                    return diceRoll % 2 != 0
                        ? Task.FromResult(true)
                        : Task.FromResult(false);

                default: return (diceRoll - 1) == (int)betType 
                        ? Task.FromResult(true)
                        : Task.FromResult(false);
            }
        }
    }
}
