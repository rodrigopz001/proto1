using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proto1.Models
{
    public class Empresa
    {

        public int id { get; set; }

        public String nombre { get; set; }

        public String rut { get; set; }

        public String direccion { get; set; }

        public int is_active { get; set; }

        public int ruta_bd { get; set; }

    }
}