﻿using System.Threading.Tasks;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Services
{
    public interface IDiceService
    {
        public Task<bool> GetBetResultAsync(int diceRoll, BetType betType);
    }
}