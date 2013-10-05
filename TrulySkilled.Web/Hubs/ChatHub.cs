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
        public static readonly ConcurrentDictionary<String, String> usersOnline = new ConcurrentDictionary<String, string>();

        /// <summary>
        /// Submits a chat message to all users in the chat room
        /// </summary>
        /// <param name="message">The message to submit</param>
        public void SubmitMessage(String message)
        {
            Clients.All.addMessage(Context.GetCurrentUserName(), message);
        }

        #region lifetime events
        public override Task OnConnected()
        {
            var username = Context.GetCurrentUserName();
            var connectionId = Context.ConnectionId;

            bool userIsOnline = false;
            usersOnline.AddOrUpdate(username, connectionId, (_1, _2) =>
            {
                userIsOnline = true;
                return connectionId;
            });

            if (!userIsOnline)
                Clients.Others.addUsers(new[] { username });

            Clients.Caller.addUsers(usersOnline.Keys.ToArray());

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            String username;
            usersOnline.TryRemove(Context.GetCurrentUserName(), out username);
            Clients.All.removeUser(Context.GetCurrentUserName());

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