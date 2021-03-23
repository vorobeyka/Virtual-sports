using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualSports.BE.Models.DatabaseModels;

namespace VirtualSports.BE.Services
{
    public interface IDiceService
    {
        public Task<bool> GetBetResultAsync(int diceRoll, BetType betType);
    }
}
