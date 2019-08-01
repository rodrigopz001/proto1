using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Proto1.Models;

//Tutorial en: https://www.c-sharpcorner.com/UploadFile/francissvk/crud-operations-in-Asp-Net-mvc-using-ado-net/

namespace Proto1.Controllers
{
    public class CrudController : Controller
    {
        // GET: Crud
        public ActionResult Index()
        {

            String vista = "<body><h1>";

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

        public String Create()
        {

            return "CRUD: Create";

        }

        public String Update()
        {

            return "CRUD: Update";

        }

        public String Delete()
        {

            return "CRUD: Delete";

        }

    }
}