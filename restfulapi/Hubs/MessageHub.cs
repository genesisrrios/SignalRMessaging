using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.HubInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Restfulapi.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        public async Task ReceiveMessage(MessageReceivedDTO message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
