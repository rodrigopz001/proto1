using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Proto1.Models;
using System.Diagnostics;

//Tutorial en: https://www.c-sharpcorner.com/UploadFile/francissvk/crud-operations-in-Asp-Net-mvc-using-ado-net/

namespace Proto1.Controllers
{
    public class CrudController : Controller
    {
        // GET: Crud
        public ActionResult Index()
        {

            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cliente1;Integrated Security=True";

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

                String id = row["id"].ToString();
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

        public ActionResult Create(Entity entity)
        {

            Debug.WriteLine("Create");
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cliente1;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                String id = entity.id;
                String name = entity.name;
                String content = entity.content;
                DateTime creation_date = entity.creation_date;

                con.Open();
                SqlCommand cmd = new SqlCommand("insert cliente1(id,name,content,creation_date) values(@id, @name, @content, @creation_date");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.InsertCommand = cmd;

            }

            return View();

        }

        public ActionResult Update(String id)
        {

            Debug.WriteLine("Update ID: " + id);
            Entity entity = GetEntity(id);

            return View(entity);

        }

        public ActionResult Delete(String id)
        {

            Debug.WriteLine("Delete ID: " + id);
            Entity entity = GetEntity(id);

            return View(entity);

        }

        public ActionResult Details(String id)
        {

            Debug.WriteLine("Details ID: " + id);
            Entity entity = GetEntity(id);

            return View(entity);

        }

        private Entity GetEntity(String id)
        {

            DataTable dt = new DataTable();
            string strConString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cliente1;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Entity where convert(varchar,id)=" + id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            String name = dt.Rows[0][1].ToString();
            String content = dt.Rows[0][2].ToString();
            DateTime creation_date = Convert.ToDateTime(dt.Rows[0][3].ToString());

            Entity entity = new Entity();
            entity.id = id;
            entity.name = name;
            entity.content = content;
            entity.creation_date = creation_date;

            return entity;

        }

    }
}