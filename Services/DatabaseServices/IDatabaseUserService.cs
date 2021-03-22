﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualSports.BE.Services.DatabaseServices
{
    public interface IDatabaseUserService
    {
        Task<bool> RegisterUserAsync(string login, string password, CancellationToken cancellationToken);
        Task<Guid> LoginUserAsync(string login, string password, CancellationToken cancellationToken);
    }
}
