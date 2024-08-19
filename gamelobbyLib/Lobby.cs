using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
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

        public Lobby() { } //I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

        public Lobby(string name, User hostUser, string title, string description, string mode, List<string> tags)
        {
            Name = name;
            Host = hostUser.Name;
            users = new List<User>();
            Title = title;
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

        public string TagDis
        {
            // uses ternery operator to either return the tags list in string form or an empty string
            get => Tags != null ? string.Join(",",Tags): string.Empty;
        }
    }
}
