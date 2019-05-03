using Carrito_Compras.Models;
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
        {    //Variable Contador de producto encontrados
             int contadorProductos =0;
            //Lista para almacenar productos encontrados en la bùsqueda
             LinkedList<Producto> list= new LinkedList<Producto>();
      
            //Comparmos si el Requeste No es Nulo(Es decir no se ha buscado ningùn producto)
      if (!String.IsNullOrEmpty(Request["search"])){

          //Recorremos todos los Productos
     foreach (var obj in Obtener.Productos())
        {
            //Filtro por marca de cada producto
            if (obj.marca.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
            {
             //Agregamos a la lista
              list.AddLast(obj);
              contadorProductos++;
              }

          //Filtro por Categoria a la cual pertenece cada producto
            if (obj.categoria.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
            {
                list.AddLast(obj);
                contadorProductos++;
            }

         //Filtro por Promocion que tienen algunos productos
            if (obj.promocion.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
            {
                list.AddLast(obj);
                contadorProductos++;
            }
         //Filtro  por Nombre de productos
            if (obj.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
            {
                list.AddLast(obj);
                contadorProductos++;
            }

        }

     if (contadorProductos == 0)
     {
    //Variables donde guardamos la lista de productos para enviarla a la principal
     ViewBag.contador = contadorProductos;
     ViewBag.Listado = Obtener.Productos();
          }
     else
     {
         //Variables donde guardamos la lista de productos para enviarla a la principal
         
         ViewBag.Listado = list;
     }

 }
          //Sino hemos echo ninguna busqueda mostramos todos los productos
 else  {
        // ViewBag.contador = contadorProductos;
          ViewBag.Listado = Obtener.Productos();
          
 }
 return View();
        }

        public ActionResult Principal()
        {

            //Variable Contador de producto encontrados
            int contadorProductos = 0;
            //Lista para almacenar productos encontrados en la bùsqueda
            LinkedList<Producto> list = new LinkedList<Producto>();

            //Comparmos si el Requeste No es Nulo(Es decir no se ha buscado ningùn producto)
            if (!String.IsNullOrEmpty(Request["search"]))
            {

                //Recorremos todos los Productos
                foreach (var obj in Obtener.Productos())
                {
                    //Filtro por marca de cada producto
                    if (obj.marca.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
                    {
                        //Agregamos a la lista
                        list.AddLast(obj);
                        contadorProductos++;
                    }

                    //Filtro por Categoria a la cual pertenece cada producto
                    if (obj.categoria.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
                    {
                        list.AddLast(obj);
                        contadorProductos++;
                    }

                    //Filtro por Promocion que tienen algunos productos
                    if (obj.promocion.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
                    {
                        list.AddLast(obj);
                        contadorProductos++;
                    }
                    //Filtro  por Nombre de productos
                    if (obj.nombre.Equals(Request["search"], StringComparison.OrdinalIgnoreCase))
                    {
                        list.AddLast(obj);
                        contadorProductos++;
                    }

                }

                if (contadorProductos == 0)
                {
                    //Variables donde guardamos la lista de productos para enviarla a la principal
                    ViewBag.contador = contadorProductos;
                    ViewBag.Listado = Obtener.Productos();
                }
                else
                {
                    //Variables donde guardamos la lista de productos para enviarla a la principal

                    ViewBag.Listado = list;
                }

            }
            //Sino hemos echo ninguna busqueda mostramos todos los productos
            else
            {
                // ViewBag.contador = contadorProductos;
                ViewBag.Listado = Obtener.Productos();

            }
            return View();

        }

        public ActionResult Carrito(double precio)
        {
            /*Recibimos el valor(precio del prudcto que se esta comprando)
             * Cremos una variable de Session para el manejo del subtotal conforme se compran 
            productos, hasta que el usuario deje de comprar y salga de su cuenta.
             * 
             * Convertimos el objeto Session a Double e incrementamos el valor actual 
             * de los productos que se van agregando
             * */
            Session["subtotal"] = Convert.ToDouble(Session["subtotal"])+precio;
            ViewBag.actual = Convert.ToDouble(Session["subtotal"]);
            return View();
        }
        public ActionResult Descripcion(int id)
        {
            
            //Recibe/envia id de producto 
            //Envia listado de productos
            //Recibimos el id del producto para mostrar la descripcion enviamos a la Vista Descripcion
            ViewBag.prods = Obtener.Productos();
            ViewBag.idProducto = id;
            return View();
        }

        public ActionResult Login()
        {


            return View();
        }


        public ActionResult Logout()
        { //Serramos sesion y volvemos a la Vista Principal
            Session.Abandon();
            return RedirectToAction("Principal", "Home");
        }

        [HttpPost]
        public ActionResult Validar()
        {
            string correo = Request["txtcorreo"].ToString();
            string contra = Request["txtcontra"].ToString();
            Usuario usu = new Usuario(correo, contra);
            //Si existe el Usuario nos devuelve una mensaje "exito"
            if (usu.resultado.Equals("exito"))
            {   //Se guarda usuario en la session
                Session["UserName"] = usu.nombres;
               // Manejo de Roles 
                /*1.Administrador softech (Estadisticas, reportes...)
                * 2. Empleado (Gestiona  productos, promociones,...) 
                * 3. Cliente (Cliente registrado que puede comprar, ver promociones, ...)
                * */
                return usu.rol == 3 ? RedirectToAction("Principal", "Home"): RedirectToAction("DashBoard", "Home");
            }
            else
            {
                 //Sino Existe el  Usuario Se Muestra un  Mensaje
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