using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    public class Database
    {
        List<Lobby> data;
        List<string> users;
        List<string> tags;        

        private static readonly List<string> gameModes = new List<string> { 
            "Deathmatch", "King of the Hill", "Survival", "Tag Team", "Team Battle", "1v1 Duel", "Tournament", "Time Attack", 
            "Endurance Mode", "Arcade Mode", "Custom Match", "Friendly Match", "Ranked Match", "Story Mode", "Practice Mode" };
        private static readonly List<string> allTags = new List<string> {
            "Beginner Friendly", "Casual", "Competitive", "High Stakes", "Fast-Paced", "Strategic", "Team-Based", "Solo",
            "All-Out Brawl", "Classic", "Intense", "Relaxed", "Focused", "Social", "Hardcore"};
        public Database()
        {
            data = new List<Lobby>();
            users = new List<string> { "me", "frank" };
            tags = new List<string> { "newbie", "friendly", "casual" };
            

            Lobby testLobby = new Lobby("test", "me", users, "testLobb", "testing arena", "deathmatch", tags);
            Lobby testLobby2 = new Lobby("test", "me", users, "testLobb", "testing arena", "King of the Hill", tags);
            Lobby testLobby3 = new Lobby("test", "me", users, "testLobb", "testing arena", "deathmatch", tags);
            Lobby testLobby4 = new Lobby("test", "me", users, "testLobb", "testing arena", "King of the Hill", new List<string> {"Solo"});
            data.Add(testLobby);
            data.Add(testLobby2);
            data.Add(testLobby3);
            data.Add(testLobby4);
        }

        public List<Lobby> getAllLobbies()
        {
            return data;
        }

        //returns a list of all unique modes
        // using either the current set or the full list
        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            if (curLobbyList != null)
            {
                return curLobbyList.Select(lobby => lobby.Mode).Distinct().ToList();
            }
            return data.Select(lobby => lobby.Mode).Distinct().ToList();
        }



        //returns a list of all unique modes
        // using either the current set or the full list
        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            if (curLobbyList != null)
            {
                return curLobbyList.SelectMany(lobby => lobby.Tags).Distinct().ToList();
            }
            return data.SelectMany(lobby=> lobby.Tags).Distinct().ToList();
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
            return data.Where(lobby =>(mode == null || lobby.Mode == mode)&&(tag == null || lobby.Tags.Contains(tag))).ToList();
        }

        public static List<string> getAllModeTypes()
        {
            return gameModes;
        }

        public static List<string> getAllTagTypes()
        {
            return allTags;
        }

        public void addNewLobby(string name,string userName,string desc, string mode, List<string> tags)
        {
            Lobby lobby = new Lobby(name, userName, new List<string> { userName}, name, desc, mode, tags);
            data.Add(lobby);//need to make a call to the server to update lobby list for other clients
        }
    }
}
