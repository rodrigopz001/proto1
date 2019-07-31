using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proto1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public String Saludo()
        {

            return "<marquee><h1>Hola mundo</h1></marquee>";

        }

        public String Despedida()
        {

            return "<marquee><h1>Chao mundo</h1></marquee>";

        }

        public String Mensaje()
        {

            String mensaje = this.Request.QueryString["id"];
            return "<marquee><h1>El mensaje es: " +mensaje+ "</h1></marquee>";

        }


    }
}