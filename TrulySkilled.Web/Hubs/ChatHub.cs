using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrulySkilled.Web.Hubs
{
    public class ChatHub : Hub
    {
        protected static readonly ConcurrentDictionary<String, String> connectionIdToUsernameMap =
            new ConcurrentDictionary<string, string>();
        protected static readonly ConcurrentDictionary<String, String> usernameToGroupMap =
            new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Submits a chat message to all users in the chat room
        /// </summary>
        /// <param name="message">The message to submit</param>
        public void SubmitMessage(String message)
        {
            String group;
            if (usernameToGroupMap.TryGetValue(Context.GetCurrentUserName(), out group))
            {
                Clients.Group(group).addMessage(Context.GetCurrentUserName(), message);
            }
        }

        public async Task JoinChatGroup(String groupName)
        {
            usernameToGroupMap.AddOrUpdate(Context.GetCurrentUserName(), groupName, (_1, _2) => groupName);

            await Groups.Add(Context.ConnectionId, groupName);
        }

        #region lifetime events
        public override Task OnConnected()
        {
            var username = Context.GetCurrentUserName();
            var connectionId = Context.ConnectionId;

            connectionIdToUsernameMap.AddOrUpdate(connectionId, username, (_1, _2) => username);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            String username;
            if (connectionIdToUsernameMap.TryRemove(Context.ConnectionId, out username))
            {
                String group;
                usernameToGroupMap.TryRemove(username, out group);
            }

            return base.OnDisconnected();
        }
        #endregion
    }

    public static class Extensions
    {
        public static String GetCurrentUserName(this HubCallerContext context)
        {
            return context.User.Identity.Name;
        }
    }
}