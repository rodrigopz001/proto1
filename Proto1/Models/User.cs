using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proto1.Models
{
    public class User
    {

        public int id { get; set; }

        public String username { get; set; }

        public String password { get; set; }

        public String rut { get; set; }

        public String email { get; set; }

        public int is_active { get; set; } 

        public int user_type { get; set; }

        public int id_ruta { get; set; }


    }
}