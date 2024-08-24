using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace DataLayer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServerImplementation : IDataServerInterface
    {
        private static Database database;
        public DataServerImplementation()
        {
            database = new Database();
        }

        public List<Lobby> GetAllLobbies()
        {
            return database.getAllLobbies();
        }

        public List<User> GetUsers(Lobby lobby)
        {
            return database.getLobbyUsers(lobby);
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
            return database.getfilterdLobbiesList(mode,tag);
        }

        public List<string> GetAllModeTypes()
        {
            return Database.getAllModeTypes();
        }

        public List<string> GetAllTagTypes()
        {
            return Database.getAllTagTypes();
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
            database.RemoveUser(lobby, user);
        }
    }
}
