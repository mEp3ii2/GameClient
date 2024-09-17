/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: DataServerImplementation
Purpose: Implements the data server interface for managing the data layer of the online gaming lobby, including handling lobbies, users, messages, and file sharing.
Notes: This class communicates with the database to handle all CRUD operations for users, lobbies, messages, and files in a multi-threaded environment.
*/

using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.IO;

namespace DataLayer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DataServerImplementation : IDataServerInterface //change back to internal later
    {
        private static Database database;
        private static uint logNumber = 0;

        /**
        Method: DataServerImplementation (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the database instance using the singleton pattern.
        Algorithm: Calls the getInstance method of the Database class to retrieve the database singleton.
        */
        public DataServerImplementation()
        {
            database = Database.getInstance();
        }

        /**
        Method: GetAllLobbies
        Imports: None
        Exports: List<Lobby> (All lobbies in the system)
        Notes: Retrieves all the lobbies currently stored in the database.
        Algorithm: Returns the result of the getAllLobbies method in the Database class.
        */
        public List<Lobby> GetAllLobbies()
        {
            Log($"Grabbing all Lobbies");
            return database.getAllLobbies();
        }

        /**
        Method: GetUsers
        Imports: Lobby lobby (The lobby to retrieve users from)
        Exports: List<User> (Users in the specified lobby)
        Notes: Retrieves all users currently in the specified lobby.
        Algorithm: Calls the getLobbyUsers method in the Database class.
        */
        public List<User> GetUsers(Lobby lobby)
        {
            Log($"Getting users from lobby {lobby.Name}");
            return database.getLobbyUsers(lobby);
        }

        /**
        Method: GetUser
        Imports: string name (The name of the user to retrieve)
        Exports: User (The user object with the specified name, or null if not found)
        Notes: Retrieves a user by their name from the database.
        Algorithm: Calls the GetUser method in the Database class.
        */
        public User GetUser(string name)
        {
            return database.GetUser(name);
        }

        /**
        Method: GetAllUsers
        Imports: None
        Exports: List<User> (All users in the system)
        Notes: Retrieves all the users currently stored in the database.
        Algorithm: Calls the getAllUsers method in the Database class.
        */
        public List<User> GetAllUsers()
        {
            return database.getAllUsers();
        }

        /**
        Method: AddUser
        Imports: User user (The user to be added to the system)
        Exports: None
        Notes: Adds a new user to the system.
        Algorithm: Calls the addUser method in the Database class.
        */
        public void AddUser(User user)
        {
            database.addUser(user);
        }

        /**
        Method: GetUniqueModes
        Imports: List<Lobby> curLobbyList (Optional lobby list to filter by)
        Exports: List<string> (Unique modes in the current lobbies or the whole system)
        Notes: Retrieves unique game modes from the provided lobby list or all lobbies.
        Algorithm: Calls the GetUniqueModes method in the Database class.
        */
        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            return database.GetUniqueModes(curLobbyList);
        }

        /**
        Method: GetUniqueTags
        Imports: List<Lobby> curLobbyList (Optional lobby list to filter by)
        Exports: List<string> (Unique tags in the current lobbies or the whole system)
        Notes: Retrieves unique tags from the provided lobby list or all lobbies.
        Algorithm: Calls the GetUniqueTags method in the Database class.
        */
        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return database.GetUniqueTags(curLobbyList);
        }

        /**
        Method: GetfilterdLobbiesList
        Imports: string mode, string tag (Optional filtering parameters for mode and tag)
        Exports: List<Lobby> (Lobbies filtered by mode and/or tag)
        Notes: Retrieves the list of lobbies filtered by mode and tag.
        Algorithm: Calls the getfilterdLobbiesList method in the Database class.
        */
        public List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null)
        {
            return database.getfilterdLobbiesList(mode, tag);
        }

        /**
        Method: GetAllModeTypes
        Imports: None
        Exports: List<string> (All available game modes)
        Notes: Retrieves all available game modes.
        Algorithm: Calls the getAllModeTypes method in the Database class.
        */
        public List<string> GetAllModeTypes()
        {
            return database.getAllModeTypes();
        }

        /**
        Method: GetAllTagTypes
        Imports: None
        Exports: List<string> (All available tags)
        Notes: Retrieves all available tags.
        Algorithm: Calls the getAllTagTypes method in the Database class.
        */
        public List<string> GetAllTagTypes()
        {
            return database.getAllTagTypes();
        }

        /**
        Method: AddLobby
        Imports: Lobby lobby (The lobby to be added)
        Exports: None
        Notes: Adds a new lobby to the system.
        Algorithm: Calls the addNewLobby method in the Database class.
        */
        public void AddLobby(Lobby lobby)
        {
            database.addNewLobby(lobby);
        }

        /**
        Method: RemoveUserFromLobby
        Imports: Lobby lobby, User user (The lobby and user to be removed from the lobby)
        Exports: None
        Notes: Removes the specified user from the specified lobby.
        Algorithm: Calls the RemoveUser method in the Database class.
        */
        public void RemoveUserFromLobby(Lobby lobby, User user)
        {
            database.RemoveUser(lobby, user);
        }

        /**
        Method: RemoveUser
        Imports: User user (The user to be removed)
        Exports: None
        Notes: Removes the specified user from the system and all lobbies they are in.
        Algorithm: Calls the RemoveUser method in the Database class.
        */
        public void RemoveUser(User user)
        {
            database.RemoveUser(user);
        }

        /**
        Method: UpdateMessage
        Imports: Message msg, Lobby lobby (The message to update and the lobby it belongs to)
        Exports: None
        Notes: Updates the specified message in the provided lobby.
        Algorithm: Calls the updateMessage method on the appropriate lobby in the Database class.
        */
        public void UpdateMessage(Message msg, Lobby lobby)
        {
            database.getLobby(lobby).updateMessage(msg);
        }

        /**
        Method: joinLobby
        Imports: Lobby lobby, User user (The lobby to join and the user joining it)
        Exports: None
        Notes: Adds the specified user to the specified lobby.
        Algorithm: Calls the joinLobby method in the Database class.
        */
        public void joinLobby(Lobby lobby, User user)
        {
            database.joinLobby(lobby, user);
        }

        /**
        Method: GetChats
        Imports: Lobby lobby, User currUser (The lobby and the current user to retrieve chats for)
        Exports: List<Message> (The chat messages in the specified lobby for the current user)
        Notes: Retrieves the chat history for the specified user in the specified lobby.
        Algorithm: Filters the lobby's messages by checking if they involve the current user or are general messages.
        */
        public List<Message> GetChats(Lobby lobby, User currUser)
        {
            return database.getLobby(lobby).Messages
                .Where(m => m.UserList.Contains(currUser) || m.UserList.Contains(null)).ToList();
        }

        /**
        Method: AddMessage
        Imports: Lobby lobby, User user1, User user2 (The lobby and the two users involved in the message)
        Exports: None
        Notes: Adds a new message between the two users in the specified lobby.
        Algorithm: Creates a new message between the users and adds it to the lobby's message list.
        */
        public void AddMessage(Lobby lobby, User user1, User user2)
        {
            Log($"Adding message between {user1.Name} and {user2.Name} in lobby {lobby.Name}");
            Lobby thisLobby = database.getLobby(lobby);

            if (thisLobby != null)
            {
                Message newMessage = new Message(new User[] { user1, user2 });
                thisLobby.Messages.Add(newMessage);
                Log($"Message added successfully between {user1.Name} and {user2.Name}.");
            }
            else
            {
                throw new ArgumentException($"Lobby '{lobby.Name}' not found.");
            }
        }

        /**
        Method: GetMessage
        Imports: User user1, User user2, Lobby lobby (The two users and the lobby to retrieve the message for)
        Exports: Message (The message between the two users in the specified lobby)
        Notes: Retrieves the message between two users in the provided lobby.
        Algorithm: Calls the getMessage method in the appropriate lobby.
        */
        public Message GetMessage(User user1, User user2, Lobby lobby)
        {
            return database.getLobby(lobby).getMessage(user1, user2);
        }

        /**
        Method: GetLobby
        Imports: string lobbyName (The name of the lobby to retrieve)
        Exports: Lobby (The lobby object with the specified name, or null if not found)
        Notes: Retrieves a lobby by its name from the database.
        Algorithm: Calls the getLobby method in the Database class.
        */
        public Lobby GetLobby(string lobbyName)
        {
            return database.getLobby(lobbyName);
        }

        /**
        Method: saveFile
        Imports: string fileName, byte[] fileData, Lobby lobby (The file data, its name, and the lobby it belongs to)
        Exports: None
        Notes: Saves the uploaded file to the server and records the file in the specified lobby.
        Algorithm: Writes the file to disk and updates the lobby's file list.
        */
        public void saveFile(string fileName, byte[] fileData, Lobby lobby)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Ensure the directory exists
            File.WriteAllBytes(filePath, fileData);

            Log($"File {fileName} saved to {filePath}");

            // Retrieve the current lobby using the passed lobby name
            Lobby currentLobby = database.getLobby(lobby);
            currentLobby?.AddFile(fileName);  // Add the file to the lobby's list of uploaded files
        }

        /**
        Method: GetLobbyFiles
        Imports: Lobby lobby (The lobby to retrieve files from)
        Exports: List<string> (The list of files uploaded in the specified lobby)
        Notes: Retrieves the list of files uploaded to the specified lobby.
        Algorithm: Calls the GetLobbyFiles method in the appropriate lobby.
        */
        public List<string> GetLobbyFiles(Lobby lobby)
        {
            Lobby searchLobby = database.getLobby(lobby);
            return searchLobby?.UploadedFiles ?? new List<string>();
        }

        /**
        Method: downloadFile
        Imports: string fileName (The name of the file to download)
        Exports: byte[] (The file data in byte format)
        Notes: Downloads the file with the specified name from the server.
        Algorithm: Retrieves the file from disk and returns its data as a byte array.
        */
        public byte[] downloadFile(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            if (File.Exists(filePath))
            {
                Log($"File {fileName} downloaded from {filePath}");
                return File.ReadAllBytes(filePath);
            }
            else
            {
                throw new FileNotFoundException($"File {fileName} not found.");
            }
        }

        /**
        Method: Log
        Imports: string logString (The log message to be recorded)
        Exports: None
        Notes: Logs the provided message with a unique log number and timestamp.
        Algorithm: Increments the log number, appends the message, and prints it to the console.
        */
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        /**
        Method: GetUserCount
        Imports: None
        Exports: int (The total number of users in the system)
        Notes: Retrieves the total number of users currently registered in the system.
        Algorithm: Calls the GetUserCount method in the Database class.
        */
        public int GetUserCount()
        {
            return database.GetUserCount();
        }
    }
}
