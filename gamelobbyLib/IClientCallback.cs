using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract]
        void NotifyNewMessage(string message);
        
        [OperationContract(IsOneWay = true)]
        void UpdateUserCount(int userCount);


    }
}
