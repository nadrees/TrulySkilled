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
        public static readonly HashSet<String> usersOnline = new HashSet<string>();

        public void SubmitMessage(String message)
        {
            Clients.All.addMessage(Context.User.Identity.Name, message);
        }

        #region lifetime events
        public override Task OnConnected()
        {
            var username = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;

            if (!usersOnline.Contains(username))
            {
                Clients.AllExcept(connectionId).addUsers(new[] { username });
                usersOnline.Add(username);
            }

            Clients.Caller.addUsers(usersOnline.ToArray());

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            Clients.All.removeUser(Context.User.Identity.Name);

            return base.OnDisconnected();
        }
        #endregion
    }
}