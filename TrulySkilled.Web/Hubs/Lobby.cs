using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace TrulySkilled.Web.Hubs
{
    public class Lobby : Hub
    {
        public static readonly Dictionary<String, String> usersOnline = new Dictionary<String, string>();

        public void SubmitMessage(String message)
        {
            Clients.All.addMessage(Context.User.Identity.Name, message);
        }

        public void Challenge(String username)
        {
            if (usersOnline.ContainsKey(username))
            {
                var connectionId = usersOnline[username];
                Clients.Client(connectionId).addChallenge(username);
                Clients.Caller.challengeSent();
            }
        }

        #region lifetime events
        public override Task OnConnected()
        {
            var username = Context.User.Identity.Name;
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
            usersOnline.Remove(Context.User.Identity.Name);
            Clients.All.removeUser(Context.User.Identity.Name);

            return base.OnDisconnected();
        }
        #endregion
    }
}