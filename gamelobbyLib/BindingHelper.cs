using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    public static class BindingHelper
    {
        public static NetTcpBinding CreateLargeFileBinding()
        {
            return new NetTcpBinding
            {
                MaxReceivedMessageSize = 52428800,  // 50 MB
                MaxBufferSize = 52428800,
                MaxBufferPoolSize = 52428800,
                OpenTimeout = TimeSpan.FromMinutes(5),
                CloseTimeout = TimeSpan.FromMinutes(5),
                SendTimeout = TimeSpan.FromMinutes(5),
                ReceiveTimeout = TimeSpan.FromMinutes(5),
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = 52428800,
                    MaxBytesPerRead = 4096,
                    MaxDepth = 32,
                    MaxStringContentLength = 52428800
                }
            };
        }
    }
}
