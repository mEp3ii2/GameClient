using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.IO;
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
        User GetUser(string name);

        [OperationContract]
        void AddMessage(Lobby lobby, User user1, User user2);

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
        List<Message> GetChats(Lobby lobby, User currUser);

        [OperationContract]
        void joinLobby(Lobby lobby, User user);

        [OperationContract]
        void saveFile(string fileName, byte[] fileData, Lobby lobby);

        [OperationContract]
        byte[] downloadFile(string fileName);

        [OperationContract]
        void RemoveUserFromLobby(Lobby lobby, User user);

        [OperationContract]
        void RemoveUser(User user);

        [OperationContract]
        void UpdateMessage(Message msg, Lobby lobby);

        [OperationContract]
        Message GetMessage(User user1, User user2, Lobby lobby);

        [OperationContract]
        Lobby GetLobby(string lobby);

        [OperationContract]
        List<string> GetLobbyFiles(Lobby lobby);

        [OperationContract]
        int GetUserCount();

        [OperationContract]
        Stream DownloadFile2(string fileName);
        [OperationContract]
        void saveFile2(string fileName, Stream fileData, Lobby lobby);
    }
}
