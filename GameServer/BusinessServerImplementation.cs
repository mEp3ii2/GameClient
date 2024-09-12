using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.IO;

namespace BusinessLayer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    internal class BusinessServerImplementation : IBusinessServerInterface
    {
        private IDataServerInterface foob;
        private static uint logNumber = 0;

        public BusinessServerImplementation()
        {
            ChannelFactory<IDataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8200/DataService";
            foobFactory = new ChannelFactory<IDataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public async Task<List<Lobby>> GetAllLobbiesAsync()
        {
            Log("Retrieving Lobbies from data layer");
            return await foob.GetAllLobbiesAsync();
        }

        public async Task<List<string>> GetUsersAsync(string lobbyName)
        {
            Lobby lobby = await foob.GetLobbyAsync(lobbyName);
            List<string> userNames = new List<string>();
            foreach (User user in await foob.GetUsersAsync(lobby))
            {
                userNames.Add(user.Name);
            }
            return userNames;
        }

        public async Task<User> GetUserAsync(string name)
        {
            return await foob.GetUserAsync(name);
        }

        public async Task RemoveUserFromLobbyAsync(string lobbyName, string userName)
        {
            if (string.IsNullOrEmpty(lobbyName) || string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Lobby name and user name must not be null or empty.");
            }

            Lobby lobby = await foob.GetLobbyAsync(lobbyName);
            User user = await foob.GetUserAsync(userName);

            if (lobby == null)
            {
                throw new Exception($"Lobby '{lobbyName}' not found.");
            }

            if (user == null)
            {
                throw new Exception($"User '{userName}' not found.");
            }

            await foob.RemoveUserFromLobbyAsync(lobby, user);
        }

        public async Task AddMessageAsync(Lobby lobby, User user, string messageContent)
        {
            Log($"Adding message from {user.Name} in lobby {lobby.Name}");
            Lobby thisLobby = await foob.GetLobbyAsync(lobby.Name);

            if (thisLobby != null)
            {
                Message lobbyMessage = thisLobby.getMessage(user, null); // Get the current lobby message
                lobbyMessage.AddMessage(user.Name, messageContent); // Add message with username in content
                await Task.Run(() => thisLobby.Messages.Add(lobbyMessage)); // Add the message to the list
                Log($"Message added successfully from {user.Name}.");
            }
            else
            {
                throw new ArgumentException($"Lobby '{lobby.Name}' not found.");
            }
        }

        public async Task RemoveUserAsync(string userName)
        {
            User user = await foob.GetUserAsync(userName);
            await foob.RemoveUserAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await foob.GetAllUsersAsync();
        }

        public async Task AddUserAsync(string userName)
        {
            Log($"Added new user: {userName}");
            await foob.AddUserAsync(new User(userName));
        }

        public async Task<List<string>> GetUniqueModesAsync(List<Lobby> curLobbyList)
        {
            return await foob.GetUniqueModesAsync(curLobbyList);
        }

        public async Task<List<string>> GetUniqueTagsAsync(List<Lobby> curLobbyList)
        {
            return await foob.GetUniqueTagsAsync(curLobbyList);
        }

        public async Task<List<Lobby>> GetFilteredLobbiesListAsync(string mode = null, string tag = null)
        {
            return await foob.GetFilteredLobbiesListAsync(mode, tag);
        }

        public async Task<List<string>> GetAllModeTypesAsync()
        {
            return await foob.GetAllModeTypesAsync();
        }

        public async Task<List<string>> GetAllTagTypesAsync()
        {
            return await foob.GetAllTagTypesAsync();
        }

        public async Task AddLobbyAsync(string roomName, string desc, string mode, List<string> tags)
        {
            Log($"New lobby created: {roomName}");
            Lobby lobby = new Lobby(roomName, desc, mode, tags);
            await foob.AddLobbyAsync(lobby);
        }

        public async Task<bool> UniqueUserAsync(string userName)
        {
            Log($"Attempted login with username {userName}");
            List<User> users = await foob.GetAllUsersAsync();

            foreach (User user in users)
            {
                if (user.Name.Equals(userName))
                {
                    Log($"Login failed {userName} already used");
                    return false;
                }
            }
            Log($"Login for {userName} successful");
            return true;
        }

        public async Task<List<Message>> GetChatsAsync(Lobby lobby, User currUser)
        {
            return await foob.GetChatsAsync(lobby, currUser);
        }

        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        public async Task UploadFileAsync(byte[] fileData, string fileName, string lobbyName)
        {
            try
            {
                if (fileData == null || fileData.Length == 0)
                {
                    throw new ArgumentException("File data cannot be null or empty");
                }

                Lobby lobby = await foob.GetLobbyAsync(lobbyName);
                if (lobby == null)
                {
                    throw new Exception($"Lobby '{lobbyName}' not found.");
                }

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, fileData); // Synchronously write the file to disk

                lobby.AddFile(fileName); // Add the file to the lobby's list

                Console.WriteLine($"File {fileName} successfully uploaded to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file upload: {ex.Message}");
                throw new FaultException($"Error during file upload: {ex.Message}");
            }
        }

        public async Task<byte[]> DownloadFileAsync(string fileName)
        {
            Log($"Received file download request: {fileName}");
            return await foob.DownloadFileAsync(fileName);
        }

        public async Task UpdateMessageAsync(List<string> messageText, string lobbyName, string userName1, string userName2)
        {
            User user1 = await foob.GetUserAsync(userName1);
            User user2 = await foob.GetUserAsync(userName2);
            Lobby lobby = await foob.GetLobbyAsync(lobbyName);
            Message msg = await foob.GetMessageAsync(user1, user2, lobby);
            msg.MessageList = messageText;
            await foob.UpdateMessageAsync(msg, lobby);
        }

        public async Task JoinLobbyAsync(string lobbyName, string userName)
        {
            User user = await foob.GetUserAsync(userName);
            Lobby lobby = await foob.GetLobbyAsync(lobbyName);
            await foob.JoinLobbyAsync(lobby, user);
        }

        public async Task<List<string>> GetMessageAsync(string userName1, string userName2, string lobbyName)
        {
            User user1 = await foob.GetUserAsync(userName1);
            User user2 = await foob.GetUserAsync(userName2);
            Lobby lobby = await foob.GetLobbyAsync(lobbyName);
            Message message = await foob.GetMessageAsync(user1, user2, lobby);
            return message.MessageList;
        }

        public async Task<List<string>> GetLobbyFilesAsync(string lobbyName)
        {
            Lobby lobby = await foob.GetLobbyAsync(lobbyName);
            return await foob.GetLobbyFilesAsync(lobby);
        }

        public async Task<int> GetUserCountAsync()
        {
            return await foob.GetUserCountAsync();
        }
    }
}
