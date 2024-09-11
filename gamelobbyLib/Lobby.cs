using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GameLobbyLib
{

    public class Lobby : INotifyPropertyChanged
    {
        private static int nextID = 1;
        private string name;
        private List<User> users;
        private List<Message> messages;
        private string description;
        private string mode;
        private List<string> tags;
        public int Id; // if set to private id doesnt get passed to businesss layer and its set to 0 so idk
        public List<string> UploadedFiles { get; set; }  // New list to store uploaded file names
        public event PropertyChangedEventHandler PropertyChanged;

        public Lobby() { } //I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

        public Lobby(string name, string description, string mode, List<string> tags)
        {
            Users = new List<User>();
            Messages = new List<Message>();
            UploadedFiles = new List<string>();  // Initialize file list
            Id = nextID++;
            Name = name;
            messages = new List<Message>();
            Message defaultMessage = new Message(new User[] { null, null });
            defaultMessage.MessageList.Add("Lobby chat is now open!");
            messages.Add(defaultMessage);
            Description = description;
            Mode = mode;
            Tags = tags;
        }

        public override bool Equals(object obj)
        {
            Lobby other = obj as Lobby;
            if (other == null) return false;
            if (Id == other.ID && Name.Equals(other.Name))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Id.GetHashCode();
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
            set
            {
                if (users != value)
                {
                    users = value;
                    OnPropertyChanged(nameof(Users));
                    OnPropertyChanged(nameof(UserCount)); // Update the user count as well
                }
            }
        }

        public int UserCount => Users?.Count ?? 0;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                if (searchUser.Equals(user))
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
                if (message.UserList.Contains(user1) && message.UserList.Contains(user2))
                {
                    Console.WriteLine("Sending messages from " + user1.Name + " to " + user2.Name);
                    return message;
                }
            }
            //If no message with both users, create one
            Message newMessage = new Message(new User[] { user1, user2 });
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

        public void AddFile(string fileName)
        {
            if (!UploadedFiles.Contains(fileName))
            {
                UploadedFiles.Add(fileName);
            }
        }
    }
}
