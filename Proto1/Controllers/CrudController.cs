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

            if (System.Web.HttpContext.Current.Session["user_name"] == null || System.Web.HttpContext.Current.Session["db_path"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
                String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

                DataTable dt = new DataTable();
                string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + db_path + ";Integrated Security=True";

                using (SqlConnection con = new SqlConnection(strConString))
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select * from Entity", con);
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

        }

        [HttpGet]
        public ActionResult Create()
        {

            if (System.Web.HttpContext.Current.Session["user_name"] == null || System.Web.HttpContext.Current.Session["db_path"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                Debug.WriteLine("Create GET");
                return View();

            }

        }

        [HttpPost]
        public ActionResult Create(Entity entity)
        {

            String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
            String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

            Debug.WriteLine("Create GET");
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" +db_path+ ";Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();

                String query = "insert into Entity(name,content,creation_date) values(@name, @content, @creation_date)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@name", entity.name);
                cmd.Parameters.Add("@content", entity.content);
                cmd.Parameters.Add("@creation_date", DateTime.Now);
                cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index", "Crud");

        }

        [HttpGet]
        public ActionResult Update(String id)
        {

            if (System.Web.HttpContext.Current.Session["user_name"] == null || System.Web.HttpContext.Current.Session["db_path"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                Debug.WriteLine("Update (GET), ID: " + id);
                Entity entity = GetEntity(id);
                return View(entity);


            }

        }

        [HttpPost]
        public ActionResult Update(Entity entity)
        {

            String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
            String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

            Debug.WriteLine("Update (POST)");

            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + db_path + ";Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();

                String query = "update Entity set name = @name, content = @content, creation_date = @creation_date where id = @id";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@id", entity.id);
                cmd.Parameters.Add("@name", entity.name);
                cmd.Parameters.Add("@content", entity.content);
                cmd.Parameters.Add("@creation_date", DateTime.Now);
                cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index", "Crud");

        }

        [HttpGet]
        public ActionResult Delete(String id)
        {

            if (System.Web.HttpContext.Current.Session["user_name"] == null || System.Web.HttpContext.Current.Session["db_path"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                Debug.WriteLine("Delete(GET), ID: " + id);
                Entity entity = GetEntity(id);
                return View(entity);

            }

        }

        [HttpPost]
        public ActionResult Delete(Entity entity)
        {

            String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
            String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

            Debug.WriteLine("Delete(POST), ID: " + entity.id);

            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + db_path + ";Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();

                String query = "delete from Entity where id = @id";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@id", entity.id);
                cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Index", "Crud");

        }


        public ActionResult Details(String id)
        {

            Debug.WriteLine("Details ID: " + id);
            Entity entity = GetEntity(id);

            return View(entity);

        }

        private Entity GetEntity(String id)
        {

            String user_name = System.Web.HttpContext.Current.Session["user_name"].ToString();
            String db_path = System.Web.HttpContext.Current.Session["db_path"].ToString();

            Debug.WriteLine("Get Entity");
            Debug.WriteLine("ID: " + id);

            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" +db_path+";Integrated Security=True";
            Debug.WriteLine(strConString);

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Entity where convert(varchar,id)=" + id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            Debug.WriteLine(id);

            String name = dt.Rows[0]["name"].ToString();
            Debug.WriteLine(name);

            String content = dt.Rows[0]["content"].ToString();
            Debug.WriteLine(content);

            DateTime creation_date = Convert.ToDateTime(dt.Rows[0]["creation_date"].ToString());
            Debug.WriteLine(creation_date);

            Entity entity = new Entity();
            entity.id = Int32.Parse(id);
            entity.name = name;
            entity.content = content;
            entity.creation_date = creation_date;

            return entity;

        }

    }
}