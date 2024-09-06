using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ServiceContract(CallbackContract =typeof(ProcessServiceCallBack))]
    public interface IBusinessServerInterface
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
        bool UniqueUser(string userName);
        
        [OperationContract]
        void UploadFile(byte[] fileData, string fileName);

        [OperationContract]
        void DownloadFile();

        [OperationContract]
        void RemoveUser(Lobby lobby,User user);

        [OperationContract]
        List<Message> getChats(Lobby lobby, User currUser);

        [OperationContract]
        void UpdateMessage(Message msg);

        [OperationContract]
        void joinLobby(Lobby lobby, User user);
    }

    public interface ProcessServiceCallBack
    {
        [OperationContract(IsOneWay = true)]
        void UpdateUserCount(int userCount);
    }
}
