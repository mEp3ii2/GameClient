﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbyLib
{
    public class User
    {
        public User(string name) { this.name = name; }

        private string name;

        public string Name { get { return name; } set { name = value; } }

        public override string ToString()
        {
            return name;
        }
    }
}
