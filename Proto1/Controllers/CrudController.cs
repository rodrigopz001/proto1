using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Proto1.Models;
using System.Diagnostics;

namespace Proto1.Controllers
{
    public class CrudController : Controller
    {

        // GET: Crud
        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
                {

                    String db_path = System.Web.HttpContext.Current.Session["nombre_bd"].ToString();

                    DataTable dt = new DataTable();
                    string strConString = @"Data Source=localhost;Initial Catalog=" + db_path + ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select * from Entidades", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                    }

                    var entityList = new List<Entity>();

                    foreach (DataRow row in dt.Rows)
                    {

                        int id = Int32.Parse(row["id"].ToString());
                        String name = row["name"].ToString();
                        String content = row["content"].ToString();
                        DateTime creation_date = Convert.ToDateTime(row["creation_date"].ToString());

                        Entity entity = new Entity();
                        entity.id = id;
                        entity.name = name;
                        entity.content = content;
                        entity.creation_date = creation_date;

                        entityList.Add(entity);

                    }

                    return View(entityList);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

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

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
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
        public ActionResult Create(Entity entity)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 0 || user_type == 1 || user_type == 2)
                {

                    String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
                    String db_path = System.Web.HttpContext.Current.Session["nombre_bd"].ToString();

                    string strConString = @"Data Source=localhost;Initial Catalog=" + db_path + ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Entidades(name,content,creation_date) values(@name, @content, @creation_date)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@name", entity.name);
                        cmd.Parameters.AddWithValue("@content", entity.content);
                        cmd.Parameters.AddWithValue("@creation_date", DateTime.Now);
                        cmd.ExecuteNonQuery();

                    }

                    return RedirectToAction("Index", "Crud");

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

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
                {

                    Entity entity = GetEntity(id);
                    return View(entity);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Update(Entity entity)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
                {

                    String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
                    String db_path = System.Web.HttpContext.Current.Session["nombre_bd"].ToString();

                    Debug.WriteLine("Update (POST)");

                    string strConString = @"Data Source=localhost;Initial Catalog=" + db_path + ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "update Entidades set name = @name, content = @content, creation_date = @creation_date where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", entity.id);
                        cmd.Parameters.AddWithValue("@name", entity.name);
                        cmd.Parameters.AddWithValue("@content", entity.content);
                        cmd.Parameters.AddWithValue("@creation_date", DateTime.Now);
                        cmd.ExecuteNonQuery();

                    }

                    return RedirectToAction("Index", "Crud");

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

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
                {

                    Entity entity = GetEntity(id);
                    return View(entity);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Delete(Entity entity)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
                {

                    String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
                    String db_path = System.Web.HttpContext.Current.Session["nombre_bd"].ToString();

                    string strConString = @"Data Source=localhost;Initial Catalog=" + db_path + ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "delete from Entidades where id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", entity.id);
                        cmd.ExecuteNonQuery();

                    }

                    return RedirectToAction("Index", "Crud");

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }


        public ActionResult Details(String id)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                int user_type = Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString());

                if (user_type == 1 || user_type == 2)
                {

                    Entity entity = GetEntity(id);
                    return View(entity);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        private Entity GetEntity(String id)
        {

            String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
            String db_path = System.Web.HttpContext.Current.Session["nombre_db"].ToString();

            DataTable dt = new DataTable();
            string strConString = @"Data Source=localhost;Initial Catalog=" +db_path+";Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Entidades where convert(varchar,id)=" + id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            String name = dt.Rows[0]["name"].ToString();
            String content = dt.Rows[0]["content"].ToString();
            DateTime creation_date = Convert.ToDateTime(dt.Rows[0]["creation_date"].ToString());

            Entity entity = new Entity();
            entity.id = Int32.Parse(id);
            entity.name = name;
            entity.content = content;
            entity.creation_date = creation_date;

            return entity;

        }

    }
}