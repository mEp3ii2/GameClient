using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    public class UploadedFile
    {
        public int lobbyID { get;}
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
