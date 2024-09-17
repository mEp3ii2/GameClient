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
        static void Main(string[] args)
        {
           
            //This should *definitely* be more descriptive.
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
