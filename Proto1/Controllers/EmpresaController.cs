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
    public class EmpresaController : Controller
    {
        // GET: Empresa
        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    DataTable dt = new DataTable();
                    string strConString = @"Data Source=localhost;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand("select * from Empresas where id > 1 and is_active = 1", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        con.Close();

                    }

                    var empresaList = new List<Empresa>();

                    foreach (DataRow row in dt.Rows)
                    {

                        Empresa empresa = new Empresa();
                        empresa.id = Int32.Parse(row["id"].ToString());
                        empresa.nombre = row["nombre"].ToString();
                        empresa.rut = row["rut"].ToString();
                        empresa.direccion = row["direccion"].ToString();
                        empresa.is_active = Int32.Parse(row["is_active"].ToString());
                        empresaList.Add(empresa);

                    }

                    return View(empresaList);

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
        public ActionResult Create(Empresa empresa)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {


                    int id_empresa;
                    string strConString = @"Data Source=localhost;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Empresas(nombre,rut,direccion,is_active) values(@nombre, @rut, @direccion, @is_active)";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                        cmd.Parameters.AddWithValue("@rut", empresa.rut);
                        cmd.Parameters.AddWithValue("@direccion", empresa.direccion);
                        cmd.Parameters.AddWithValue("@is_active", 1);
                        cmd.ExecuteNonQuery();

                        query = "select id from Empresas where nombre = @nombre and rut = @rut";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                        cmd.Parameters.AddWithValue("@rut", empresa.rut);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        id_empresa = Int32.Parse(dt.Rows[0]["id"].ToString());

                        query = "insert into Rutas(name_db,is_active) values(@name_db, @is_active)";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name_db", empresa.nombre.Replace(" ","_").ToLower() + "_db");
                        cmd.Parameters.AddWithValue("@is_active", 1);
                        cmd.ExecuteNonQuery();

                        int last_id;
                        query = "select max(id) from Rutas";
                        cmd = new SqlCommand(query, con);
                        dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        last_id = Int32.Parse(dt.Rows[0][0].ToString());

                        query = "update Empresas set id_ruta = @id_ruta where id = @id_empresa";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id_ruta", last_id);
                        cmd.Parameters.AddWithValue("@id_empresa", id_empresa);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    String nombre_db;
                    strConString = @"Data Source=localhost;Initial Catalog=master;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        nombre_db = empresa.nombre.Replace(" ", "_").ToLower() + "_db";

                        con.Open();

                        String query = "create database " + nombre_db;
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    strConString = @"Data Source=localhost;Initial Catalog=" + nombre_db+ ";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "CREATE TABLE Entidades(name varchar(50), content varchar(50), creation_date datetime, id int not null identity (1,1))";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        query = "CREATE TABLE Credenciales(id INT PRIMARY KEY, username varchar(50), password varchar(50), rut varchar(50), email varchar(50), is_active INT, user_type INT)";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    int last_id2;
                    strConString = @"Data Source=localhost;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Credenciales(username, password, is_active, user_type, id_empresa) values(@username, @password, @is_active, @user_type, @id_empresa)";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@username", empresa.nombre.Replace(" ", "_").ToLower());
                        cmd.Parameters.AddWithValue("@password", empresa.nombre.Replace(" ", "_").ToLower() + "_123");
                        cmd.Parameters.AddWithValue("@is_active", 1);
                        cmd.Parameters.AddWithValue("@user_type", 1);
                        cmd.Parameters.AddWithValue("@id_empresa", id_empresa);
                        cmd.ExecuteNonQuery();

                        query = "select max(id) from Credenciales";
                        cmd = new SqlCommand(query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        last_id2 = Int32.Parse(dt.Rows[0][0].ToString());

                        con.Close();

                    }

                    strConString = @"Data Source=localhost;Initial Catalog="+nombre_db+";Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query = "insert into Credenciales(id, username, password, rut, email, is_active, user_type) values(@id, @username, @password, @rut, @email, @is_active, @user_type)";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("id", last_id2);
                        cmd.Parameters.AddWithValue("@username", empresa.nombre.Replace(" ", "_").ToLower());
                        cmd.Parameters.AddWithValue("@password", empresa.nombre.Replace(" ", "_").ToLower() + "_123");
                        cmd.Parameters.AddWithValue("@rut", "12345678-9");
                        cmd.Parameters.AddWithValue("@email", "email@email.com");
                        cmd.Parameters.AddWithValue("@is_active", 1);
                        cmd.Parameters.AddWithValue("@user_type", 1);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    return RedirectToAction("Index", "Empresa");

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

                    Empresa empresa = GetEmpresa(id);
                    return View(empresa);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Update(Empresa empresa)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    string strConString = @"Data Source=localhost;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query;
                        SqlCommand cmd;

                        query = "update Empresas set nombre = @nombre, direccion = @direccion, rut = @rut where id = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", empresa.id);
                        cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                        cmd.Parameters.AddWithValue("@direccion", empresa.direccion);
                        cmd.Parameters.AddWithValue("@rut", empresa.rut);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    return RedirectToAction("Index","Empresa");

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

                    Empresa empresa = GetEmpresa(id);
                    return View(empresa);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        [HttpPost]
        public ActionResult Delete(Empresa empresa)
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                if (Int32.Parse(System.Web.HttpContext.Current.Session["user_type"].ToString()) == 0)
                {

                    string strConString = @"Data Source=localhost;Initial Catalog=catalogo;Integrated Security=True";

                    using (SqlConnection con = new SqlConnection(strConString))
                    {

                        con.Open();

                        String query;
                        SqlCommand cmd;

                        query = "update Empresas set is_active = @is_active where id = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", empresa.id);
                        cmd.Parameters.AddWithValue("@is_active", 0);
                        cmd.ExecuteNonQuery();

                        con.Close();

                    }

                    return RedirectToAction("Index", "Empresa");

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

                    Empresa empresa = GetEmpresa(id);
                    return View(empresa);

                }
                else
                {

                    return Content("No tiene permiso para ingresar aquí.");

                }

            }

        }

        private Empresa GetEmpresa(String id)
        {

            DataTable dt = new DataTable();
            string strConString = @"Data Source=localhost;Initial Catalog=catalogo;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(strConString))
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("select * from Empresas where id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                con.Close();

            }

            Empresa empresa = new Empresa();
            empresa.nombre = dt.Rows[0]["nombre"].ToString();
            empresa.direccion = dt.Rows[0]["direccion"].ToString();
            empresa.rut = dt.Rows[0]["rut"].ToString();

            return empresa;

        }

    }
}