using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DataLayer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServerImplementation : IDataServerInterface //change back to internal later
    {
        private static Database database;
        private static uint logNumber = 0;
        public DataServerImplementation()
        {
            database = Database.getInstance();
        }

        public List<Lobby> GetAllLobbies()
        {
            Log($"Grabbing all Lobbies");
            List<Lobby> lob = database.getAllLobbies();
            foreach (Lobby lobb in lob)
            {
                Log($"Lobby {lobb.Name}, ID: {lobb.ID}");
                foreach(User user in lobb.Users)
                {
                    Log($"\tUser: {user.Name}");
                }
            }
            return lob;
        }

        public List<User> GetUsers(Lobby lobby)
        {
            Log($"Getting users from lobby {lobby.Name}");
            List<User> users = database.getLobbyUsers(lobby);
            foreach (User user in users)
            {
                Log($"{user}");
            }
            return users;
        }

        public User GetUser(string name)
        {
            return database.getUser(name);
        }

        public List<User> GetAllUsers()
        {
            return database.getAllUsers();
        }

        public void AddUser(User user)
        {
            database.addUser(user);
        }

        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
            return database.GetUniqueModes(curLobbyList);
        }

        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return database.GetUniqueTags(curLobbyList);
        }

        public List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null)
        {
            return database.getfilterdLobbiesList(mode, tag);
        }

        public List<string> GetAllModeTypes()
        {
            return database.getAllModeTypes();
        }

        public List<string> GetAllTagTypes()
        {
            return database.getAllTagTypes();
        }

        public void AddLobby(Lobby lobby)
        {
            database.addNewLobby(lobby);
            
        }

        public void saveFile(string fileName, byte[] fileData)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromLobby(Lobby lobby, User user)
        {
            Log($"Remove user {user.Name} from lobby {lobby.Name}");
            database.RemoveUser(lobby, user);
            Log($"Update Lobby list:");
            List<User> users = database.getLobbyUsers(lobby);
            foreach (var item in users)
            {
                Log($"{item.Name}");
            }
        }

        public void RemoveUser(User user)
        {
            database.RemoveUser(user);
        }

        public void UpdateMessage(Message msg, Lobby lobby)
        {
            Lobby thisLobby = database.getLobby(lobby.Name);
            thisLobby.updateMessage(msg);
        }

        public void joinLobby(Lobby lobby, User user)
        {
            Log($"Adding user {user.Name} to lobby {lobby.ID}");
            database.joinLobby(lobby, user);
        }


        public List<Message> GetChats(Lobby lobby, User currUser)
        {
            Log($"Retrieving Chats associated with lobby id {lobby.ID} for user {currUser}");
            Lobby thisLobby =  database.getLobby(lobby.Name);
            List<Message> lobMes = new List<Message>();
            foreach (Message message in thisLobby.Messages)
            {
                if (message.UserList.Contains(null) || message.UserList.Contains(currUser.Name))
                {
                    lobMes.Add(message);
                }
            }
            return lobMes;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        public void AddMessage(Lobby lobby, User user1, User user2)
        {
            Lobby thisLobby = database.getLobby(lobby.Name);
            thisLobby.Messages.Add(new Message(new string[] {user1.Name, user2.Name}));
        }

        public Message GetMessage(User user1, User user2, Lobby lobby)
        {
            Lobby thisLobby = database.getLobby(lobby.Name);
            return thisLobby.getMessage(user1, user2);
        }

        public Lobby GetLobby(Lobby lobby)
        {
            return database.getLobby(lobby.Name);
        }
    }
}
