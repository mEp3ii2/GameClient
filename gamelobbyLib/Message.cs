using GameLobbyLib;
using System.Collections.Generic;

public class Message
{
    public int id;
    private static int nextID = 1;
    private List<string> messageList; // Keep this as List<string>
    private User[] userList;

    public Message() { }//I dont fully understand why this works but DO NOT REMOVE. It fucks with something to do with serialization

    public Message(User[] userList = null)
    {
        id = nextID++;
        this.messageList = new List<string>();
        this.userList = userList;
    }

    // Add a method to format and store a message with username
    public void AddMessage(string userName, string messageContent)
    {
        string formattedMessage = $"{userName}: {messageContent}"; // Format message with username
        messageList.Add(formattedMessage);
    }

    public List<string> MessageList
    {
        get { return messageList; }
        set { messageList = value; }
    }

    public User[] UserList
    {
        get { return userList; }
        set { userList = value; }
    }
}
