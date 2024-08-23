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
        private List<User> users;
        private string description;
        private string mode;
        private List<string> tags;
        private readonly int userCount;
        private List<List<string>> lobbyMsgs;
        private List<UploadedFile> files;

        public Lobby() { } //I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

        public Lobby(string name, User hostUser, string title, string description, string mode, List<string> tags)
        {
            Name = name;
            users = new List<User>();
            Description = description;
            Mode = mode;
            Tags = tags;
            userCount= users.Count();
            lobbyMsgs = new List<List<string>>();
            files = new List<UploadedFile>();
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

        public List<List<string>> LobbyMsg
        {
            get => lobbyMsgs;
            set => lobbyMsgs = value;
        }

        public List<UploadedFile> Files
        {
            get => files;
            set => files = value;
        }
    }
}
