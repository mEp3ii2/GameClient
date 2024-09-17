# Mortal Kombat X Online Gaming Lobby

This project is an implementation of an online gaming lobby management system for **Mortal Kombat X** using **.NET** and **Windows Communication Foundation (WCF)**. The project is designed to simulate an online lobby where multiple players can join or create rooms, chat in real-time, send private messages, and share files within their chosen lobby room.

## Project Overview

This system is built to meet the requirements of the **Distributed Computing (COMP3008) Assignment 1 (Part A)**. The assignment focuses on building a functional lobby management system that allows interaction between multiple players through a WPF-based client interface and a WCF server backend.

### Functional Specification Breakdown

#### Server Features (Gaming Lobby Server):
1. **User Management**: Ensures unique player logins by checking for duplicate usernames. [2 Marks]
2. **Lobby Room Management**: Handles creation, joining, and leaving of lobby rooms, each room accommodating multiple players. [2 Marks]
3. **Message Distribution**: Ensures messages sent by players are distributed to all players within the same lobby room. [2 Marks]
4. **Private Messaging**: Allows players to send private messages to other players in the same room. [2 Marks]
5. **File Sharing**: Facilitates the sharing of image and text files between players in the same room. [3 Marks]

#### Client Features (WPF Client Application):
1. **User Login**: Prompts the user to log in with a unique username and ensures no duplicate logins. [2 Marks]
2. **Lobby Room Selection**: Displays a list of available rooms for players to join. [2 Marks]
3. **Lobby Room Creation**: Allows users to create a new room and see notifications of room creation. [2 Marks]
4. **Lobby Room Messaging**: Enables real-time chat within the selected room. [2 Marks]
5. **Private Messaging**: Allows players to send private messages to other players within the same room. [2 Marks]
6. **File Sharing**: Facilitates the sharing of image and document files, displayed as clickable links in the chat. [2 Marks]
7. **Logout**: Players can log out of the client application. [1 Mark]
8. **Pull Strategy with Refresh**: Allows GUI updates through refresh buttons to keep user lists, messages, and other details up to date. [3 Marks]

### Additional Features:
1. **Multi-Threading**: Uses separate threads to update the list of lobby rooms and users, as well as receive messages and file shares to simulate real-time interactions. [5 Marks]

## Project Structure

This solution consists of three main projects:

1. **Gaming Lobby Server (BusinessLayer)**: Manages player logins, lobby room operations, message distribution, private messaging, and file sharing between players within the same lobby.
   
2. **Data Server (DataLayer)**: Stores and manages data related to players, lobbies, messages, and files. It serves as the backend data store accessed by the business layer.
   
3. **Client Application (GameClient)**: A **WPF** client application that allows players to log in, create or join lobbies, chat, send private messages, and share files.

### Key Features of the Codebase

#### Server Features:
- **Concurrency Management**: Uses WCF's `ConcurrencyMode.Multiple` to handle concurrent requests.
- **File Sharing**: Files (images and documents) are shared within the lobby rooms and are downloadable by users in the same lobby.
- **Message Management**: Supports both public and private messaging within the lobby rooms.

#### Client Features:
- **Real-Time Chat**: Messages in the lobby are displayed as they are sent, either in public or private chats.
- **File Sharing**: Files shared in the lobby are displayed as links, allowing users to download and view them.
- **Threaded Updates**: The client application uses threads to listen for updates (new messages, files, and users) without requiring continuous user interaction.

## Getting Started

### Prerequisites

To build and run this project, you'll need:
- **Visual Studio 2019 or later**
- **.NET Framework 4.7.2 or later**
- **WPF (Windows Presentation Foundation) libraries**

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/mEp3ii2/GameClient
   ```
2. Open the solution in **Visual Studio**.
3. Configure the solution to use multiple startup projects:
   - **DataLayer** (Data Server)
   - **BusinessLayer** (Gaming Lobby Server)
   - **GameClient** (Client Application)

4. Build the solution to restore any missing dependencies.

### Running the Application

1. **Start the Data Server**: Run the **DataLayer** project to initialize the database and make it available for the business server.
2. **Start the Gaming Lobby Server**: Run the **BusinessLayer** project to manage player logins, lobbies, messages, and file sharing.
3. **Start the Client Application**: Run the **GameClient** project to launch the WPF-based client. Multiple instances can be launched to simulate multiple players.

### Testing

- Test the system by launching 1 to 5 clients to ensure proper functionality.
- The server uses a **static IP and port combination** (`net.tcp://localhost:8100/GameService` for the gaming lobby server, and `net.tcp://localhost:8200/DataService` for the data server), which must be the same for all clients.

## File Sharing

The system supports file sharing within each lobby. Shared files (images and text documents) are displayed as clickable links in the chat. When clicked, the client downloads and opens the file.

## Contribution

This project was developed by:
- Lachlan Kipling
- Mitch Pontague
- Ryan Mackintosh

## License

This project is developed for educational purposes as part of the **COMP3008 Distributed Computing** course at Curtin University.
