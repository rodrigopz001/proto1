using Proto1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proto1.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult Index()
        {
            if(System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    DataTable dt = new DataTable();
                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select * from Credencial where user_type > 0", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        con.Close();

                    }

                    var userList = new List<User>();

                    foreach (DataRow row in dt.Rows)
                    {

                        int id = Int32.Parse(row["id"].ToString());
                        String name = row["username"].ToString();
                        String content = row["password"].ToString();
                        int is_active = Int32.Parse(row["is_active"].ToString());
                        String db_path = row["id_ruta"].ToString();
                        int user_type = Int32.Parse(row["user_type"].ToString());

                        User user = new User();
                        user.id = id;
                        user.username = name;
                        user.password = content;
                        user.is_active = is_active;
                        user.user_type = user_type;

                        userList.Add(user);

                    }

                    return View(userList);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpGet]
        public ActionResult Details(String id)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    User user = GetUser(id);
                    return View(user);


                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí");

                }

            }

        }

        [HttpGet]
        public ActionResult Create()
        {

            if(System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    return View();

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Create(User user)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {


                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Credencial(username,password,is_active,user_type) values(@username, @password, @is_active, @user_type)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.Parameters.AddWithValue("@user_type", 1);
                        cmd.ExecuteNonQuery();

                        query = "insert into Rutas(name_db, is_active) values(@name_db, @is_active)";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name_db", user.username + "_db");
                        cmd.Parameters.AddWithValue("@is_active", 1);
                        cmd.ExecuteNonQuery();

                        query = "select max(id) from Rutas";
                        cmd = new SqlCommand(query, con);
                        SqlDataAdapter ca = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        ca.Fill(dt);

                        int last_id = Int32.Parse(dt.Rows[0][0].ToString());
                        query = "update Credencial set id_ruta = @id_ruta where username = @username";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id_ruta", last_id);
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "create database " + user.username + "_db";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + user.username + "_db" + ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "CREATE TABLE Entity(name varchar(50), content varchar(50), creation_date datetime, id int not null identity (1,1))";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        query = "CREATE TABLE Credencial(id INT identity(1,1) PRIMARY KEY, username varchar(50), password varchar(50), is_active INT, id_ruta int, user_type INT)";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    return RedirectToAction("Index", "Admin");

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpGet]
        public ActionResult Update(String id)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    User user = GetUser(id);
                    return View(user);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Update(User user)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        SqlCommand cmd;
                        SqlDataAdapter da;
                        DataTable dt;

                        cmd = new SqlCommand("select username, id_ruta from Credencial where id = @id", con);
                        cmd.Parameters.AddWithValue("@id", user.id);
                        da = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        da.Fill(dt);

                        String username = dt.Rows[0]["username"].ToString();
                        int id_ruta = Int32.Parse(dt.Rows[0]["id_ruta"].ToString());

                        String query = "update Credencial set username = @username, password = @password, is_active = @is_active where id = @id";
                        cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.ExecuteNonQuery();

                        query = "update Rutas set name_db = @username where id = @id_ruta";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@username", user.username + "_db");
                        cmd.Parameters.AddWithValue("@id_ruta", id_ruta);
                        cmd.ExecuteNonQuery();

                        //Mejorar la seguridad.
                        query = "sp_rename " +username+ "_db , " +user.username+ "_db , 'DATABASE'";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();                      

                        con.Close();

                    }

                    return RedirectToAction("Index", "Admin");

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpGet]
        public ActionResult Delete(String id)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    User user = GetUser(id);
                    return View(user);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Delete(User user)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "delete from Credencial where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.ExecuteNonQuery();

                        //query = "delete r1 from Rutas r1 WHERE r1.id = (SELECT r2.id FROM Credencial c, Rutas r2 WHERE(c.id_ruta = r2.id) AND(c.user_type > 0) AND(c.id = @id))";
                        //cmd = new SqlCommand(query, con);
                        //cmd.Parameters.AddWithValue("@id", user.id);
                        //cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    return RedirectToAction("Index", "Admin");

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        private User GetUser(String id)
        {

            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Credencial where convert(varchar,id)=" + id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            String username = dt.Rows[0]["username"].ToString();
            String password = dt.Rows[0]["password"].ToString();
            int is_active = Int32.Parse(dt.Rows[0]["is_active"].ToString());
            int id_ruta = Int32.Parse(dt.Rows[0]["id_ruta"].ToString());
            int user_type = Int32.Parse(dt.Rows[0]["user_type"].ToString());

            User user = new User();
            user.id = Int32.Parse(id);
            user.username = username;
            user.password = password;
            user.is_active = is_active;
            user.id_ruta = id_ruta;
            user.user_type = user_type;

            return user;

        }

        [HttpGet]
        public JsonResult UserExists(string name)
        {
            
            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Credencial where username = @name", con);
                cmd.Parameters.AddWithValue("@name", name);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

            }

            if (dt.Rows.Count == 0)
            {

                return Json(new { success = true, responseText = "Nombre disponible para registrar." }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                return Json(new { success = false, responseText = "Nombre ya ocupado." }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpGet]
        public List<SelectListItem> getDbPaths()
        {

            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

            List<SelectListItem> listItems = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Credencial order by db_path, con");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            foreach (DataRow row in dt.Rows)
            {

                listItems.Add(new SelectListItem
                {
                    Text = row["db_path"].ToString(),
                    Value = row["id"].ToString(),
                });

            }

            return null;
        }

    }
}