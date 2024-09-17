/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: UploadedFile
Purpose: Represents a file that has been uploaded to a lobby. Contains the file name, its content in bytes, and the associated lobby ID.
Notes: This class is used to store the file information and content for file sharing within a lobby. The `lobbyID` is used to associate the file with a specific lobby.
*/

using System;

namespace GameLobbyLib
{
    public class UploadedFile
    {
        // Read-only property for the ID of the lobby to which the file is uploaded
        public int lobbyID { get; }

        // Property to get or set the file's name
        public string FileName { get; set; }

        // Property to get or set the file's content as a byte array
        public byte[] FileContent { get; set; }

        /**
        Constructor: UploadedFile(int lobbyID, string fileName, byte[] fileContent)
        Purpose: Initializes a new UploadedFile instance with the provided lobby ID, file name, and file content.
        */
        public UploadedFile(int lobbyID, string fileName, byte[] fileContent)
        {
            this.lobbyID = lobbyID;
            this.FileName = fileName;
            this.FileContent = fileContent;
        }
    }
}
