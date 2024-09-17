/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: Message
Purpose: Represents a message exchanged between two users or within a lobby. Contains the message content and users involved in the conversation.
Notes: The `Message` class is used for both direct user-to-user communication and lobby-wide group chats. The user list can be null for group chats in a lobby.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GameLobbyLib
{
    public class Message
    {
        public int id;
        private static int nextID = 1;
        private List<string> messageList;
        private User[] userList;

        /**
        Constructor: Message()
        Purpose: Default constructor required for serialization purposes.
        */
        public Message() { }  // Required for serialization purposes, do not remove.

        /**
        Constructor: Message(User[] userList = null)
        Purpose: Initializes a new message instance with an optional user list. If the user list is null, it represents a lobby group chat.
        */
        public Message(User[] userList = null)
        {
            id = nextID++;  // Assigns a unique ID to the message.
            this.messageList = new List<string>();
            this.userList = userList;
        }

        /**
        Method: Equals(object obj)
        Purpose: Compares two messages based on their user lists. Returns true if both messages have the same users.
        */
        public override bool Equals(object obj)
        {
            Message other = obj as Message;
            if (other != null && other.userList.Contains(userList[0]) && other.userList.Contains(userList[1]))
            {
                return true;
            }
            return false;
        }

        // Property to get the message ID
        public int ID
        {
            get { return id; }
        }

        // Property to get or set the list of message strings
        public List<string> MessageList
        {
            get { return messageList; }
            set { messageList = value; }
        }

        /**
        Property: UserList
        Purpose: Gets or sets the array of users associated with this message. This property is serialized for communication purposes.
        */
        [DataMember]
        public User[] UserList
        {
            get { return userList; }
            set { userList = value; }
        }
    }
}
