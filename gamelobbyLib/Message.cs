using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GameLobbyLib
{
    
    public class Message
    {
        public int id;
        private static int nextID = 1;
        private List<string> messageList;
        private string[] userList;

        public Message() { }//I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

        //userList can be null in the case of the lobby group chat
        public Message(string[] userList = null)
        {
            id = nextID++; 
            this.messageList = new List<string>();
            this.userList = userList;
        }

        public int ID
        {
            get { return id; }
        }
    
        public List<string> MessageList
        {
            get { return messageList; }
            set { messageList = value; }
        }

        [DataMember]
        public string[] UserList
        {
            get { return userList; }
            set { userList = value; }
        }

    }
}
