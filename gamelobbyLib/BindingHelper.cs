/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: BindingHelper
Purpose: Provides helper methods to create a pre-configured NetTcpBinding for large file transfers.
Notes: This class is used to set up a `NetTcpBinding` instance with properties tailored to handle large file sizes of up to 50 MB.
*/

using System;
using System.ServiceModel;

namespace GameLobbyLib
{
    public static class BindingHelper
    {
        /**
        Method: CreateLargeFileBinding
        Imports: None
        Exports: NetTcpBinding (configured for large file transfers)
        Notes: Creates and configures a `NetTcpBinding` to support transferring large files up to 50 MB.
        Algorithm: Sets various binding parameters, such as message size limits, timeouts, and reader quotas, to handle large data transfers efficiently.
        */
        public static NetTcpBinding CreateLargeFileBinding()
        {
            // Configure the NetTcpBinding for large file transfers
            return new NetTcpBinding
            {
                MaxReceivedMessageSize = 52428800,  // 50 MB maximum message size
                MaxBufferSize = 52428800,           // 50 MB buffer size
                MaxBufferPoolSize = 52428800,       // 50 MB buffer pool size
                OpenTimeout = TimeSpan.FromMinutes(5),  // 5-minute timeout for opening connections
                CloseTimeout = TimeSpan.FromMinutes(5), // 5-minute timeout for closing connections
                SendTimeout = TimeSpan.FromMinutes(5),  // 5-minute send timeout
                ReceiveTimeout = TimeSpan.FromMinutes(5), // 5-minute receive timeout

                // Reader quotas to handle large XML payloads
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = 52428800,        // Max array length
                    MaxBytesPerRead = 4096,           // Max bytes per read
                    MaxDepth = 32,                    // Max depth for XML
                    MaxStringContentLength = 52428800 // Max string content length
                }
            };
        }
    }
}
