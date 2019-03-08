using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tasques.Utils
{
    public class CurrentUser
    {
        public CurrentUser(string name, string password, string surnames, int rol, string token)
        {
            this.name = name;
            this.password = password;
            this.surnames = surnames;
            this.rol = rol;
            this.token = token;
        }

        public string name { get; set; }
        public string password { get; set; }
        public string surnames { get; set; }
        public int rol { get; set; }
        public string token { get; set; }
    }
}