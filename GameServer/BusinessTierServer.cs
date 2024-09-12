using System;
using System.ServiceModel;

namespace BusinessLayer
{
    internal class BusinessTierServer
    {
        static void Main(string[] args)
        {
            //This should *definitely* be more descriptive.
            Console.WriteLine("Business Server starting...");

            //This is the actual host service system
            ServiceHost host;

            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();

            // Set the message and buffer size limits for larger file transfers

            tcp.MaxReceivedMessageSize = 52428800;  // 50 MB
            tcp.MaxBufferSize = 52428800;
            tcp.MaxBufferPoolSize = 52428800;

            // Set reader quotas for large messages
            tcp.ReaderQuotas.MaxArrayLength = 52428800;
            tcp.ReaderQuotas.MaxStringContentLength = 52428800;
            tcp.ReaderQuotas.MaxBytesPerRead = 8192;
            tcp.ReaderQuotas.MaxDepth = 64;
            // Adjust if you're dealing with deeply nested XML structures

            // Set timeouts to handle long-running operations
            tcp.OpenTimeout = TimeSpan.FromMinutes(1);
            tcp.CloseTimeout = TimeSpan.FromMinutes(1);
            tcp.ReceiveTimeout = TimeSpan.FromMinutes(10); // For long-running connections
            tcp.SendTimeout = TimeSpan.FromMinutes(10);    // For large files

            // Bind the server to the implementation of BusinessServer
            host = new ServiceHost(typeof(BusinessServerImplementation));

            // Present the publicly accessible interface to the client. 0.0.0.0 tells .net to accept on any interface. :8100 means this will use port 8100. 
            host.AddServiceEndpoint(typeof(IBusinessServerInterface), tcp, "net.tcp://0.0.0.0:8100/GameService");

            // Open the host for business!
            host.Open();
            Console.WriteLine("Business Server Online");
            Console.ReadLine();

            // Don't forget to close the host after you're done!
            host.Close();
        }
    }
}
