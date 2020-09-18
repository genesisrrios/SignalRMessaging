using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.HubInterfaces
{
    public interface IMessageClient
    {
        Task ReceiveMessage(MessageReceivedDTO message);
    }
}
