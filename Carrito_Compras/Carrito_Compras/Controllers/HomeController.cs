using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carrito_Compras.Controllers
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

        public ActionResult Principal()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Carrito()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Descripcion()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Boleta()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}