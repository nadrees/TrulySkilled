using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrulySkilled.Web.Controllers;

namespace TrulySkilled.Web.Hubs
{
    public abstract class LobbyHub<T> : ChatHub where T : IGameController, new()
    {
        private String gameName;

        protected static ConcurrentDictionary<String, String> challenges = 
            new ConcurrentDictionary<string, string>();
        protected static ConcurrentDictionary<String, UsersInfo> lobbyToUsersMap =
            new ConcurrentDictionary<string, UsersInfo>();

        public LobbyHub(String gameName)
        {
            this.gameName = gameName + "-lobby";
        }

        /// <summary>
        /// Adds the current user to the current game lobby
        /// </summary>
        /// <param name="game"></param>
        protected async Task JoinGameLobby()
        {
            var users = lobbyToUsersMap.GetOrAdd(gameName, new UsersInfo());

            if (users.AddUser(Context.GetCurrentUserName(), Context.ConnectionId))
            {                
                Clients.Group(gameName).addUsers(new[] { 
                    new User(Context.GetCurrentUserName() , Context.ConnectionId)
                });
                await Groups.Add(Context.ConnectionId, gameName);
                Clients.Caller.addUsers(users.GetUsers());
            }
        }

        /// <summary>
        /// Issues a challenge to the target user
        /// </summary>
        /// <param name="username">The user being challenged by the current user</param>
        public void Challenge(String connectionId)
        {
            bool alreadyHasChallenge = false;
            challenges.AddOrUpdate(Context.ConnectionId, connectionId, (_1, currentChallenge) =>
            {
                alreadyHasChallenge = true;
                return currentChallenge;
            });

            String username;
            if (!alreadyHasChallenge && connectionIdToUsernameMap.TryGetValue(connectionId, out username))
            {
                Clients.Client(connectionId).addChallenge(new User(Context.GetCurrentUserName(), Context.ConnectionId));
                Clients.Caller.challengeSent(username);
            }
        }

        /// <summary>
        /// Cancels a challenge to the target user.
        /// </summary>
        /// <param name="username">The user who's challenge is being canceled.</param>
        public void CancelChallenge()
        {
            String challengedConnectionId;
            if (challenges.TryRemove(Context.ConnectionId, out challengedConnectionId))
            {
                Clients.Client(challengedConnectionId).cancelChallenge(Context.GetCurrentUserName());
            }
        }

        /// <summary>
        /// Rejects another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void RejectChallenge(String connectionId)
        {
            String _;
            if (challenges.TryRemove(connectionId, out _))
            {
                Clients.Client(connectionId).rejectChallenge();
            }
        }

        /// <summary>
        /// Accepts another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void AcceptChallenge(String connectionId)
        {
            String _;
            if (challenges.TryRemove(connectionId, out _))
            {
                Guid gameId = Guid.NewGuid();

                String otherUsername;
                if (connectionIdToUsernameMap.TryGetValue(connectionId, out otherUsername))
                {
                    new T().RegisterGame(gameId, new[] { otherUsername, Context.GetCurrentUserName() });

                    String randomGuid = gameId.ToString();
                    Clients.Client(connectionId).acceptChallenge(randomGuid);
                    Clients.Caller.acceptChallenge(randomGuid);
                }
            }
        }

        #region Lifetime Events
        public override Task OnDisconnected()
        {
            CancelChallenge(); // cancel any challenges the current user had pending

            UsersInfo users;
            if (lobbyToUsersMap.TryGetValue(gameName, out users))
            {
                if (users.RemoveUser(Context.ConnectionId))
                {
                    Clients.OthersInGroup(gameName).removeUser(Context.GetCurrentUserName());
                }
            }

            return base.OnDisconnected();
        }
        #endregion

        protected class UsersInfo
        {
            // using instance variable as lock to ensure multiple instances
            // of this class don't block each other
            private Object lockObj = new Object();

            private List<User> users = new List<User>();

            public bool AddUser(String username, String connectionId)
            {
                lock (lockObj)
                {
                    if (users.Any(u => u.Username == username))
                        return false;

                    users.Add(new User(username, connectionId));
                    return true;
                }
            }

            public bool RemoveUser(String connectionId)
            {
                lock (lockObj)
                {
                    return users.RemoveAll(u => u.ConnectionId == connectionId) > 0;
                }
            }

            public IEnumerable<User> GetUsers()
            {
                lock (lockObj)
                {
                    return users.ToList();
                }
            }
        }

        protected class User
        {
            public User(string username, string connectionId)
            {
                this.Username = username;
                this.ConnectionId = connectionId;
            }

            public String Username { get; private set; }
            public String ConnectionId { get; private set; }
        }
    }
}