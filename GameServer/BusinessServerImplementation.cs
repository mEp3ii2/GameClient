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
using System.Security.Cryptography;

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
            return foob.GetAllLobbies(); ;
        }

        public List<string> GetUsers(string lobbyName)
        {
            Lobby lobby = foob.GetLobby(lobbyName);
            List<string> userNames = new List<string>();
            foreach (User user in foob.GetUsers(lobby))
            {
                userNames.Add(user.Name);
            }
            return userNames;
        }

        public User GetUser(string name)
        {
            return foob.GetUser(name);
        }

        public void RemoveUserFromLobby(string lobbyName, string userName)
        {
            User user = foob.GetUser(userName);
            Lobby lobby = foob.GetLobby(lobbyName);
            foob.RemoveUserFromLobby(lobby, user);
        }

        public void AddMessage(Lobby lobby, User user1, User user2)
        {
            foob.AddMessage(lobby, user1, user2);
        }

        public void RemoveUser(string userName)
        {
            User user = foob.GetUser(userName);
            foob.RemoveUser(user);
        }

        public List<User> GetAllUsers()
        {
            return foob.GetAllUsers();
        }

        public void AddUser(string userName)
        {
            Log($"Added new user: {userName}");
            foob.AddUser(new User(userName));
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

        public void AddLobby(string roomName, string desc, string mode, List<string> tags)
        {
            Log($"New lobby created: {roomName}");
            Lobby lobby = new Lobby(roomName, desc, mode, tags);
            foob.AddLobby(lobby);
        }

        public bool UniqueUser(string userName)
        {
            Log($"Attempted Login with username {userName}");
            List<User> users = foob.GetAllUsers();
            
            foreach (User user in users)
            {
                if (user.Name.Equals(userName))
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
        public void UploadFile(byte[] fileData, string fileName, string lobbyName)
        {
            Lobby lobby = foob.GetLobby(lobbyName);
            // Delegate to the data server with the lobby name
            foob.saveFile(fileName, fileData, lobby);
        }

        // Download a file from the server
        public byte[] DownloadFile(string fileName)
        {
            Log($"Received file download request: {fileName}");
            return foob.downloadFile(fileName);
        }

        public void UpdateMessage(List<string> messageText, string lobbyName, string userName1, string userName2)
        {
            User user1 = foob.GetUser(userName1 );
            User user2 = foob.GetUser(userName2 );
            Lobby lobby = foob.GetLobby(lobbyName);
            Message msg = foob.GetMessage(user1 , user2, lobby);
            msg.MessageList = messageText;
            foob.UpdateMessage(msg, lobby);
        }

        public void joinLobby(string lobbyName, string userName)
        {
            User user = foob.GetUser(userName);
            Lobby lobby = foob.GetLobby(lobbyName);
            foob.joinLobby(lobby,user);
        }

        public List<string> GetMessage(string userName1, string userName2, string lobbyName)
        {
            User user1 = foob.GetUser(userName1);
            User user2 = foob.GetUser(userName2);
            Lobby lobby = foob.GetLobby(lobbyName);
            Message message = foob.GetMessage(user1 ,user2, lobby);
            return message.MessageList;
        }

        // Fetch previously uploaded files for the specified lobby
        public List<string> GetLobbyFiles(string lobbyName)
        {
            Lobby lobby = foob.GetLobby(lobbyName);
            return foob.GetLobbyFiles(lobby);  // Delegate the call to the Data Layer
        }

        public int GetUserCount()
        {
            return foob.GetUserCount();
        }
    }
}
