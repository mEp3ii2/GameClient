/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: BusinessTierServer
Purpose: The entry point for the business tier server, responsible for hosting the business layer service and handling client-server communication.
Notes: This class sets up and starts the WCF service for handling business logic requests from clients.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GameLobbyLib;

namespace BusinessLayer
{
    internal class BusinessTierServer
    {
        /**
        Method: Main
        Imports: string[] args (Command line arguments)
        Exports: None
        Notes: This is the entry point of the business tier server. It initializes and starts the WCF service.
        Algorithm: Sets up the NetTcpBinding, initializes the service host, and binds the business server implementation to the endpoint.
        */
        static void Main(string[] args)
        {
            // This should *definitely* be more descriptive.
            Console.WriteLine("Business Server starting...");

            // Use the BindingHelper to get the binding configuration
            NetTcpBinding tcp = BindingHelper.CreateLargeFileBinding();

            // Set up the host and bind the BusinessServerImplementation to the endpoint
            ServiceHost host = new ServiceHost(typeof(BusinessServerImplementation));
            host.AddServiceEndpoint(typeof(IBusinessServerInterface), tcp, "net.tcp://0.0.0.0:8100/GameService");

            // Start the host
            host.Open();
            Console.WriteLine("Business Server Online");
            Console.ReadLine();

            // Close the host after usage
            host.Close();
        }
    }
}
