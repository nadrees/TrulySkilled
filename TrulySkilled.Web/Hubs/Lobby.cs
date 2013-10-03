using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace TrulySkilled.Web.Hubs
{
    public class Lobby : Hub
    {
        public static readonly Dictionary<String, String> usersOnline = new Dictionary<String, string>();

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

            if (!usersOnline.ContainsKey(username))
            {
                Clients.Others.addUsers(new[] { username });
                usersOnline.Add(username, connectionId);
            }

            Clients.Caller.addUsers(usersOnline.Keys.ToArray());

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            usersOnline.Remove(Context.GetCurrentUserName());
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