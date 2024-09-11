using GameLobbyLib;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

[ServiceContract]
public interface IDataServerInterface
{
    [OperationContract]
    Task<List<Lobby>> GetAllLobbiesAsync();

    [OperationContract]
    Task<List<User>> GetUsersAsync(Lobby lobby);
    
    [OperationContract]
    Task<User> GetUserAsync(string name);

    [OperationContract]
    Task<List<User>> GetAllUsersAsync();

    [OperationContract]
    Task AddMessageAsync(Lobby lobby, User user, string messageContent);

    [OperationContract]
    Task<List<string>> GetUniqueModesAsync(List<Lobby> curLobbyList);

    [OperationContract]
    Task<List<string>> GetUniqueTagsAsync(List<Lobby> curLobbyList);

    [OperationContract]
    Task<List<Lobby>> GetFilteredLobbiesListAsync(string mode = null, string tag = null);

    [OperationContract]
    Task<List<string>> GetAllModeTypesAsync();

    [OperationContract]
    Task<List<string>> GetAllTagTypesAsync();

    [OperationContract]
    Task AddLobbyAsync(Lobby lobby);

    [OperationContract]
    Task RemoveUserFromLobbyAsync(Lobby lobby, User user);

    [OperationContract]
    Task RemoveUserAsync(User user);

    [OperationContract]
    Task UpdateMessageAsync(Message msg, Lobby lobby);

    [OperationContract]
    Task JoinLobbyAsync(Lobby lobby, User user);

    [OperationContract]
    Task<List<Message>> GetChatsAsync(Lobby lobby, User currUser);

    [OperationContract]
    Task<Message> GetMessageAsync(User user1, User user2, Lobby lobby);

    [OperationContract]
    Task<Lobby> GetLobbyAsync(string lobbyName);

    [OperationContract]
    Task SaveFileAsync(string fileName, byte[] fileData, Lobby lobby);

    [OperationContract]
    Task<List<string>> GetLobbyFilesAsync(Lobby lobby);

    [OperationContract]
    Task<byte[]> DownloadFileAsync(string fileName);

    [OperationContract]
    Task<int> GetUserCountAsync();

    [OperationContract]
    Task AddUserAsync(User user);
}
