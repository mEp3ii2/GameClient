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
    internal interface ServerInterface
    {   
        [OperationContract]
        List<User> getUsers(Lobby lobby);

        [OperationContract]
        List<Lobby> getLobbys();
    }
}
