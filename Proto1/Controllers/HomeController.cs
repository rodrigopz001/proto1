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

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                return View();

            }

        }

        public ActionResult About()
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                ViewBag.Message = "En construcción.";
                return View();

            }

        }

        public ActionResult Contact()
        {

            if (System.Web.HttpContext.Current.Session["is_logged"] == null)
            {

                return RedirectToAction("Index", "Login");

            }
            else
            {

                ViewBag.Message = "Datos de contacto.";
                return View();

            }
                
        }

    }
}