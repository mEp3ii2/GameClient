using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{

    [Serializable]
    public class Lobby
    {
        private string name;
        private string host;
        private List<User> users;
        private string title;
        private string description;
        private string mode;
        private List<string> tags;
        private readonly int userCount;

        public Lobby(string name, User hostUser, string title, string description, string mode, List<string> tags)
        {
            Name = name;
            Host = hostUser.ToString();
            users = new List<User>();
            users.Add(hostUser);
            Title = title; //What is the difference between lobby name and lobby title?
            Description = description;
            Mode = mode;
            Tags = tags;
            userCount= users.Count();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public List<User> Users 
        { 
            get => users; 
            set => users = value; 
        }
        public string Title 
        { 
            get => title; 
            set => title = value; 
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

        public int UserCount
        {
            get => userCount;
        }

        public void addUser(User user)
        {
            users.Add(user);
        }

        public void removeUser(User user)
        {
            if (users.Contains(user))
            {
                users.Remove(user);
            }
            else
            {
                Console.WriteLine("User not found: " + user.ToString());
            }

        }

        public string TagDis
        {
            // uses ternery operator to either return the tags list in string form or an empty string
            get => Tags != null ? string.Join(",",Tags): string.Empty;
        }
    }
}
