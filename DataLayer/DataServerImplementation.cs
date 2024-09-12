using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Lobby>> GetAllLobbiesAsync()
        {
            Log("Grabbing all Lobbies");
            return await Task.FromResult(database.getAllLobbies());
        }

        public async Task<List<User>> GetUsersAsync(Lobby lobby)
        {
            Log($"Getting users from lobby {lobby.Name}");
            return await Task.FromResult(database.getLobbyUsers(lobby));
        }

        public async Task<User> GetUserAsync(string name)
        {
            return await Task.FromResult(database.GetUser(name));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(database.getAllUsers());
        }

        public async Task AddUserAsync(User user)
        {
            await Task.Run(() => database.addUser(user));
        }

        public async Task<List<string>> GetUniqueModesAsync(List<Lobby> curLobbyList)
        {
            return await Task.FromResult(database.GetUniqueModes(curLobbyList));
        }

        public async Task<List<string>> GetUniqueTagsAsync(List<Lobby> curLobbyList)
        {
            return await Task.FromResult(database.GetUniqueTags(curLobbyList));
        }

        public async Task<List<Lobby>> GetFilteredLobbiesListAsync(string mode = null, string tag = null)
        {
            return await Task.FromResult(database.getfilterdLobbiesList(mode, tag));
        }

        public async Task<List<string>> GetAllModeTypesAsync()
        {
            return await Task.FromResult(database.getAllModeTypes());
        }

        public async Task<List<string>> GetAllTagTypesAsync()
        {
            return await Task.FromResult(database.getAllTagTypes());
        }

        public async Task AddLobbyAsync(Lobby lobby)
        {
            await Task.Run(() => database.addNewLobby(lobby));
        }

        public async Task RemoveUserFromLobbyAsync(Lobby lobby, User user)
        {
            await Task.Run(() => database.RemoveUser(lobby, user));
        }

        public async Task RemoveUserAsync(User user)
        {
            await Task.Run(() => database.RemoveUser(user));
        }

        public async Task UpdateMessageAsync(Message msg, Lobby lobby)
        {
            await Task.Run(() => database.getLobby(lobby).updateMessage(msg));
        }

        public async Task JoinLobbyAsync(Lobby lobby, User user)
        {
            await Task.Run(() => database.joinLobby(lobby, user));
        }

        public async Task<List<Message>> GetChatsAsync(Lobby lobby, User currUser)
        {
            return await Task.FromResult(database.getLobby(lobby).Messages
                .Where(m => m.UserList.Contains(currUser) || m.UserList.Contains(null)).ToList());
        }

        public async Task AddMessageAsync(Lobby lobby, User user, string messageContent)
        {
            Lobby thisLobby = database.getLobby(lobby);

            if (thisLobby != null)
            {
                Message newMessage = new Message(new User[] { user, null });
                newMessage.AddMessage(user.Name, messageContent); // Add message with username
                await Task.Run(() => thisLobby.Messages.Add(newMessage)); // Save the message
            }
            else
            {
                throw new ArgumentException($"Lobby '{lobby.Name}' not found.");
            }
        }

        public async Task<Message> GetMessageAsync(User user1, User user2, Lobby lobby)
        {
            return await Task.FromResult(database.getLobby(lobby).getMessage(user1, user2));
        }

        public async Task<Lobby> GetLobbyAsync(string lobbyName)
        {
            return await Task.FromResult(database.getLobby(lobbyName));
        }

        // Save the file and record the file name in the lobby
        public async Task SaveFileAsync(string fileName, byte[] fileData, Lobby lobby)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Ensure the directory exists
            File.WriteAllBytes(filePath, fileData); // Synchronous version used here

            Log($"File {fileName} saved to {filePath}");

            // Retrieve the current lobby using the passed lobby name
            Lobby currentLobby = database.getLobby(lobby);
            await Task.Run(() => currentLobby?.AddFile(fileName));  // Add the file to the lobby's list of uploaded files
        }

<<<<<<< HEAD
        public async Task<byte[]> DownloadFileAsync(string fileName)
=======
        // Retrieve the list of previously uploaded files for a given lobby
        public List<string> GetLobbyFiles(Lobby lobby)
        {
            Lobby searchLobby = database.getLobby(lobby);
            return searchLobby?.UploadedFiles ?? new List<string>();  // Return file names only
        }

        // Add file download method
        public byte[] downloadFile(string fileName)
>>>>>>> DevRyanA
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            if (File.Exists(filePath))
            {
                Log($"File {fileName} downloaded from {filePath}");
<<<<<<< HEAD
                return await Task.FromResult(File.ReadAllBytes(filePath)); // Synchronous version used here
=======
                return File.ReadAllBytes(filePath);  // Send file data only when requested
>>>>>>> DevRyanA
            }
            else
            {
                throw new FileNotFoundException($"File {fileName} not found.");
            }
        }


        // Retrieve the list of previously uploaded files for a given lobby
        public async Task<List<string>> GetLobbyFilesAsync(Lobby lobby)
        {
            Lobby searchLobby = database.getLobby(lobby);
            return await Task.FromResult(searchLobby?.UploadedFiles ?? new List<string>());
        }

        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        public async Task<int> GetUserCountAsync()
        {
            return await Task.FromResult(database.GetUserCount());
        }
    }
}
