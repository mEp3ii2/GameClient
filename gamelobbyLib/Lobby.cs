using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

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
        public int Id; // if set to private id doesnt get passed to businesss layer and its set to 0 so idk


        public Lobby() { } //I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

        public Lobby(string name,  string description, string mode, List<string> tags)
        {
            Id = nextID++;
            Name = name;
            users = new List<User>();
            User lobbyUser = new User("lobby");
            users.Add(lobbyUser);
            messages = new List<Message>();
            Message defaultMessage = new Message(new string[] { lobbyUser.Name, null });
            defaultMessage.MessageList.Add("Lobby chat is now open!");
            messages.Add(defaultMessage);
            Description = description;
            Mode = mode;
            Tags = tags;
        }

        
        public int ID
        {
            get { return Id; }

        }

        public string Name
        {
            get { return name; }
            set { name = value; }
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

        public string TagDis
        {
            // uses ternery operator to either return the tags list in string form or an empty string
            get => Tags != null ? string.Join(",",Tags): string.Empty;
        }

        public void addUser(User user)
        {
            users.Add(user);
        }

        public void removeUser(User user)
        {
            foreach (User searchUser in users)
            {
                if (searchUser.Name.Equals(user.Name))
                {
                    users.Remove(searchUser);
                    break;
                }
            }
        }

        public Message getMessage(User user1, User user2)
        {
            
            //Case for if a user is the lobby ie selecting lobby chat
            if(user1==null || user2 == null)
            {
                Console.WriteLine("Sending messages from lobby chat");
                return messages[0];
            }
            //if not lobby chat, find a message with both users
            foreach (Message message in messages)
            {
                if (message.UserList.Contains(user1.Name) && message.UserList.Contains(user2.Name))
                {
                    Console.WriteLine("Sending messages from " + user1.Name + " to " + user2.Name);
                    return message;
                }
            }
            //If no message with both users, create one
            Message newMessage = new Message(new string[] { user1.Name, user2.Name });
            messages.Add(newMessage);
            Console.WriteLine("Sending new message from " + user1.Name + " to " + user2.Name);
            return newMessage;
        }

        public void updateMessage(Message inMessage)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if (inMessage.UserList.Contains(messages[i].UserList[0]) && inMessage.UserList.Contains(messages[i].UserList[1])){
                    Console.WriteLine("Updating message...");
                    messages[i] = inMessage;
                    break;
                }
            }
        }
    }
}
