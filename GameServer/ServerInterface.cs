﻿using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [ServiceContract]
    public interface ServerInterface
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
    }
}
