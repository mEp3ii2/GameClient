/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: User
Purpose: Represents a user in the game lobby system. Each user has a unique name that serves as their identifier.
Notes: The class overrides Equals for user comparison based on the name property. It also includes a ToString method to return the user's name as a string.
*/

using System;

namespace GameLobbyLib
{
    public class User
    {
        // Private field for the user's name
        private string name;

        /**
        Constructor: User()
        Purpose: Default constructor. Initializes a new instance of the User class without setting the name.
        */
        public User() { }

        /**
        Constructor: User(string name)
        Purpose: Initializes a new instance of the User class with the provided name.
        */
        public User(string name)
        {
            this.name = name;
        }

        /**
        Method: Equals(object obj)
        Purpose: Overrides the Equals method to compare User objects by their name.
        Algorithm: The method checks if the obj parameter is of type User, then compares the name property.
        */
        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user == null) return false;
            return name == user.name;
        }

        // Property to get or set the user's name
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /**
        Method: ToString()
        Purpose: Overrides the ToString method to return the user's name as a string.
        */
        public override string ToString()
        {
            return name;
        }
    }
}
