/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: Lobby
Purpose: Represents a game lobby that contains users, messages, and uploaded files. It manages users, messages, and files associated with the lobby.
Notes: This class is used in both the data and business layers to represent a lobby instance. It includes functionality for adding/removing users, handling messages, and storing file names.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLobbyLib
{
    public class Lobby
    {
        private static int nextID = 1;
        private string name;
        private List<User> users;
        private List<Message> messages;
        private string description;
        private string mode;
        private List<string> tags;
        public int Id;  // This field must remain public to be accessed in the business layer
        public List<string> UploadedFiles { get; set; }  // List to store uploaded file names

        /**
        Constructor: Lobby()
        Purpose: Default constructor required for serialization purposes.
        */
        public Lobby() { }  // Required for serialization; should not be removed.

        /**
        Constructor: Lobby(string name, string description, string mode, List<string> tags)
        Purpose: Initializes a new lobby instance with the specified name, description, mode, and tags.
        */
        public Lobby(string name, string description, string mode, List<string> tags)
        {
            Users = new List<User>();
            Messages = new List<Message>();
            UploadedFiles = new List<string>();  // Initialize the file list
            Id = nextID++;  // Assign a unique ID to the lobby
            Name = name;
            messages = new List<Message>();
            Message defaultMessage = new Message(new User[] { null, null });
            defaultMessage.MessageList.Add("Lobby chat is now open!");
            messages.Add(defaultMessage);
            Description = description;
            Mode = mode;
            Tags = tags;
        }

        /**
        Method: Equals(object obj)
        Purpose: Determines if two lobby instances are equal based on their ID and name.
        */
        public override bool Equals(object obj)
        {
            Lobby other = obj as Lobby;
            if (other == null) return false;
            return Id == other.ID && Name.Equals(other.Name);
        }

        // Properties for ID, Name, Users, Messages, Description, Mode, and Tags
        public int ID => Id;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public List<User> Users
        {
            get => users;
            set => users = value;
        }

        public List<Message> Messages
        {
            get => messages;
            set => messages = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string Mode
        {
            get => mode;
            set => mode = value;
        }

        public List<string> Tags
        {
            get => tags;
            set => tags = value;
        }

        public string TagDis => Tags != null ? string.Join(",", Tags) : string.Empty;

        /**
        Method: addUser(User user)
        Purpose: Adds a user to the lobby.
        */
        public void addUser(User user)
        {
            users.Add(user);
        }

        /**
        Method: removeUser(User user)
        Purpose: Removes a user from the lobby.
        */
        public void removeUser(User user)
        {
            users.RemoveAll(u => u.Equals(user));
        }

        /**
        Method: getMessage(User user1, User user2)
        Purpose: Retrieves the message between two users in the lobby or returns the lobby chat if either user is null.
        */
        public Message getMessage(User user1, User user2)
        {
            if (user1 == null || user2 == null)
            {
                Console.WriteLine("Sending messages from lobby chat");
                return messages[0];  // Return the lobby chat message
            }

            foreach (Message message in messages)
            {
                if (message.UserList.Contains(user1) && message.UserList.Contains(user2))
                {
                    Console.WriteLine("Sending messages from " + user1.Name + " to " + user2.Name);
                    return message;
                }
            }

            // If no message exists between the users, create a new one
            Message newMessage = new Message(new User[] { user1, user2 });
            messages.Add(newMessage);
            Console.WriteLine("Sending new message from " + user1.Name + " to " + user2.Name);
            return newMessage;
        }

        /**
        Method: updateMessage(Message inMessage)
        Purpose: Updates an existing message in the lobby.
        */
        public void updateMessage(Message inMessage)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if (inMessage.UserList.Contains(messages[i].UserList[0]) && inMessage.UserList.Contains(messages[i].UserList[1]))
                {
                    Console.WriteLine("Updating message...");
                    messages[i] = inMessage;
                    break;
                }
            }
        }

        /**
        Method: AddFile(string fileName)
        Purpose: Adds a file name to the list of uploaded files if it's not already present.
        */
        public void AddFile(string fileName)
        {
            if (!UploadedFiles.Contains(fileName))
            {
                UploadedFiles.Add(fileName);
            }
        }
    }
}
