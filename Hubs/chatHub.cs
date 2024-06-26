using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
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
        public static Dictionary<string, string> userConnections = new Dictionary<string, string>();
        public async Task SendMessage(int currentReactAfter, int threadID)
        {
            await Clients.Others.SendAsync("ReceiveMessage", currentReactAfter, threadID);
        }
        public void RegisterConnection(string userId, string connectionId)
        {
            userConnections[userId] = connectionId;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != null)
            {
                userConnections.Remove(userId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessageOneUser(int partnerId, string messContent)
        {
            await Clients.User(partnerId + "").SendAsync("ReceiveMessageOneUser", partnerId, messContent);
        }
    }
}
