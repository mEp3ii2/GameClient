/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: BusinessServerImplementation
Purpose: Implements the business logic for the gaming lobby server, allowing interaction with the data server.
Notes: Handles various operations like user management, messaging, file sharing, etc.
*/

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

        /**
        Method: BusinessServerImplementation (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the connection to the DataServer via the IDataServerInterface.
        Algorithm: Uses ChannelFactory to establish a connection to the data server at a specified URL.
        */
        public BusinessServerImplementation()
        {
            ChannelFactory<IDataServerInterface> foobFactory;
            NetTcpBinding tcp = BindingHelper.CreateLargeFileBinding();
            string URL = "net.tcp://localhost:8200/DataService";
            foobFactory = new ChannelFactory<IDataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        /**
        Method: GetAllLobbies
        Imports: None
        Exports: List<Lobby> (List of all lobbies)
        Notes: Retrieves all lobbies from the data server.
        Algorithm: Calls the data layer to get all lobbies and logs the retrieval process.
        */
        public List<Lobby> GetAllLobbies()
        {
            Log($"Retriving Lobbies from data layer");
            return foob.GetAllLobbies();
        }

        /**
        Method: GetUsers
        Imports: string lobbyName (Name of the lobby)
        Exports: List<string> (List of usernames)
        Notes: Retrieves all users in a specified lobby.
        Algorithm: Fetches the lobby from the data layer and extracts the list of usernames.
        */
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

        /**
        Method: GetUser
        Imports: string name (Username)
        Exports: User (User object)
        Notes: Retrieves a user object based on the username.
        Algorithm: Calls the data layer to get the user by name.
        */
        public User GetUser(string name)
        {
            return foob.GetUser(name);
        }

        /**
        Method: RemoveUserFromLobby
        Imports: string lobbyName (Name of the lobby), string userName (Username)
        Exports: None
        Notes: Removes a user from the specified lobby.
        Algorithm: Fetches the user and lobby, then calls the data layer to remove the user from the lobby.
        */
        public void RemoveUserFromLobby(string lobbyName, string userName)
        {
            User user = foob.GetUser(userName);
            Lobby lobby = foob.GetLobby(lobbyName);
            foob.RemoveUserFromLobby(lobby, user);
        }

        /**
        Method: AddMessage
        Imports: Lobby lobby (Lobby object), User user1 (First user), User user2 (Second user)
        Exports: None
        Notes: Adds a message between two users in the specified lobby.
        Algorithm: Calls the data layer to add a message between the users in the specified lobby.
        */
        public void AddMessage(Lobby lobby, User user1, User user2)
        {
            foob.AddMessage(lobby, user1, user2);
        }

        /**
        Method: RemoveUser
        Imports: string userName (Username)
        Exports: None
        Notes: Removes a user from the system.
        Algorithm: Fetches the user and calls the data layer to remove them.
        */
        public void RemoveUser(string userName)
        {
            User user = foob.GetUser(userName);
            foob.RemoveUser(user);
        }

        /**
        Method: GetAllUsers
        Imports: None
        Exports: List<User> (List of all users)
        Notes: Retrieves all users from the data layer.
        Algorithm: Calls the data layer to get all users.
        */
        public List<User> GetAllUsers()
        {
            return foob.GetAllUsers();
        }

        /**
        Method: AddUser
        Imports: string userName (Username)
        Exports: None
        Notes: Adds a new user to the system and logs the addition.
        Algorithm: Logs the addition and calls the data layer to add the user.
        */
        public void AddUser(string userName)
        {
            Log($"Added new user: {userName}");
            foob.AddUser(new User(userName));
        }

        /**
        Method: GetUniqueModes
        Imports: List<Lobby> curLobbyList (Current list of lobbies)
        Exports: List<string> (List of unique game modes)
        Notes: Retrieves the unique game modes from the current list of lobbies.
        Algorithm: Calls the data layer to get unique modes.
        */
        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            return foob.GetUniqueModes(curLobbyList);
        }

        /**
        Method: GetUniqueTags
        Imports: List<Lobby> curLobbyList (Current list of lobbies)
        Exports: List<string> (List of unique tags)
        Notes: Retrieves the unique tags from the current list of lobbies.
        Algorithm: Calls the data layer to get unique tags.
        */
        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return foob.GetUniqueTags(curLobbyList);
        }

        /**
        Method: GetfilterdLobbiesList
        Imports: string mode (Game mode), string tag (Lobby tag)
        Exports: List<Lobby> (List of filtered lobbies)
        Notes: Retrieves a filtered list of lobbies based on mode and/or tag.
        Algorithm: Calls the data layer to filter the lobbies by mode and tag.
        */
        public List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null)
        {
            return foob.GetfilterdLobbiesList(mode, tag);
        }

        /**
        Method: GetAllModeTypes
        Imports: None
        Exports: List<string> (List of all available game modes)
        Notes: Retrieves all available game modes from the data layer.
        Algorithm: Calls the data layer to get the game modes.
        */
        public List<string> GetAllModeTypes()
        {
            return foob.GetAllModeTypes();
        }

        /**
        Method: GetAllTagTypes
        Imports: None
        Exports: List<string> (List of all available lobby tags)
        Notes: Retrieves all available lobby tags from the data layer.
        Algorithm: Calls the data layer to get the lobby tags.
        */
        public List<string> GetAllTagTypes()
        {
            return foob.GetAllTagTypes();
        }

        /**
        Method: AddLobby
        Imports: string roomName (Lobby name), string desc (Description), string mode (Game mode), List<string> tags (Tags for the lobby)
        Exports: None
        Notes: Adds a new lobby to the system and logs the creation.
        Algorithm: Logs the creation and calls the data layer to add the lobby.
        */
        public void AddLobby(string roomName, string desc, string mode, List<string> tags)
        {
            Log($"New lobby created: {roomName}");
            Lobby lobby = new Lobby(roomName, desc, mode, tags);
            foob.AddLobby(lobby);
        }

        /**
        Method: UniqueUser
        Imports: string userName (Username)
        Exports: bool (True if username is unique, otherwise false)
        Notes: Checks if the username is unique.
        Algorithm: Iterates through all users and returns false if a match is found, otherwise true.
        */
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

        /**
        Method: getChats
        Imports: Lobby lobby (Lobby object), User currUser (Current user)
        Exports: List<Message> (List of chat messages)
        Notes: Retrieves the chat messages for the specified lobby and user.
        Algorithm: Calls the data layer to get the chat messages.
        */
        public List<Message> getChats(Lobby lobby, User currUser)
        {
            return foob.GetChats(lobby, currUser);
        }

        /**
        Method: Log
        Imports: string logString (Log message)
        Exports: None
        Notes: Logs a message with a log number and timestamp.
        Algorithm: Increments the log number and writes the log message to the console.
        */
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        /**
        Method: UploadFile
        Imports: byte[] fileData (File data), string fileName (File name), string lobbyName (Lobby name)
        Exports: None
        Notes: Uploads a file to the specified lobby.
        Algorithm: Fetches the lobby and calls the data layer to save the file.
        */
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UploadFile(byte[] fileData, string fileName, string lobbyName)
        {
            Lobby lobby = foob.GetLobby(lobbyName);
            foob.saveFile(fileName, fileData, lobby);
        }

        /**
        Method: DownloadFile
        Imports: string fileName (File name)
        Exports: byte[] (File data)
        Notes: Downloads a file by name.
        Algorithm: Logs the download request and calls the data layer to fetch the file.
        */
        public byte[] DownloadFile(string fileName)
        {
            Log($"Received file download request: {fileName}");
            return foob.downloadFile(fileName);
        }

        /**
        Method: UpdateMessage
        Imports: List<string> messageText (List of message text), string lobbyName (Lobby name), string userName1 (First username), string userName2 (Second username)
        Exports: None
        Notes: Updates a message between two users in the specified lobby.
        Algorithm: Fetches the users, lobby, and message, then updates the message in the data layer.
        */
        public void UpdateMessage(List<string> messageText, string lobbyName, string userName1, string userName2)
        {
            User user1 = foob.GetUser(userName1);
            User user2 = foob.GetUser(userName2);
            Lobby lobby = foob.GetLobby(lobbyName);
            Message msg = foob.GetMessage(user1, user2, lobby);
            msg.MessageList = messageText;
            foob.UpdateMessage(msg, lobby);
        }

        /**
        Method: joinLobby
        Imports: string lobbyName (Lobby name), string userName (Username)
        Exports: None
        Notes: Joins a user to a lobby.
        Algorithm: Fetches the user and lobby, then calls the data layer to add the user to the lobby.
        */
        public void joinLobby(string lobbyName, string userName)
        {
            User user = foob.GetUser(userName);
            Lobby lobby = foob.GetLobby(lobbyName);
            foob.joinLobby(lobby, user);
        }

        /**
        Method: GetMessage
        Imports: string userName1 (First username), string userName2 (Second username), string lobbyName (Lobby name)
        Exports: List<string> (List of message texts)
        Notes: Retrieves a message between two users in the specified lobby.
        Algorithm: Fetches the users, lobby, and message from the data layer.
        */
        public List<string> GetMessage(string userName1, string userName2, string lobbyName)
        {
            User user1 = foob.GetUser(userName1);
            User user2 = foob.GetUser(userName2);
            Lobby lobby = foob.GetLobby(lobbyName);
            Message message = foob.GetMessage(user1, user2, lobby);
            return message.MessageList;
        }

        /**
        Method: GetLobbyFiles
        Imports: string lobbyName (Lobby name)
        Exports: List<string> (List of uploaded files)
        Notes: Retrieves the list of uploaded files for the specified lobby.
        Algorithm: Fetches the lobby and calls the data layer to get the files.
        */
        public List<string> GetLobbyFiles(string lobbyName)
        {
            Lobby lobby = foob.GetLobby(lobbyName);
            return foob.GetLobbyFiles(lobby);  // Delegate the call to the Data Layer
        }

        /**
        Method: GetUserCount
        Imports: None
        Exports: int (Total number of users)
        Notes: Retrieves the total number of users in the system.
        Algorithm: Calls the data layer to get the user count.
        */
        public int GetUserCount()
        {
            return foob.GetUserCount();
        }
    }
}
