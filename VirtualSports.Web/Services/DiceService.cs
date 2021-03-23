using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services
{
    public class DiceService : IDiceService
    {
        public Task<bool> GetBetResultAsync(int diceRoll, BetType betType)
        {
            switch (betType)
            {
                case BetType.EVEN:
                    return diceRoll % 2 != 0
                        ? Task.FromResult(true)
                        : Task.FromResult(false);

                case BetType.ODD:
                    return diceRoll % 2 == 0
                        ? Task.FromResult(true)
                        : Task.FromResult(false);

                default: return diceRoll == (int)betType 
                        ? Task.FromResult(true)
                        : Task.FromResult(false);
            }
        }
    }
}
