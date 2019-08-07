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

        public int is_active { get; set; } 

        public String db_path { get; set; }

        public int user_type { get; set; }


    }
}