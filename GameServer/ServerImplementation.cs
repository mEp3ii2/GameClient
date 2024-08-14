﻿using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ServerImplementation : ServerInterface
    {
        Database database;
        public ServerImplementation() {
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
            return database.getfilterdLobbiesList();
        }

        public List<string> GetAllModeTypes()
        {
            return Database.getAllModeTypes();
        }

        public List<string> GetAllTagTypes()
        {
            return Database.getAllTagTypes();
        }
    }
}
