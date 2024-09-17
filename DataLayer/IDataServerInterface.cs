/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: IDataServerInterface
Purpose: Defines the contract for the data server interface in the WCF service. This interface outlines the methods for managing users, lobbies, messages, files, and other operations related to the data layer of the gaming lobby system.
Notes: This interface is implemented by the DataServerImplementation class to provide the actual service functionality. It uses WCF OperationContracts to expose methods to clients.
*/

using GameLobbyLib;
using System.Collections.Generic;
using System.ServiceModel;

namespace DataLayer
{
    [ServiceContract]
    public interface IDataServerInterface
    {
        /**
        Method: GetUsers
        Imports: Lobby lobby
        Exports: List<User> (List of users in the given lobby)
        Notes: Retrieves the list of users from a specific lobby.
        */
        [OperationContract]
        List<User> GetUsers(Lobby lobby);

        /**
        Method: GetAllLobbies
        Imports: None
        Exports: List<Lobby> (List of all lobbies)
        Notes: Retrieves a list of all lobbies currently in the system.
        */
        [OperationContract]
        List<Lobby> GetAllLobbies();

        /**
        Method: GetAllUsers
        Imports: None
        Exports: List<User> (List of all users)
        Notes: Retrieves a list of all users in the system.
        */
        [OperationContract]
        List<User> GetAllUsers();

        /**
        Method: GetUser
        Imports: string name (The name of the user to retrieve)
        Exports: User (The user object with the given name)
        Notes: Retrieves a user by their name from the system.
        */
        [OperationContract]
        User GetUser(string name);

        /**
        Method: AddMessage
        Imports: Lobby lobby, User user1, User user2
        Exports: None
        Notes: Adds a message between two users in a specific lobby.
        */
        [OperationContract]
        void AddMessage(Lobby lobby, User user1, User user2);

        /**
        Method: AddUser
        Imports: User user
        Exports: None
        Notes: Adds a new user to the system.
        */
        [OperationContract]
        void AddUser(User user);

        /**
        Method: GetUniqueModes
        Imports: List<Lobby> currLobbyList (List of current lobbies)
        Exports: List<string> (List of unique game modes)
        Notes: Retrieves a list of unique game modes across all or specific lobbies.
        */
        [OperationContract]
        List<string> GetUniqueModes(List<Lobby> currLobbyList);

        /**
        Method: GetUniqueTags
        Imports: List<Lobby> curLobbyList (List of current lobbies)
        Exports: List<string> (List of unique tags)
        Notes: Retrieves a list of unique tags across all or specific lobbies.
        */
        [OperationContract]
        List<string> GetUniqueTags(List<Lobby> curLobbyList);

        /**
        Method: GetfilterdLobbiesList
        Imports: string mode, string tag (Optional filters for lobby mode and tag)
        Exports: List<Lobby> (Filtered list of lobbies)
        Notes: Retrieves a filtered list of lobbies based on mode and tag.
        */
        [OperationContract]
        List<Lobby> GetfilterdLobbiesList(string mode = null, string tag = null);

        /**
        Method: GetAllModeTypes
        Imports: None
        Exports: List<string> (List of all available game modes)
        Notes: Retrieves a list of all available game modes in the system.
        */
        [OperationContract]
        List<string> GetAllModeTypes();

        /**
        Method: GetAllTagTypes
        Imports: None
        Exports: List<string> (List of all available tags)
        Notes: Retrieves a list of all available tags in the system.
        */
        [OperationContract]
        List<string> GetAllTagTypes();

        /**
        Method: AddLobby
        Imports: Lobby lobby
        Exports: None
        Notes: Adds a new lobby to the system.
        */
        [OperationContract]
        void AddLobby(Lobby lobby);

        /**
        Method: GetChats
        Imports: Lobby lobby, User currUser
        Exports: List<Message> (List of chat messages in the lobby)
        Notes: Retrieves a list of chat messages from a specific lobby.
        */
        [OperationContract]
        List<Message> GetChats(Lobby lobby, User currUser);

        /**
        Method: joinLobby
        Imports: Lobby lobby, User user
        Exports: None
        Notes: Adds a user to a specified lobby.
        */
        [OperationContract]
        void joinLobby(Lobby lobby, User user);

        /**
        Method: saveFile
        Imports: string fileName, byte[] fileData, Lobby lobby
        Exports: None
        Notes: Saves a file to the specified lobby for sharing with users.
        */
        [OperationContract]
        void saveFile(string fileName, byte[] fileData, Lobby lobby);

        /**
        Method: downloadFile
        Imports: string fileName
        Exports: byte[] (File data)
        Notes: Downloads a file from the server based on the provided file name.
        */
        [OperationContract]
        byte[] downloadFile(string fileName);

        /**
        Method: RemoveUserFromLobby
        Imports: Lobby lobby, User user
        Exports: None
        Notes: Removes a user from a specific lobby.
        */
        [OperationContract]
        void RemoveUserFromLobby(Lobby lobby, User user);

        /**
        Method: RemoveUser
        Imports: User user
        Exports: None
        Notes: Removes a user from the system entirely.
        */
        [OperationContract]
        void RemoveUser(User user);

        /**
        Method: UpdateMessage
        Imports: Message msg, Lobby lobby
        Exports: None
        Notes: Updates an existing message in a specific lobby.
        */
        [OperationContract]
        void UpdateMessage(Message msg, Lobby lobby);

        /**
        Method: GetMessage
        Imports: User user1, User user2, Lobby lobby
        Exports: Message (The message between the two users in the lobby)
        Notes: Retrieves a message between two users in a specific lobby.
        */
        [OperationContract]
        Message GetMessage(User user1, User user2, Lobby lobby);

        /**
        Method: GetLobby
        Imports: string lobby
        Exports: Lobby (The lobby object with the specified name)
        Notes: Retrieves a lobby based on its name.
        */
        [OperationContract]
        Lobby GetLobby(string lobby);

        /**
        Method: GetLobbyFiles
        Imports: Lobby lobby
        Exports: List<string> (List of file names shared in the lobby)
        Notes: Retrieves the list of files shared within a specific lobby.
        */
        [OperationContract]
        List<string> GetLobbyFiles(Lobby lobby);

        /**
        Method: GetUserCount
        Imports: None
        Exports: int (Total number of users in the system)
        Notes: Retrieves the total number of users currently in the system.
        */
        [OperationContract]
        int GetUserCount();
    }
}
