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

        public DataServerImplementation()
        {
            database = Database.getInstance();
        }

        public List<Lobby> GetAllLobbies()
        {
            Log($"Grabbing all Lobbies");
            return database.getAllLobbies();
        }

        public List<User> GetUsers(Lobby lobby)
        {
            Log($"Getting users from lobby {lobby.Name}");
            return database.getLobbyUsers(lobby);
        }

        public User GetUser(string name)
        {
            return database.GetUser(name);
        }

        public List<User> GetAllUsers()
        {
            return database.getAllUsers();
        }

        public void AddUser(User user)
        {
            database.addUser(user);
        }

        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            return database.GetUniqueModes(curLobbyList);
        }

        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return database.GetUniqueTags(curLobbyList);
        }

        public List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null)
        {
            return database.getfilterdLobbiesList(mode, tag);
        }

        public List<string> GetAllModeTypes()
        {
            return database.getAllModeTypes();
        }

        public List<string> GetAllTagTypes()
        {
            return database.getAllTagTypes();
        }

        public void AddLobby(Lobby lobby)
        {
            database.addNewLobby(lobby);
        }

        public void RemoveUserFromLobby(Lobby lobby, User user)
        {
            database.RemoveUser(lobby, user);
        }

        public void RemoveUser(User user)
        {
            database.RemoveUser(user);
        }

        public void UpdateMessage(Message msg, Lobby lobby)
        {
            database.getLobby(lobby).updateMessage(msg);
        }

        public void joinLobby(Lobby lobby, User user)
        {
            database.joinLobby(lobby, user);
        }

        public List<Message> GetChats(Lobby lobby, User currUser)
        {
            return database.getLobby(lobby).Messages
                .Where(m => m.UserList.Contains(currUser) || m.UserList.Contains(null)).ToList();
        }

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

        public Message GetMessage(User user1, User user2, Lobby lobby)
        {
            return database.getLobby(lobby).getMessage(user1, user2);
        }

        public Lobby GetLobby(string lobbyName)
        {
            return database.getLobby(lobbyName);
        }

        // Save the file and record the file name in the lobby
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
        

        public void saveFile2(string fileName, Stream fileData, Lobby lobby)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Ensure the directory exists
            using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fileData.CopyTo(outputFileStream);  // Copy the incoming stream to the file
            }
            Log($"File {fileName} saved to {filePath}");

            // Retrieve the current lobby using the passed lobby name
            Lobby currentLobby = database.getLobby(lobby);
            currentLobby?.AddFile(fileName);  // Add the file to the lobby's list of uploaded files
        }

        // Retrieve the list of previously uploaded files for a given lobby
        public List<string> GetLobbyFiles(Lobby lobby)
        {
            Lobby searchLobby = database.getLobby(lobby);
            return searchLobby?.UploadedFiles ?? new List<string>();
        }

        // Add file download method
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

        public Stream DownloadFile2(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            if (File.Exists(filePath))
            {
                Log($"File {fileName} downloaded from {filePath}");
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return fileStream;
            }
            else
            {
                throw new FileNotFoundException($"File {fileName} not found.");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        public int GetUserCount()
        {
            return database.GetUserCount();
        }

       
    }
}
