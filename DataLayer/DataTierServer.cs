using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal class DataTierServer
    {
        static void Main(string[] args)
        {

            //This should *definitely* be more descriptive.
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
