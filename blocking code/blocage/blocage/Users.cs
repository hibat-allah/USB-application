using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blocage
{

        public class Users
        {
            // Attributs
            public string Name { get; set; }
            public string Password { get; set; }

            // Constructeur
            public Users(string name, string password)
            {
                Name = name;
                Password = password;
            }
        }
    
}
