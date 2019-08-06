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
        public ActionResult Index(User user)
        {

            DataTable dt = new DataTable();
            SqlCommand cmd;
            SqlDataAdapter da;

            Console.WriteLine("Usuario: " + user.username);
            Console.WriteLine("DB Path: " + user.db_path);

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

                    return RedirectToAction("Index", "Login");

                }
                else
                {

                    String name = user.username;
                    String db_path = dt.Rows[0]["db_path"].ToString();

                    string strConString2 = @"Data Source=.\SQLEXPRESS;Initial Catalog=" +db_path+";Integrated Security=True";
                    Debug.WriteLine(db_path);
                    Debug.WriteLine(strConString2);

                    System.Web.HttpContext.Current.Session["user_name"] = name;
                    System.Web.HttpContext.Current.Session["db_path"] = db_path;

                    Debug.WriteLine(System.Web.HttpContext.Current.Session["user_name"]);
                    Debug.WriteLine(System.Web.HttpContext.Current.Session["db_path"]);

                    return RedirectToAction("Index", "Crud");

                }

            }

        }

        public ActionResult Cerrar()
        {

            Debug.WriteLine("Cerrando sesión");
            System.Web.HttpContext.Current.Session["user_name"] = null;
            System.Web.HttpContext.Current.Session["db_path"] = null;
            return RedirectToAction("Index", "Login");

        }
    }
}