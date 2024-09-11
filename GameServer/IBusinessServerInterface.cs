using GameLobbyLib;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ServiceContract]
    public interface IBusinessServerInterface
    {
        [OperationContract]
        Task<List<string>> GetUsersAsync(string lobbyName);

        [OperationContract]
        Task<List<Lobby>> GetAllLobbiesAsync();

        [OperationContract]
        Task<User> GetUserAsync(string name);

        [OperationContract]
        Task AddMessageAsync(Lobby lobby, User user1, User user2);

        [OperationContract]
        Task<List<User>> GetAllUsersAsync();

        [OperationContract]
        Task AddUserAsync(string userName);

        [OperationContract]
        Task<List<string>> GetUniqueModesAsync(List<Lobby> currLobbyList);

        [OperationContract]
        Task<List<string>> GetUniqueTagsAsync(List<Lobby> curLobbyList);

        [OperationContract]
        Task<List<Lobby>> GetFilteredLobbiesListAsync(string mode = null, string tag = null);

        [OperationContract]
        Task<List<string>> GetAllModeTypesAsync();

        [OperationContract]
        Task<List<string>> GetAllTagTypesAsync();

        [OperationContract]
        Task AddLobbyAsync(string roomName, string desc, string mode, List<string> tags);

        [OperationContract]
        Task<bool> UniqueUserAsync(string userName);

        [OperationContract]
        Task UploadFileAsync(byte[] fileData, string fileName, string lobbyName);

        [OperationContract]
        Task<byte[]> DownloadFileAsync(string fileName);

        [OperationContract]
        Task RemoveUserFromLobbyAsync(string lobbyName, string userName);

        [OperationContract]
        Task RemoveUserAsync(string userName);

        [OperationContract]
        Task<List<Message>> GetChatsAsync(Lobby lobby, User currUser);

        [OperationContract]
        Task UpdateMessageAsync(List<string> messageText, string lobby, string userName1, string userName2);

        [OperationContract]
        Task JoinLobbyAsync(string lobbyName, string userName);  // Add this method

        [OperationContract]
        Task<List<string>> GetMessageAsync(string userName1, string userName2, string lobbyName);

        [OperationContract]
        Task<List<string>> GetLobbyFilesAsync(string lobbyName);

        [OperationContract]
        Task<int> GetUserCountAsync();
    }
}
