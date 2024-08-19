# Mortal Kombat X Online Gaming Lobby

This project is an implementation of an online gaming lobby management system for Mortal Kombat X using .NET and WCF (Windows Communication Foundation). The lobby allows multiple players to join, create rooms, communicate in real-time, and share files within the game lobby environment.

## Project Structure

This project consists of two main components:

1. **Gaming Lobby Server**: Manages player logins, room management, message distribution, and file sharing between players within the same lobby room.
2. **Client Application (Player)**: A WPF (Windows Presentation Foundation) application that allows players to log in, join or create lobby rooms, chat with other players, send private messages, and share files.

## Features

### Server Features
- **User Management**: Ensures unique player logins (no duplicate usernames).
- **Lobby Room Management**: Handles creation, joining, and leaving of lobby rooms.
- **Message Distribution**: Distributes messages sent by players to all others within the same lobby room.
- **Private Messaging**: Supports private messages between players within the same room.
- **File Sharing**: Allows players to share image and text files within a lobby room.

### Client Features
- **User Login**: Prompts the player to log in with a unique name.
- **Lobby Room Selection**: Displays a list of available rooms to join.
- **Lobby Room Creation**: Allows players to create new rooms.
- **Lobby Room Messaging**: Enables real-time chat within a room.
- **Private Messaging**: Allows sending and receiving private messages.
- **File Sharing**: Facilitates the sharing of files (images, documents) within a room.
- **Logout**: Allows the player to log out of the application.

## Getting Started

### Prerequisites
- .NET Framework
- WPF for the client application

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/mEp3ii2/GameClient
   ```
2. Open the solution in Visual Studio.
3. Build the solution to restore any missing dependencies.

### Running the Application
1. **Server**: Start the Gaming Lobby Server project to initiate the server.
2. **Client**: Start the Client Application project. Multiple instances can be launched to simulate different players.

### Testing
- The system should be tested with 1 to 5 clients to ensure proper functionality.
- The server uses a static IP:Port combination, which must be known to all clients.

## Contribution
- Lachlan, Mitch, Ryan

## License
This project is for educational purposes as part of the COMP3008 course at Curtin University.
