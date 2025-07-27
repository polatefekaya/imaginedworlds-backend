using System;
using Microsoft.AspNetCore.SignalR;

namespace ImaginedWorlds.Infrastructure;

public class ImaginedWorldsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}
