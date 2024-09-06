using GameLobbyLib;
using System;
using System.Collections.Generic;
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

        public void RemoveUser(Lobby lobby, User user)
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

        public void UpdateMessage(Message msg)
        {
            database.UpdateMessage(msg);
        }

        public void joinLobby(Lobby lobby, User user)
        {
            Log($"Adding user {user.Name} to lobby {lobby.ID}");
            database.joinLobby(lobby, user);
        }


        public List<Message> GetChats(Lobby lobby, User currUser)
        {
            Log($"Retrieving Chats associated with lobby id {lobby.ID} for user {currUser}");
            List<Message> lobMes = database.getChats(lobby, currUser);
            foreach (Message message in lobMes)
            {
                Log($"Retrieved message: {message.LobbyID}");
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

    }
}
