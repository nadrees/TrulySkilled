using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrulySkilled.Web.Hubs
{
    public class GameLobbyHub : ChatHub
    {
        public static ConcurrentDictionary<String, String> challenges = new ConcurrentDictionary<string, string>();

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

                bool hasChallenge = false;
                challenges.AddOrUpdate(currentUserName, username, (_, existingUserName) =>
                {
                    hasChallenge = true;
                    return existingUserName;
                });

                if (!hasChallenge)
                {
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

            String challengedUser;
            if (challenges.TryRemove(currentUserName, out challengedUser) && challengedUser != null)
            {
                String connectionId;
                if (usersOnline.TryGetValue(challengedUser, out connectionId))
                {
                    Clients.Client(connectionId).cancelChallenge(challengedUser);
                }
            }
        }

        /// <summary>
        /// Rejects another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void RejectChallenge(String username)
        {
            String _;
            if (challenges.TryRemove(username, out _))
            {
                String connectionId;
                if (usersOnline.TryGetValue(username, out connectionId))
                {
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
            String _;
            if (challenges.TryRemove(username, out _))
            {
                String randomGuid = Guid.NewGuid().ToString();
                String connectionId;
                if (usersOnline.TryGetValue(username, out connectionId))
                {
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