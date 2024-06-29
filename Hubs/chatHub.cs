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
        // Method to join a group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Method to leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
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
        public async Task SendMessageOneUser(int partnerIdSignalR, string messContent, string avtAuthor, string ReceiveName)
        {
            await Clients.User(partnerIdSignalR + "").SendAsync("ReceiveMessageOneUser", partnerIdSignalR, messContent, avtAuthor, ReceiveName);
        }
        public async Task SendMessageToGroup(string groupName, int AuthorId, string content, string avt, string userName, string groupImg, string groupId)
        {
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveGroupMessage", AuthorId, content, avt, userName, groupImg, groupName, groupId);
        }
    }
}
