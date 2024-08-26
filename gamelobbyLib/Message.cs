using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    public class Message
    {
        private int lobbyID;
        private List<string> messageList;
        private User[] userList;
        
        //userList can be null in the case of the lobby group chat
        public Message(int lobbyID, User[] userList = null)
        {
            this.lobbyID = lobbyID; 
            this.messageList = new List<string>();
            this.userList = userList;
        }

        public int LobbyID
        {
            get { return lobbyID; }
        }

        public List<string> MessageList
        {
            get { return messageList; }
            set { messageList = value; }
        }

        public User[] UserList
        {
            get { return userList; }            
        }

    }
}
