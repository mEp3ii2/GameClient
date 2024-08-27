using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [ServiceContract]
    public interface IDataServerInterface
    {
        [OperationContract]
        List<User> GetUsers(Lobby lobby);

        [OperationContract]
        List<Lobby> GetAllLobbies();

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        void AddUser(User user);

        [OperationContract]
        List<string> GetUniqueModes(List<Lobby> currLobbyList);

        [OperationContract]
        List<string> GetUniqueTags(List<Lobby> curLobbyList);

        [OperationContract]
        List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null);

        [OperationContract]
        List<string> GetAllModeTypes();

        [OperationContract]
        List<string> GetAllTagTypes();

        [OperationContract]
        void AddLobby(Lobby lobby);
        
        [OperationContract]
        List<Message> GetChats(int lobbyId, User currUser);

       
        
        [OperationContract]
        void saveFile(string fileName, byte[] fileData);
        
        [OperationContract]
        void RemoveUser(Lobby lobby, User user);
    }
}
