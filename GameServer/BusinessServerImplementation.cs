using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServerImplementation : BusinessServerInterface
    {
        private DataServerInterface foob;
        public BusinessServerImplementation() {
            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8200/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public List<Lobby> GetAllLobbies()
        {
            return foob.GetAllLobbies();
        }

        public List<User> GetUsers(Lobby lobby)
        {
            return foob.GetUsers(lobby);
        }

        public List<User> GetAllUsers()
        {
            return foob.GetAllUsers();
        }

        public void AddUser(User user)
        {
           foob.AddUser(user);
        }

        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
           return foob.GetUniqueModes(curLobbyList);
        }

        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return foob.GetUniqueTags(curLobbyList);
        }

        public List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null)
        {
            return foob.GetfilterdLobbiesList(mode, tag);
        }

        public List<string> GetAllModeTypes()
        {
            return foob.GetAllModeTypes();
        }

        public List<string> GetAllTagTypes()
        {
            return foob.GetAllTagTypes();
        }

        public void AddLobby(Lobby lobby)
        {
            foob.AddLobby(lobby);
        }
    }
}
