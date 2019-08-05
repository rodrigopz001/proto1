using Proto1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Proto1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {

            return View();

        }

        [HttpPost]
        public String Index(User user)
        {

            DataTable dt = new DataTable();
            SqlCommand cmd;
            SqlDataAdapter da;

            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();

                cmd = new SqlCommand("select * from Credencial where convert(varchar,username) = @username and convert(varchar,password) = @password", con);
                cmd.Parameters.Add("@username", user.username);
                cmd.Parameters.Add("@password", user.password);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if(dt.Rows.Count == 0)
                {

                    return "El cliente no existe o la contraseña es errónea.";

                }
                else
                {

                    String name = user.username;
                    String db_path = dt.Rows[0]["db_path"].ToString();

                    string strConString2 = @"Data Source=.\SQLEXPRESS;Initial Catalog=" +db_path+";Integrated Security=True";
                    Debug.WriteLine(db_path);
                    Debug.WriteLine(strConString2);
                    
                    using (SqlConnection con2 = new SqlConnection(strConString2))
                    {

                        dt = new DataTable();

                        con2.Open();
                        cmd = new SqlCommand("select * from Entity");

                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        String html = "";
                        html += "<p>Bienvenido " + name + "<p>";
                        html += "<p>Tus entidades son:</p>";
                        html += "<br>";
                        html += "<table>";
                        html += "<tr>";
                        html += "<th>ID</th>";
                        html += "<th>Nombre</th>";
                        html += "<th>Descripción</th>";
                        html += "<th>Fecha creación</th>";
                        html += "</tr>";

                        foreach (DataRow row in dt.Rows)
                        {

                            int e_id = Int32.Parse(row["id"].ToString());
                            String e_name = row["name"].ToString();
                            String e_content = row["content"].ToString();
                            DateTime e_creation_date = Convert.ToDateTime(row["creation_date"].ToString());

                            html += "<tr>";
                            html += "<td>" + e_id + "</td>";
                            html += "<td>" + e_name + "</td>";
                            html += "<td>" + e_content + "</td>";
                            html += "<td>" + e_creation_date + "</td>";
                            html += "</tr>";

                        }

                        html += "</table>";

                        return html;

                    }

                }

            }



        }
    }
}