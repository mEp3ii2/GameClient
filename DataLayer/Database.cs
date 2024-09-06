using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using GameLobbyLib;

namespace DataLayer
{
    
    internal class Database
    {
        private static Database Instance = null;
        public static Database getInstance()
        {
            if (Instance == null)
            {
                Instance = new Database();
            }
            return Instance;
        }
        

        static List<Lobby> lobbies;
        static List<User> users;
        static List<UploadedFile> uploadedFiles;

        List<string> tags;        //test value to be removed later


        private static readonly List<string> gameModes = new List<string> { 
            "Deathmatch", "King of the Hill", "Survival", "Tag Team", "Team Battle", "1v1 Duel", "Tournament", "Time Attack", 
            "Endurance Mode", "Arcade Mode", "Custom Match", "Friendly Match", "Ranked Match", "Story Mode", "Practice Mode" };
        private static readonly List<string> allTags = new List<string> {
            "Beginner Friendly", "Casual", "Competitive", "High Stakes", "Fast-Paced", "Strategic", "Team-Based", "Solo",
            "All-Out Brawl", "Classic", "Intense", "Relaxed", "Focused", "Social", "Hardcore"};

        public Database()
        {
            lobbies = new List<Lobby>();
            User user1 = new User("me");
            User user2 = new User("frank");
            users = new List<User>();
            users.Add(user1);
            users.Add(user2);

            tags = new List<string> { "newbie", "friendly", "casual" };


            addNewLobby("test1", "testing arena", "deathmatch", tags);
            addNewLobby("test2", "testing arena", "King of the Hill", tags);
            addNewLobby("test3", "testing arena", "deathmatch", tags);
            addNewLobby("test4", "testing arena", "King of the Hill", new List<string> {"Solo"});
            lobbies[0].Users.Add(user1);
            lobbies[0].Users.Add(user2);
        }

        public List<Lobby> getAllLobbies()
        {
            return lobbies;
        }

        public List<User> getLobbyUsers(Lobby lobby)
        {
            Lobby thisLobby = getLobby(lobby.Name);
            List<User> users = thisLobby.Users;
            return users;
        }

        public User getUser(string name)
        {
            foreach (User user in users)
            {
                if (user.Name.Equals(name))
                {
                    return user;
                }
            }
            return null;
        }
        public List<User> getAllUsers()
        {
            return users;
        }

        //Remove user from lobby
        public void RemoveUser(Lobby lobby, User user)
        {
            Lobby editLobby = getLobby(lobby.Name);
            editLobby.removeUser(user);

        }

        //Remove user from database
        public void RemoveUser(User user)
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

        //returns a list of all unique modes
        // using either the current set or the full list
        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            if (curLobbyList != null)
            {
                return curLobbyList.Select(lobby => lobby.Mode).Distinct().ToList();
            }
            return lobbies.Select(lobby => lobby.Mode).Distinct().ToList();
        }



        //returns a list of all unique modes
        // using either the current set or the full list
        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            if (curLobbyList != null)
            {
                return curLobbyList.SelectMany(lobby => lobby.Tags).Distinct().ToList();
            }
            return lobbies.SelectMany(lobby=> lobby.Tags).Distinct().ToList();
        }

        public List<Lobby> getfilterdLobbiesList(string mode = null, string tag = null)
        {
            /*List<Lobby> tempList = new List<Lobby>();
            foreach(Lobby lobby in data)
            {
                if (lobby.Mode == mode)
                {
                    tempList.Add(lobby);
                }
            }
            return tempList;*/
            return lobbies.Where(lobby =>(mode == null || lobby.Mode == mode)&&(tag == null || lobby.Tags.Contains(tag))).ToList();
        }

        public List<string> getAllModeTypes()
        {
            return gameModes;
        }

        public List<string> getAllTagTypes()
        {
            return allTags;
        }

        public void addNewLobby(string name,string desc, string mode, List<string> tags)
        {
            Lobby lobby = new Lobby(name, desc, mode, tags);
            foreach (Lobby thislobby in lobbies)
            {
                if (name.Equals(thislobby.Name))
                {
                    throw new Exception("Lobbies cannot have the same name");
                }
            }

            lobbies.Add(lobby);//need to make a call to the server to update lobby list for other clients
        }

        public void addNewLobby(Lobby lobby) {
            lobbies.Add(lobby);
        }

        public void addUser(User user)
        {
            users.Add(user);
        }

        public Lobby getLobby(string name)
        {
            foreach (Lobby lobby in lobbies)
            {
                if (lobby.Name.Equals(name))
                    return lobby;
            }
            return null;
        }

        public void joinLobby(Lobby lobby, User user)
        {
            Lobby changeLobby = getLobby(lobby.Name);
            changeLobby.Users.Add(user);
        }

        


        //public void addFile(int lobbyID,!fileDetails here not sure yet)

        
    }
}
