using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ServerImplementation : ServerInterface
    {
        Database database;
        public ServerImplementation() {
            database = new Database();
        }

        public List<Lobby> getLobbys()
        {
            return database.getAllLobbies();
        }

        public List<User> getUsers(Lobby lobby)
        {
            return database.getLobbyUsers(lobby);
        }
    }
}
