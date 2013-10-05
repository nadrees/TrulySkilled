using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TrulySkilled.Web.Hubs
{
    public class GameLobbyHub : ChatHub
    {
        public static Dictionary<String, String> challenges = new Dictionary<string, string>();

        /// <summary>
        /// Issues a challenge to the target user
        /// </summary>
        /// <param name="username">The user being challenged by the current user</param>
        public void Challenge(String username)
        {
            String connectionId;
            usersOnline.TryGetValue(username, out connectionId);

            if (connectionId != null)
            {
                String currentUserName = Context.GetCurrentUserName();

                if (!challenges.ContainsKey(currentUserName))
                {
                    challenges.Add(currentUserName, username);

                    Clients.Client(connectionId).addChallenge(currentUserName);
                    Clients.Caller.challengeSent(username);
                }
            }
        }

        /// <summary>
        /// Cancels a challenge to the target user.
        /// </summary>
        /// <param name="username">The user who's challenge is being canceled.</param>
        public void CancelChallenge()
        {
            String currentUserName = Context.GetCurrentUserName();

            if (challenges.ContainsKey(currentUserName))
            {
                String otherUserName = challenges[currentUserName];
                challenges.Remove(currentUserName);

                if (usersOnline.ContainsKey(otherUserName))
                {
                    var connectionId = usersOnline[otherUserName];
                    Clients.Client(connectionId).cancelChallenge(currentUserName);
                }
            }
        }

        /// <summary>
        /// Rejects another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void RejectChallenge(String username)
        {
            if (challenges.ContainsKey(username))
            {
                challenges.Remove(username);

                if (usersOnline.ContainsKey(username))
                {
                    var connectionId = usersOnline[username];
                    Clients.Client(connectionId).rejectChallenge();
                }
            }
        }

        /// <summary>
        /// Accepts another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void AcceptChallenge(String username)
        {
            if (challenges.ContainsKey(username))
            {
                challenges.Remove(username);

                String randomGuid = Guid.NewGuid().ToString();
                if (usersOnline.ContainsKey(username))
                {
                    var connectionId = usersOnline[username];
                    Clients.Client(connectionId).acceptChallenge(randomGuid);
                    Clients.Caller.acceptChallenge(randomGuid);
                }
            }
        }

        #region Lifetime Events
        public override Task OnDisconnected()
        {
            CancelChallenge(); // cancel any challenges the current user had pending

            return base.OnDisconnected();
        }
        #endregion
    }
}