/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: DataTierServer
Purpose: Entry point for the Data Server that hosts the WCF service for managing the data layer of the gaming lobby. This class is responsible for configuring and starting the server using the correct bindings.
Notes: This server uses the NetTcpBinding to enable communication between the clients and the data server for large file transfers and lobby management.
*/

using GameLobbyLib;
using System;
using System.ServiceModel;

namespace DataLayer
{
    internal class DataTierServer
    {
        /**
        Method: Main
        Imports: string[] args (Command-line arguments, if any)
        Exports: None
        Notes: The entry point of the Data Server. This method sets up the WCF service, configures the binding, and starts the server to handle client requests.
        Algorithm: 
        1. Create the binding configuration using BindingHelper.
        2. Set up and configure the service host.
        3. Open the host and wait for incoming requests.
        4. Close the host when the application is terminated.
        */
        static void Main(string[] args)
        {
            // Log message to indicate the server is starting
            Console.WriteLine("Data Server starting...");

            // Use the BindingHelper to get the binding configuration
            NetTcpBinding tcp = BindingHelper.CreateLargeFileBinding();

            // Set up the host and bind the DataServerImplementation to the endpoint
            ServiceHost host = new ServiceHost(typeof(DataServerImplementation));
            host.AddServiceEndpoint(typeof(IDataServerInterface), tcp, "net.tcp://0.0.0.0:8200/DataService");

            // Start the host
            host.Open();
            Console.WriteLine("Data Server Online");
            Console.ReadLine();

            // Close the host after usage
            host.Close();
        }
    }
}
