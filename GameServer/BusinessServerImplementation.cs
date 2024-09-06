using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Runtime.CompilerServices;
using System.IO;
using System.Collections.Concurrent;

namespace BusinessLayer    
{
    

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServerImplementation : IBusinessServerInterface
    {
        private IDataServerInterface foob;
        private static uint logNumber = 0;
        public BusinessServerImplementation() {

            ChannelFactory<IDataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            

            string URL = "net.tcp://localhost:8200/DataService";
            foobFactory = new ChannelFactory<IDataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            
        }

        public List<Lobby> GetAllLobbies()
        {
            Log($"Retriving Lobbies from data layer");
            List<Lobby> lobbyList = foob.GetAllLobbies();
            foreach (Lobby lob in lobbyList)
            {
                Log($"Lobby {lob.Name}, {lob.ID}");
            }
            return lobbyList;
        }

        public List<User> GetUsers(Lobby lobby)
        {
            return foob.GetUsers(lobby);
        }

        public void RemoveUser(Lobby lobby, User user)
        {
            foob.RemoveUser(lobby, user);
        }

        public List<User> GetAllUsers()
        {
            return foob.GetAllUsers();
        }

        public void AddUser(User user)
        {
            Log($"Added new user: {user.Name}");
            foob.AddUser(user);
            OperationContext.Current.GetCallbackChannel<ProcessServiceCallBack>().UpdateUserCount(foob.GetAllUsers().Count());
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
            Log($"New lobby created: {lobby.Name}");
            foob.AddLobby(lobby);
        }

        public bool UniqueUser(string userName)
        {
            Log($"Attempted Login with username {userName}");
            List<User> users = foob.GetAllUsers();
            
            foreach (User user in users)
            {
                if (user.Name == userName)
                {
                    Log($"Login failed {userName} already used");
                    return false;
                }
            }
            Log($"Login for {userName} successful ");
            return true;
        }

        public List<Message> getChats(Lobby lobby, User currUser)
        {
            return foob.GetChats(lobby, currUser);
        }

        

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UploadFile(byte[] fileData, string fileName)
        {
            Log($"Recieved file upload: {fileName}");

            foob.saveFile(fileName, fileData);
        }

        public void DownloadFile()
        {
            Log($"File download by ?");
        }

        public void UpdateMessage(Message msg)
        {
            foob.UpdateMessage(msg);
        }

        public void joinLobby(Lobby lobby, User user)
        {
            foob.joinLobby(lobby,user);
        }
    }
}
