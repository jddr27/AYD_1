﻿using Carrito_Compras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Carrito_Compras.Models;
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


        public ActionResult Principal2()
        {
            LinkedList<Producto> listado = new LinkedList<Producto>();
          if (!String.IsNullOrEmpty(Request["search"])){
              String busqueda = Request["search"].ToString();
            if(busqueda.Equals("categoria")){
                //Aqui filtro los datos por categoria
        foreach (var obj in Obtener.Productos())
        {
            if(obj.categoria.Equals(Request["search"]).ToString()){

            }
        }
            return View();
                   }

            if (busqueda.Equals("productos"))
            {
                ViewBag.Listado = Obtener.Productos();
                return View();
            }
           }
          ViewBag.Listado = Obtener.Productos();
          return View();
        }

        public ActionResult Principal()
        {
            
            //Envia Listado de Productos
            ViewBag.Listado = Obtener.Productos();
           
            return View();

        }

        public ActionResult Carrito(double precio)
        {
            
            Session["subtotal"] = Convert.ToDouble(Session["subtotal"])+precio;
            ViewBag.actual = Convert.ToDouble(Session["subtotal"]);
            return View();
        }
        public ActionResult Descripcion(int id)
        {
            
            //Recibe/envia id de producto 
            //Envia listado de productos
            ViewBag.prods = Obtener.Productos();
            ViewBag.idProducto = id;
            return View();
        }

        public ActionResult Login()
        {


            return View();
        }


        public ActionResult Logout()
        { //Serramos sesion
            Session.Abandon();
            return RedirectToAction("Principal", "Home");
        }

        [HttpPost]
        public ActionResult Validar()
        {
            string correo = Request["txtcorreo"].ToString();
            string contra = Request["txtcontra"].ToString();
            Usuario usu = new Usuario(correo, contra);
            
            if (usu.resultado.Equals("exito"))
            {   //Se guarda usuario en la session
                Session["UserName"] = usu.nombres;
               // return RedirectToAction("Principal", "Home");
                 return usu.rol == 3 ? RedirectToAction("Principal", "Home"): RedirectToAction("DashBoard", "Home");
            }
            else
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> " + usu.resultado + "<br/>");
                return Content(sbInterest.ToString());
            }
        }

        public ActionResult Todos()
        {
            StringBuilder sbInterest = new StringBuilder();
            sbInterest.Append("<br><b>Resultado:</b>" + ""+ "<br/>");
            sbInterest.Append("<br><b>exito</b><br/>");
            return Content(sbInterest.ToString());
        }


        public ActionResult Boleta()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Tarjeta()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Clientes()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult DashBoard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Inventario()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Registro()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}