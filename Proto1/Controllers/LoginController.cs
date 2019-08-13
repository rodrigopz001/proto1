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

                cmd = new SqlCommand("select c.* from Credencial c, Rutas r where c.id_ruta = r.id AND (c.username = @username AND c.password = @password)", con);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@password", user.password);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {

                    Debug.WriteLine("No hay nada");
                    return RedirectToAction("Index", "Login");

                }
                else
                {

                    int user_type = Int32.Parse(dt.Rows[0]["user_type"].ToString());

                    String name = user.username;
                    String id_ruta = dt.Rows[0]["id_ruta"].ToString();

                    System.Web.HttpContext.Current.Session["user_name"] = name;
                    System.Web.HttpContext.Current.Session["id_ruta"] = id_ruta;
                    System.Web.HttpContext.Current.Session["user_type"] = user_type;
                    System.Web.HttpContext.Current.Session["is_logged"] = 1;

                    if (user_type == 0)
                    {

                        con.Close();
                        return RedirectToAction("Index", "Admin");

                    }

                    if (user_type == 1)
                    {

                        con.Close();
                        return RedirectToAction("Index", "Subadmin");

                    }

                    if (user_type == 2)
                    {

                        con.Close();
                        return RedirectToAction("Index", "Crud");

                    }

                    return RedirectToAction("Index", "Login");

                }

            }

        }

        public ActionResult Cerrar()
        {

            Debug.WriteLine("Cerrando sesión");
            System.Web.HttpContext.Current.Session["user_name"] = null;
            System.Web.HttpContext.Current.Session["id_ruta"] = null;
            System.Web.HttpContext.Current.Session["user_type"] = null;
            System.Web.HttpContext.Current.Session["is_logged"] = null;
            return RedirectToAction("Index", "Login");

        }

    }
}