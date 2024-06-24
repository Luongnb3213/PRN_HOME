using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace PRN221_Assignment.Hubs
{
    public class chatHub: Hub
    {
        //public async Task SendMessage()
        //{
        //    var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        //    await Clients.All.SendAsync("ReceiveMessage", userId);
        //}
        public async Task SendMessage(int currentReactAfter, int threadID)
        {
            await Clients.Others.SendAsync("ReceiveMessage", currentReactAfter, threadID);
        }
    }
}
