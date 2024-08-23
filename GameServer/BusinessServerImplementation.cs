﻿using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Runtime.CompilerServices;
using System.IO;

namespace BusinessLayer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServerImplementation : BusinessServerInterface
    {
        private DataServerInterface foob;
        private static uint logNumber = 0;
        public BusinessServerImplementation() {

            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8200/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public List<Lobby> GetAllLobbies()
        {
            return foob.GetAllLobbies();
        }

        public List<User> GetUsers(Lobby lobby)
        {
            return foob.GetUsers(lobby);
        }

        public List<User> GetAllUsers()
        {
            return foob.GetAllUsers();
        }

        public void AddUser(User user)
        {
            Log($"Added new user: {user.Name}");
            foob.AddUser(user);
        }

        public List<string> GetUniqueModes(List<Lobby> curLobbyList)
        {
           return foob.GetUniqueModes(curLobbyList);
        }

        public List<string> GetUniqueTags(List<Lobby> curLobbyList)
        {
            return foob.GetUniqueTags(curLobbyList);
        }

        public List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null)
        {
            return foob.GetfilterdLobbiesList(mode, tag);
        }

        public List<string> GetAllModeTypes()
        {
            return foob.GetAllModeTypes();
        }

        public List<string> GetAllTagTypes()
        {
            return foob.GetAllTagTypes();
        }

        public void AddLobby(Lobby lobby)
        {
            Log($"New lobby created: {lobby.Name}");
            foob.AddLobby(lobby);
        }

        public bool UniqueUser(string userName)
        {
            Log($"Attempted Login with username {userName}");
            List<User> users = foob.GetAllUsers();
            
            foreach (User user in users)
            {
                Console.WriteLine(user.Name);
                if (user.Name == userName)
                {
                    Log($"Login failed {userName} already used");
                    return false;
                }
            }
            Log($"Login for {userName} successful ");
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logNumber++;
            string logMsg = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMsg);
        }

        public void UploadFile(byte[] fileData, string fileName)
        {
            Log($"Recieved file upload: {fileName}");

            foob.saveFile(fileName, fileData);
        }

        public void DownloadFile()
        {
            Log($"File download by ?");
        }
    }
}
