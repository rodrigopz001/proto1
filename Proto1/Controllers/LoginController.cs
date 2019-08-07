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

            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();

                cmd = new SqlCommand("select * from Credencial where convert(varchar,username) = @username and convert(varchar,password) = @password", con);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@password", user.password);

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
                    int user_type = Int32.Parse(dt.Rows[0]["user_type"].ToString());

                    string strConString2 = @"Data Source=.\SQLEXPRESS;Initial Catalog=" +db_path+";Integrated Security=True";

                    System.Web.HttpContext.Current.Session["user_name"] = name;
                    System.Web.HttpContext.Current.Session["db_path"] = db_path;
                    System.Web.HttpContext.Current.Session["user_type"] = user_type;
                    System.Web.HttpContext.Current.Session["is_logged"] = 1;

                    if (user_type == 0)
                    {

                        return RedirectToAction("Index", "Admin");

                    }
                    else
                    {

                        return RedirectToAction("Index", "Crud");

                    }     

                }

            }

        }

        public ActionResult Cerrar()
        {

            Debug.WriteLine("Cerrando sesión");
            System.Web.HttpContext.Current.Session["user_name"] = null;
            System.Web.HttpContext.Current.Session["db_path"] = null;
            System.Web.HttpContext.Current.Session["user_type"] = null;
            System.Web.HttpContext.Current.Session["is_logged"] = null;
            return RedirectToAction("Index", "Login");

        }

    }
}