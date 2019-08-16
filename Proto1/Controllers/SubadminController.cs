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
    public class SubadminController : Controller
    {
        // GET: Subadmin
        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
                {

                    String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

                    DataTable dt = new DataTable();
                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + db_path + ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select * from Credencial", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                    }

                    var userList = new List<User>();

                    foreach (DataRow row in dt.Rows)
                    {

                        int id = Int32.Parse(row["id"].ToString());
                        String username = row["username"].ToString();
                        String password = row["password"].ToString();
                        int is_active = Int32.Parse(row["is_active"].ToString());
                        int user_type = Int32.Parse(row["user_type"].ToString());

                        User user = new User();
                        user.id = id;
                        user.username = username;
                        user.password = password;
                        user.is_active = is_active;
                        user.id_ruta = 1;
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

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
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

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
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

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
                {

                    int id_ruta = Int32.Parse(System.Web.HttpContext.Current.Session["id_ruta"].ToString());
                    String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();
                    int last_id;

                    String strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Credencial(username,password,is_active,id_ruta,user_type) values(@username, @password, @is_active, @id_ruta, @user_type)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.Parameters.AddWithValue("@id_ruta", id_ruta);
                        cmd.Parameters.AddWithValue("@user_type", 2);
                        cmd.ExecuteNonQuery();

                        query = "select max(id) from Credencial";
                        cmd = new SqlCommand(query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        last_id = Int32.Parse(dt.Rows[0][0].ToString());

                    }
           
                    strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog="+db_path+";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Credencial(id, username,password,is_active,user_type,id_ruta) values(@id, @username, @password, @is_active, @user_type, @id_ruta)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", last_id);
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.Parameters.AddWithValue("@id_ruta", id_ruta);
                        cmd.Parameters.AddWithValue("@user_type", 2);
                        cmd.ExecuteNonQuery();

                    }

                    return RedirectToAction("Index", "Subadmin");

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

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
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

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
                {

                    int id_ruta = Int32.Parse(System.Web.HttpContext.Current.Session["id_ruta"].ToString());
                    String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();
                    
                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog="+db_path+";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "update Credencial set username = @username, password = @password, is_active = @is_active where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.ExecuteNonQuery();

                    }

                    strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con2 = new SqlConnection(strConString))
                    {

                        con2.Open();

                        String query = "update Credencial set username = @username, password = @password, is_active = @is_active where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con2);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@is_active", user.is_active);
                        cmd.ExecuteNonQuery();

                    }

                    return RedirectToAction("Index", "Subadmin");

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

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
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

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 1)
                {

                    String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

                    string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog="+db_path+";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "delete from Credencial where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.ExecuteNonQuery();

                    }

                    strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con2 = new SqlConnection(strConString))
                    {

                        con2.Open();

                        String query = "delete from Credencial where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con2);

                        cmd.Parameters.AddWithValue("@id", user.id);
                        cmd.ExecuteNonQuery();

                    }

                    return RedirectToAction("Index", "Subadmin");

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        private User GetUser(String id)
        {

            String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();
            int id_ruta = Int32.Parse(System.Web.HttpContext.Current.Session["id_ruta"].ToString());

            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog="+db_path+";Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Credencial where id = " + id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            String username = dt.Rows[0]["username"].ToString();
            String password = dt.Rows[0]["password"].ToString();
            int is_active = Int32.Parse(dt.Rows[0]["is_active"].ToString());
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

    }
}