/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: IBusinessServerInterface
Purpose: Defines the operations available for the business server, including lobby management, messaging, user management, and file handling.
Notes: This interface contains the method signatures for the operations that the business server will implement and expose through a WCF service.
*/

using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ServiceContract]
    public interface IBusinessServerInterface
    {
        /**
        Method: GetUsers
        Imports: string lobbyName
        Exports: List<string> (Usernames of players in the specified lobby)
        Notes: Retrieves the usernames of all players in a given lobby.
        Algorithm: Calls the data layer to fetch the list of users in the specified lobby and returns their usernames.
        */
        [OperationContract]
        List<string> GetUsers(string lobbyName);

        /**
        Method: GetAllLobbies
        Imports: None
        Exports: List<Lobby> (A list of all active lobbies)
        Notes: Retrieves all active lobbies from the data layer.
        Algorithm: Fetches the list of lobbies from the data server and returns it to the caller.
        */
        [OperationContract]
        List<Lobby> GetAllLobbies();

        /**
        Method: GetUser
        Imports: string name (Username)
        Exports: User (The user with the specified username)
        Notes: Retrieves a specific user from the data layer.
        Algorithm: Calls the data layer to fetch the user with the specified username.
        */
        [OperationContract]
        User GetUser(string name);

        /**
        Method: AddMessage
        Imports: Lobby lobby, User user1, User user2
        Exports: None
        Notes: Adds a new message between two users in a specific lobby.
        Algorithm: Sends the message details to the data layer for storage.
        */
        [OperationContract]
        void AddMessage(Lobby lobby, User user1, User user2);

        /**
        Method: GetAllUsers
        Imports: None
        Exports: List<User> (All users currently in the system)
        Notes: Retrieves a list of all users from the data layer.
        Algorithm: Calls the data layer to fetch all users and returns the result.
        */
        [OperationContract]
        List<User> GetAllUsers();

        /**
        Method: AddUser
        Imports: string userName (Username to be added)
        Exports: None
        Notes: Adds a new user to the system.
        Algorithm: Sends the user details to the data layer to be added.
        */
        [OperationContract]
        void AddUser(string userName);

        /**
        Method: GetUniqueModes
        Imports: List<Lobby> currLobbyList (Current list of lobbies)
        Exports: List<string> (Unique game modes in the current lobby list)
        Notes: Retrieves the unique game modes available in the current list of lobbies.
        Algorithm: Calls the data layer to fetch unique modes from the provided lobby list or all lobbies.
        */
        [OperationContract]
        List<string> GetUniqueModes(List<Lobby> currLobbyList);

        /**
        Method: GetUniqueTags
        Imports: List<Lobby> curLobbyList (Current list of lobbies)
        Exports: List<string> (Unique tags from the current list of lobbies)
        Notes: Retrieves the unique tags from the current list of lobbies.
        Algorithm: Calls the data layer to fetch unique tags from the provided lobby list or all lobbies.
        */
        [OperationContract]
        List<string> GetUniqueTags(List<Lobby> curLobbyList);

        /**
        Method: GetfilterdLobbiesList
        Imports: string mode, string tag (Optional filtering parameters)
        Exports: List<Lobby> (Lobbies filtered by mode and/or tag)
        Notes: Retrieves a list of lobbies filtered by the provided mode and/or tag.
        Algorithm: Calls the data layer to fetch lobbies that match the provided mode and tag criteria.
        */
        [OperationContract]
        List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null);

        /**
        Method: GetAllModeTypes
        Imports: None
        Exports: List<string> (All available game modes)
        Notes: Retrieves all the game modes supported by the system.
        Algorithm: Calls the data layer to fetch the list of game modes.
        */
        [OperationContract]
        List<string> GetAllModeTypes();

        /**
        Method: GetAllTagTypes
        Imports: None
        Exports: List<string> (All available tags for lobbies)
        Notes: Retrieves all the tags that can be assigned to lobbies.
        Algorithm: Calls the data layer to fetch the list of available tags.
        */
        [OperationContract]
        List<string> GetAllTagTypes();

        /**
        Method: AddLobby
        Imports: string roomName, string desc, string mode, List<string> tags
        Exports: None
        Notes: Adds a new lobby to the system with the specified name, description, mode, and tags.
        Algorithm: Sends the lobby details to the data layer for storage.
        */
        [OperationContract]
        void AddLobby(string roomName, string desc, string mode, List<string> tags);

        /**
        Method: UniqueUser
        Imports: string userName (Username to be checked)
        Exports: bool (True if the username is unique, false otherwise)
        Notes: Checks whether a username is unique in the system.
        Algorithm: Calls the data layer to check if the provided username already exists.
        */
        [OperationContract]
        bool UniqueUser(string userName);

        /**
        Method: UploadFile
        Imports: byte[] fileData, string fileName, string lobbyName
        Exports: None
        Notes: Uploads a file to the specified lobby for sharing among users.
        Algorithm: Sends the file data and lobby information to the data layer for storage.
        */
        [OperationContract]
        void UploadFile(byte[] fileData, string fileName, string lobbyName);

        /**
        Method: DownloadFile
        Imports: string fileName (Name of the file to be downloaded)
        Exports: byte[] (The file data)
        Notes: Downloads a file from the system based on its name.
        Algorithm: Calls the data layer to retrieve the file and return it to the caller.
        */
        [OperationContract]
        byte[] DownloadFile(string fileName);

        /**
        Method: RemoveUserFromLobby
        Imports: string lobbyName, string userName (Lobby and username of the user to be removed)
        Exports: None
        Notes: Removes a user from the specified lobby.
        Algorithm: Calls the data layer to remove the user from the given lobby.
        */
        [OperationContract]
        void RemoveUserFromLobby(string lobbyName, string userName);

        /**
        Method: RemoveUser
        Imports: string user (Username of the user to be removed)
        Exports: None
        Notes: Removes a user from the system.
        Algorithm: Calls the data layer to remove the specified user from the system.
        */
        [OperationContract]
        void RemoveUser(string user);

        /**
        Method: getChats
        Imports: Lobby lobby, User currUser (Lobby and user information)
        Exports: List<Message> (List of messages in the lobby)
        Notes: Retrieves all chat messages for the specified lobby and user.
        Algorithm: Calls the data layer to fetch chat messages for the given lobby and user.
        */
        [OperationContract]
        List<Message> getChats(Lobby lobby, User currUser);

        /**
        Method: UpdateMessage
        Imports: List<string> messageText, string lobby, string userName1, string userName2
        Exports: None
        Notes: Updates an existing message between two users in a lobby.
        Algorithm: Sends the message updates to the data layer for storage.
        */
        [OperationContract]
        void UpdateMessage(List<string> messageText, string lobby, string userName1, string userName2);

        /**
        Method: joinLobby
        Imports: string lobbyName, string userName (Lobby name and username of the user joining)
        Exports: None
        Notes: Adds a user to the specified lobby.
        Algorithm: Calls the data layer to add the user to the given lobby.
        */
        [OperationContract]
        void joinLobby(string lobbyName, string userName);

        /**
        Method: GetMessage
        Imports: string userName1, string userName2, string lobbyName
        Exports: List<string> (Messages between the two users in the lobby)
        Notes: Retrieves the chat messages between two users in the specified lobby.
        Algorithm: Calls the data layer to fetch messages between the two users in the lobby.
        */
        [OperationContract]
        List<string> GetMessage(string userName1, string userName2, string lobbyName);

        /**
        Method: GetLobbyFiles
        Imports: string lobbyName (Name of the lobby)
        Exports: List<string> (List of files shared in the lobby)
        Notes: Retrieves the list of files that have been shared in the specified lobby.
        Algorithm: Calls the data layer to fetch the list of shared files for the lobby.
        */
        [OperationContract]
        List<string> GetLobbyFiles(string lobbyName);

        /**
        Method: GetUserCount
        Imports: None
        Exports: int (The total number of users in the system)
        Notes: Retrieves the total number of users currently registered in the system.
        Algorithm: Calls the data layer to count the total number of users.
        */
        [OperationContract]
        int GetUserCount();
    }
}
