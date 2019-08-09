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

                    }

                    var userList = new List<User>();

                    foreach (DataRow row in dt.Rows)
                    {

                        int id = Int32.Parse(row["id"].ToString());
                        String name = row["username"].ToString();
                        String content = row["password"].ToString();
                        int is_active = Int32.Parse(row["is_active"].ToString());
                        String db_path = row["db_path"].ToString();
                        int user_type = Int32.Parse(row["user_type"].ToString());

                        User user = new User();
                        user.id = id;
                        user.username = name;
                        user.password = content;
                        user.is_active = is_active;
                        user.db_path = db_path;
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

                    int user_type = user.user_type;

                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";
                    int last_catalogId;

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Credencial(username,password,is_active,db_path,user_type) values(@username, @password, @is_active, @db_path, @user_type)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.Parameters.AddWithValue("@db_path", user.db_path);
                        cmd.Parameters.AddWithValue("@user_type", user_type);
                        cmd.ExecuteNonQuery();

                        DataTable dt = new DataTable();
                        query = "select top 1 * from Credencial order by id desc";
                        cmd = new SqlCommand(query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        last_catalogId = Int32.Parse(dt.Rows[0]["id"].ToString());


                    }

                    if(user_type == 1)
                    {

                        strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";

                        using (SqlConnection con2 = new SqlConnection(strConString))
                        {

                            con2.Open();

                            String query = "create database " + user.db_path;
                            SqlCommand cmd = new SqlCommand(query, con2);
                            cmd.ExecuteNonQuery();

                        }

                        strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + user.db_path + ";Integrated Security=True";

                        using (SqlConnection con3 = new SqlConnection(strConString))
                        {

                            con3.Open();

                            String query = "CREATE TABLE Entity(name text, content text, creation_date datetime, id int not null identity (1,1))";
                            SqlCommand cmd = new SqlCommand(query, con3);
                            cmd.ExecuteNonQuery();

                            query = "CREATE TABLE Credencial(id INT identity(1,1) PRIMARY KEY, username TEXT, password TEXT, is_active INT, db_path TEXT, user_type INT)";
                            cmd = new SqlCommand(query, con3);
                            cmd.ExecuteNonQuery();

                            query = "insert into Credencial(username,password,is_active,db_path,user_type) values(@username, @password, @is_active, @db_path, @user_type)";
                            cmd = new SqlCommand(query, con3);
                            cmd.Parameters.AddWithValue("@username", user.username);
                            cmd.Parameters.AddWithValue("@password", user.password);
                            cmd.Parameters.AddWithValue("@is_active", user.is_active);
                            cmd.Parameters.AddWithValue("@db_path", user.db_path);
                            cmd.Parameters.AddWithValue("@user_type", user_type);
                            cmd.ExecuteNonQuery();

                        }

                    }

                    if(user_type == 2)
                    {

                        strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog="+user.db_path+";Integrated Security=True";

                        using (SqlConnection con = new SqlConnection(strConString))
                        {

                            con.Open();

                            String query = "insert into Credencial(username,password,is_active,db_path,user_type,id_catalogo) values(@username, @password, @is_active, @db_path, @user_type, @id_catalogo)";
                            SqlCommand cmd = new SqlCommand(query, con);

                            cmd.Parameters.AddWithValue("@username", user.username);
                            cmd.Parameters.AddWithValue("@password", user.password);
                            cmd.Parameters.AddWithValue("@is_active", user.is_active);
                            cmd.Parameters.AddWithValue("@db_path", user.db_path);
                            cmd.Parameters.AddWithValue("@user_type", user_type);
                            cmd.Parameters.AddWithValue("@id_catalogo", last_catalogId);
                            cmd.ExecuteNonQuery();

                        }

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

                        String query = "update Credencial set username = @username, password = @password, is_active = @is_active, db_path = @db_path, user_type = @user_type where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.Parameters.AddWithValue("@db_path", user.db_path);
                        cmd.Parameters.AddWithValue("@user_type", user.user_type);
                        cmd.ExecuteNonQuery();

                    }

                    int user_type = user.user_type;

                    if(user_type == 1 || user_type == 2)
                    {

                        strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" +user.db_path+";Integrated Security=True";

                        using (SqlConnection con = new SqlConnection(strConString))
                        {

                            con.Open();

                            String query = "select id_catalogo from Credencial where id = @id";
                            SqlCommand cmd = new SqlCommand(query, con);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            query = "update Credencial set username = @username, password = @password, is_active = @is_active, db_path = @db_path, user_type = @user_type where id = @id";
                            cmd = new SqlCommand(query, con);

                            cmd.Parameters.AddWithValue("@id", user.id);
                            cmd.Parameters.AddWithValue("@username", user.username);
                            cmd.Parameters.AddWithValue("@password", user.password);
                            cmd.Parameters.AddWithValue("@is_active", user.is_active);
                            cmd.Parameters.AddWithValue("@db_path", user.db_path);
                            cmd.Parameters.AddWithValue("@user_type", user.user_type);
                            cmd.ExecuteNonQuery();

                        }

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

                    }

                    int user_type = user.user_type;

                    if(user_type == 1 || user_type == 2)
                    {

                        strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog="+user.db_path+";Integrated Security=True";

                        using (SqlConnection con = new SqlConnection(strConString))
                        {

                            con.Open();

                            String query = "delete from Credencial where id = @id";
                            SqlCommand cmd = new SqlCommand(query, con);

                            cmd.Parameters.AddWithValue("@id", user.id);
                            cmd.ExecuteNonQuery();

                        }

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
            String db_path = dt.Rows[0]["db_path"].ToString();
            int user_type = Int32.Parse(dt.Rows[0]["user_type"].ToString());


            User user = new User();
            user.id = Int32.Parse(id);
            user.username = username;
            user.password = password;
            user.is_active = is_active;
            user.db_path = db_path;
            user.user_type = user_type;

            return user;

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