using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLobbyLib;

namespace DataLayer
{
    internal class Database
    {
        List<Lobby> lobbies;
        List<User> users;
        List<string> tags;        

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
            

            Lobby testLobby = new Lobby("test", user1, "testLobb", "testing arena", "deathmatch", tags);
            Lobby testLobby2 = new Lobby("test", user2, "testLobb", "testing arena", "King of the Hill", tags);
            Lobby testLobby3 = new Lobby("test", user1, "testLobb", "testing arena", "deathmatch", tags);
            Lobby testLobby4 = new Lobby("test", user2, "testLobb", "testing arena", "King of the Hill", new List<string> {"Solo"});
            lobbies.Add(testLobby);
            lobbies.Add(testLobby2);
            lobbies.Add(testLobby3);
            lobbies.Add(testLobby4);
        }

        public List<Lobby> getAllLobbies()
        {
            return lobbies;
        }

        public List<User> getLobbyUsers(Lobby lobby)
        {
            return lobby.Users;
        }

        public List<User> getAllUsers()
        {
            return users;
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

        public static List<string> getAllModeTypes()
        {
            return gameModes;
        }

        public static List<string> getAllTagTypes()
        {
            return allTags;
        }

        public void addNewLobby(string name,User hostUser,string desc, string mode, List<string> tags)
        {
            Lobby lobby = new Lobby(name, hostUser, name, desc, mode, tags);
            lobbies.Add(lobby);//need to make a call to the server to update lobby list for other clients
        }

        public void addNewLobby(Lobby lobby) {
            lobbies.Add(lobby);
        }

        public void addUser(User user)
        {
            users.Add(user);
        }
    }
}
