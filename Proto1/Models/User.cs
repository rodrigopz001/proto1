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

        public bool is_Active { get; set; } 

        public String db_path { get; set; }


    }
}