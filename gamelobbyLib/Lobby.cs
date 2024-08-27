using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{

    public class Lobby
    {
        private static int nextID = 1;
        private string name;
        private List<User> users;
        private string description;
        private string mode;
        private List<string> tags;
        private int userCount;
        public int Id; // if set to private id doesnt get passed to businesss layer and its set to 0 so idk


        public Lobby() { } //I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

        public Lobby(string name,  string description, string mode, List<string> tags)
        {
            Id = nextID++;
            Name = name;
            users = new List<User>();
            Description = description;
            Mode = mode;
            Tags = tags;
            userCount = users.Count();
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
