using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ServiceContract]
    public interface IBusinessServerInterface
    {
        [OperationContract]
        List<string> GetUsers(string lobbyName);

        [OperationContract]
        List<Lobby> GetAllLobbies();

        [OperationContract]
        User GetUser(string name);

        [OperationContract]
        void AddMessage(Lobby lobby, User user1, User user2);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        void AddUser(string userName);

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
        void AddLobby(string roomName, string desc, string mode, List<string> tags);

        [OperationContract]
        bool UniqueUser(string userName);

        [OperationContract]
        void UploadFile(byte[] fileData, string fileName, string lobbyName);

        [OperationContract]
        byte[] DownloadFile(string fileName);

        [OperationContract]
        void RemoveUserFromLobby(string lobbyName,string userName);

        [OperationContract]
        void RemoveUser(string user);

        [OperationContract]
        List<Message> getChats(Lobby lobby, User currUser);

        [OperationContract]
        void UpdateMessage(List<string> messageText, string lobby, string userName1, string userName2);

        [OperationContract]
        void joinLobby(string lobbyName, string userName);

        [OperationContract]
        List<string> GetMessage(string userName1, string userName2, string lobbyName);

        [OperationContract]
        List<string> GetLobbyFiles(string lobbyName);

        [OperationContract]
        int GetUserCount();


        [OperationContract]
        Stream DownloadFile2(string fileName);

        [OperationContract]
        void UploadFile2(Stream fileStream, string fileName,string lobbyName);
    }
}
