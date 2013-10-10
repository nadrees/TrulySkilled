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
        public void Challenge(String username)
        {
            /*
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
             */
        }

        /// <summary>
        /// Cancels a challenge to the target user.
        /// </summary>
        /// <param name="username">The user who's challenge is being canceled.</param>
        public void CancelChallenge()
        {
            /*
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
             */
        }

        /// <summary>
        /// Rejects another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void RejectChallenge(String username)
        {
            /*
            String _;
            if (challenges.TryRemove(username, out _))
            {
                String connectionId;
                if (usersOnline.TryGetValue(username, out connectionId))
                {
                    Clients.Client(connectionId).rejectChallenge();
                }
            }
             */
        }

        /// <summary>
        /// Accepts another user's challenge
        /// </summary>
        /// <param name="username">The username of the user who originally issued the challenge</param>
        public void AcceptChallenge(String username)
        {
            /*
            String _;
            if (challenges.TryRemove(username, out _))
            {
                Guid gameId = Guid.NewGuid();
                String connectionId;
                if (usersOnline.TryGetValue(username, out connectionId))
                {
                    new T().RegisterGame(gameId, new[] { username, Context.GetCurrentUserName() });

                    String randomGuid = gameId.ToString();
                    Clients.Client(connectionId).acceptChallenge(randomGuid);
                    Clients.Caller.acceptChallenge(randomGuid);
                }
            }
             */
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