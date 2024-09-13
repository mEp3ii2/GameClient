using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    public class FileStreamCustom : FileStream
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string lobbyName { get; private set; }


        public FileStreamCustom(string path, FileMode mode, string lobby, string name)
            : base(path, mode)
        {
            FilePath = path;
            lobbyName = lobby;
            FileName = name;
        }

        public FileStreamCustom(string path, FileMode mode, FileAccess access, string lobby, string name)
            : base(path, mode, access)
        {
            FilePath = path;
            lobbyName = lobby;
            FileName = name;
        }

    }
}
