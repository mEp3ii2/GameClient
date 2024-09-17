/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: Database
Purpose: Manages data related to lobbies, users, and other related entities in the system. This class serves as a singleton to provide consistent data storage across the application.
Notes: This class stores and retrieves data for lobbies, users, and tags. It also supports filtering and retrieval of unique modes and tags.
*/

using System;
using System.Collections.Generic;
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

        /**
        Method: getInstance
        Imports: None
        Exports: Database (The singleton instance of the Database)
        Notes: Ensures that only one instance of the Database class is created and reused throughout the application.
        Algorithm: Checks if an instance already exists. If not, creates a new instance. Returns the single instance of the Database class.
        */
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
        static List<UploadedFile> uploadedFiles;
        List<string> tags;

        private static readonly List<string> gameModes = new List<string> {
            "Deathmatch", "King of the Hill", "Survival", "Tag Team", "Team Battle", "1v1 Duel", "Tournament", "Time Attack",
            "Endurance Mode", "Arcade Mode", "Custom Match", "Friendly Match", "Ranked Match", "Story Mode", "Practice Mode" };

        private static readonly List<string> allTags = new List<string> {
            "Beginner Friendly", "Casual", "Competitive", "High Stakes", "Fast-Paced", "Strategic", "Team-Based", "Solo",
            "All-Out Brawl", "Classic", "Intense", "Relaxed", "Focused", "Social", "Hardcore"};

        /**
        Method: Database (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the lists for lobbies and users, as well as a predefined set of game modes and tags.
        Algorithm: Populates the lobbies with initial data and adds users to the system for testing purposes.
        */
        public Database()
        {
            lobbies = new List<Lobby>();
            User user1 = new User("George");
            User user2 = new User("Clooney");
            users = new List<User> { user1, user2 };

            tags = new List<string> { "Beginner Friendly", "Ranked Match" };

            addNewLobby("Beginner Deathmatch", "A beginner friendly chat lobby for Deathmatch players.", "Deathmatch", new List<string> { "Beginner Friendly" });
            addNewLobby("Beginner King of the Hill", "A beginner friendly chat lobby for King of the Hill players.", "King of the Hill", new List<string> { "Beginner Friendly" });
            addNewLobby("Hardcore Deathmatch", "A chat lobby for hardcore Deathmatch players.", "Deathmatch", new List<string> { "Ranked Match" });
            addNewLobby("Hardcore King of the Hill", "A chat lobby for hardcore King of the Hill players.", "King of the Hill", new List<string> { "Ranked Match" });
            lobbies[0].Users.Add(user1);
            lobbies[0].Users.Add(user2);
        }

        /**
        Method: getAllLobbies
        Imports: None
        Exports: List<Lobby> (All active lobbies)
        Notes: Retrieves all the lobbies currently active in the system.
        Algorithm: Returns the list of lobbies stored in the Database.
        */
        public List<Lobby> getAllLobbies()
        {
            return lobbies;
        }

        /**
        Method: getLobbyUsers
        Imports: Lobby lobby (The lobby for which to retrieve the users)
        Exports: List<User> (Users in the specified lobby)
        Notes: Retrieves the list of users present in a specific lobby.
        Algorithm: Returns the list of users associated with the provided lobby.
        */
        public List<User> getLobbyUsers(Lobby lobby)
        {
            return lobby.Users;
        }

        /**
        Method: GetUser
        Imports: string name (The username of the user to retrieve)
        Exports: User (The user with the specified name, or null if not found)
        Notes: Retrieves a specific user by their username.
        Algorithm: Iterates over the list of users and returns the user if found. Returns null if no match is found.
        */
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

        /**
        Method: getAllUsers
        Imports: None
        Exports: List<User> (All users in the system)
        Notes: Retrieves the list of all users currently registered in the system.
        Algorithm: Returns the entire list of users.
        */
        public List<User> getAllUsers()
        {
            return users;
        }

        /**
        Method: RemoveUser
        Imports: Lobby lobby, User user (The lobby and user to be removed)
        Exports: None
        Notes: Removes a user from a specific lobby.
        Algorithm: Locates the lobby, and removes the user from the lobby's user list.
        */
        public void RemoveUser(Lobby lobby, User user)
        {
            Lobby editLobby = getLobby(lobby);
            editLobby.removeUser(user);
        }

        /**
        Method: RemoveUser (Overloaded)
        Imports: User user (The user to be removed)
        Exports: None
        Notes: Removes a user from the system, including removing them from any lobbies they are part of.
        Algorithm: Removes the user from any lobbies they are a member of, and then from the system's user list.
        */
        public void RemoveUser(User user)
        {
            foreach (Lobby lobby in lobbies)
            {
                if (lobby.Users.Contains(user))
                {
                    RemoveUser(lobby, user);
                }
            }

            users.Remove(user);
        }

        /**
        Method: GetUniqueModes
        Imports: List<Lobby> curLobbyList (Optional lobby list to filter by)
        Exports: List<string> (Unique game modes in the system or the provided lobby list)
        Notes: Retrieves the unique game modes available in the current system or a subset of lobbies.
        Algorithm: Returns distinct game modes, either from the entire system or the provided lobby list.
        */
        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            return curLobbyList != null ? curLobbyList.Select(lobby => lobby.Mode).Distinct().ToList() : lobbies.Select(lobby => lobby.Mode).Distinct().ToList();
        }

        /**
        Method: GetUniqueTags
        Imports: List<Lobby> curLobbyList (Optional lobby list to filter by)
        Exports: List<string> (Unique tags in the system or the provided lobby list)
        Notes: Retrieves the unique tags available in the current system or a subset of lobbies.
        Algorithm: Returns distinct tags, either from the entire system or the provided lobby list.
        */
        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return curLobbyList != null ? curLobbyList.SelectMany(lobby => lobby.Tags).Distinct().ToList() : lobbies.SelectMany(lobby => lobby.Tags).Distinct().ToList();
        }

        /**
        Method: getfilterdLobbiesList
        Imports: string mode, string tag (Optional filtering parameters for mode and tag)
        Exports: List<Lobby> (Lobbies filtered by the provided mode and/or tag)
        Notes: Filters the list of lobbies based on the specified mode and/or tag.
        Algorithm: Returns a list of lobbies that match the given mode and/or tag.
        */
        public List<Lobby> getfilterdLobbiesList(string mode = null, string tag = null)
        {
            return lobbies.Where(lobby => (mode == null || lobby.Mode == mode) && (tag == null || lobby.Tags.Contains(tag))).ToList();
        }

        /**
        Method: getAllModeTypes
        Imports: None
        Exports: List<string> (All available game modes)
        Notes: Retrieves the list of all game modes supported by the system.
        Algorithm: Returns the static list of game modes.
        */
        public List<string> getAllModeTypes()
        {
            return gameModes;
        }

        /**
        Method: getAllTagTypes
        Imports: None
        Exports: List<string> (All available tags)
        Notes: Retrieves the list of all tags that can be associated with lobbies.
        Algorithm: Returns the static list of all tags.
        */
        public List<string> getAllTagTypes()
        {
            return allTags;
        }

        /**
        Method: addNewLobby
        Imports: string name, string desc, string mode, List<string> tags
        Exports: None
        Notes: Adds a new lobby to the system with the specified name, description, mode, and tags.
        Algorithm: Creates a new Lobby object and adds it to the list of lobbies, ensuring that no lobbies share the same name.
        */
        public void addNewLobby(string name, string desc, string mode, List<string> tags)
        {
            Lobby lobby = new Lobby(name, desc, mode, tags);
            if (lobbies.Any(l => l.Name.Equals(name)))
            {
                throw new Exception("Lobbies cannot have the same name");
            }
            lobbies.Add(lobby);
        }

        /**
        Method: addNewLobby (Overloaded)
        Imports: Lobby lobby (The lobby to be added)
        Exports: None
        Notes: Adds a new lobby to the system.
        Algorithm: Adds the provided lobby object to the list of lobbies.
        */
        public void addNewLobby(Lobby lobby)
        {
            lobbies.Add(lobby);
        }

        /**
        Method: addUser
        Imports: User user (The user to be added to the system)
        Exports: None
        Notes: Adds a new user to the system.
        Algorithm: Adds the provided user object to the list of users.
        */
        public void addUser(User user)
        {
            users.Add(user);
        }

        /**
        Method: getLobby
        Imports: string lobbyName (The name of the lobby to retrieve)
        Exports: Lobby (The lobby object with the specified name, or null if not found)
        Notes: Retrieves a lobby by its name.
        Algorithm: Searches for the lobby by name and returns it if found. Returns null if no match is found.
        */
        public Lobby getLobby(string lobbyName)
        {
            return lobbies.FirstOrDefault(lobby => lobby.Name.Equals(lobbyName));
        }

        /**
        Method: getLobby (Overloaded)
        Imports: Lobby lobby (The lobby object to retrieve)
        Exports: Lobby (The matching lobby object, or null if not found)
        Notes: Retrieves a lobby object from the list of lobbies.
        Algorithm: Searches for the lobby object in the list and returns it if found. Returns null if no match is found.
        */
        public Lobby getLobby(Lobby lobby)
        {
            return lobbies.FirstOrDefault(searchLobby => searchLobby.Equals(lobby));
        }

        /**
        Method: joinLobby
        Imports: Lobby lobby, User user (The lobby to join and the user joining it)
        Exports: None
        Notes: Adds the specified user to the list of users in the provided lobby.
        Algorithm: Locates the lobby by name and adds the user to its user list.
        */
        public void joinLobby(Lobby lobby, User user)
        {
            Lobby changeLobby = getLobby(lobby.Name);
            changeLobby.Users.Add(user);
        }

        /**
        Method: GetUserCount
        Imports: None
        Exports: int (The total number of users in the system)
        Notes: Returns the total count of users currently registered in the system.
        Algorithm: Returns the size of the user list.
        */
        public int GetUserCount()
        {
            return users.Count;
        }
    }
}
