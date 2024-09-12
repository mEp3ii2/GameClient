using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        

        List<Lobby> lobbies;
        List<User> users;
        // static List<UploadedFile> uploadedFiles;

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
            User user1 = new User("George");
            User user2 = new User("Clooney");
            users = new List<User>();
            users.Add(user1);
            users.Add(user2);

            tags = new List<string> { "Beginner Friendly", "Ranked Match" };


            addNewLobby("Beginner Deathmatch", "A beginner friendly chat lobby for Deathmatch players.", "Deathmatch", new List<string> { "Beginner Friendly" });
            addNewLobby("Beginner King of the Hill", "A beginner friendly chat lobby for King of the Hill players.", "King of the Hill", new List<string> { "Beginner Friendly" });
            addNewLobby("Hardcore Deathmatch", "A chat lobby for hardcore Deathmatch players.", "Deathmatch", new List<string> { "Ranked Match" });
            addNewLobby("Hardcore King of the Hill", "A chat lobby for hardcore King of the Hill players.", "King of the Hill", new List<string> {"Ranked Match"});
            lobbies[0].Users.Add(user1);
            lobbies[0].Users.Add(user2);
        }

        public List<Lobby> getAllLobbies()
        {
            return lobbies;
        }

        public List<User> getLobbyUsers(Lobby lobby)
        {
            return lobby.Users;
        }

        public User GetUser(string name)
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
            // Check if the lobby is null before proceeding
            if (lobby == null)
            {
                throw new ArgumentNullException(nameof(lobby), "The provided lobby is null.");
            }

            // Rest of the logic to remove the user
            Lobby editLobby = getLobby(lobby);

            // Check if editLobby is null before proceeding
            if (editLobby == null)
            {
                throw new Exception($"Lobby '{lobby.Name}' not found in the database.");
            }

            editLobby.removeUser(user);
        }

        //Remove user from database
        public void RemoveUser(User user)
        {
            //Remove user from any lobbies they are in
            foreach (Lobby lobby in lobbies)
            {
                if (lobby.Users.Contains(user))
                {
                    RemoveUser(lobby, user);
                }
            }

            //Remove user from the application
            foreach (User searchUser in users)
            {
                if (searchUser.Equals(user))
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
            return lobbies.SelectMany(lobby => lobby.Tags).Distinct().ToList();
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

        public Lobby getLobby(string lobbyName)
        {
            foreach (Lobby searchLobby in lobbies)
            {
                if (lobbyName.Equals(searchLobby.Name))
                    return searchLobby;
            }
            return null;
        }

        public Lobby getLobby(Lobby lobby)
        {
            if (lobby == null)
            {
                throw new ArgumentNullException(nameof(lobby), "Lobby cannot be null.");
            }

            foreach (Lobby searchLobby in lobbies)
            {
                if (lobby.Equals(searchLobby))
                {
                    return searchLobby;
                }
            }

            // If no lobby was found, log the issue
            Console.WriteLine($"Lobby '{lobby.Name}' not found.");
            return null;
        }

        public void joinLobby(Lobby lobby, User user)
        {
            Lobby changeLobby = getLobby(lobby.Name);
            changeLobby.Users.Add(user);
        }

        public int GetUserCount()
        {
            return users.Count;
        }

        public void SaveFilePath(string fileName, Lobby lobby)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedFiles", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Ensure the directory exists

            Lobby currentLobby = getLobby(lobby);
            currentLobby?.AddFile(filePath);  // Store the file path instead of file content
        }

        public List<string> GetLobbyFilePaths(Lobby lobby)
        {
            Lobby searchLobby = getLobby(lobby);
            return searchLobby?.UploadedFiles ?? new List<string>();
        }
    }
}
