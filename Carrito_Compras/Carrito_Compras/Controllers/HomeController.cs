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

        int Resultado = 1;

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

        //seguir comprando

        public ActionResult carrito2(double precio,int idProducto)
        {
            Session["subtotal"] = Convert.ToDouble(Session["subtotal"]) + precio;
            ViewBag.actual = Convert.ToDouble(Session["subtotal"]);
            ViewBag.idprod = idProducto;
            ViewBag.prods = Obtener.Productos();
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

        public ActionResult Carrito(double precio,int idProducto)
        {
            /*Recibimos el valor(precio del prudcto que se esta comprando)
             * Cremos una variable de Session para el manejo del subtotal conforme se compran 
            productos, hasta que el usuario deje de comprar y salga de su cuenta.
             * 
             * Convertimos el objeto Session a Double e incrementamos el valor actual 
             * de los productos que se van agregando
             * */
            Session["subtotal"] = Convert.ToDouble(Session["subtotal"]) + precio;
            ViewBag.actual = Convert.ToDouble(Session["subtotal"]);
            ViewBag.idprod = idProducto;
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
        { //Cerramos sesion y volvemos a la Vista Principal
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
                return usu.rol == 3 ? RedirectToAction("Principal", "Home") : RedirectToAction("DashBoard", "Home");
            }
            else
            {
                //Sino Existe el  Usuario Se Muestra un  Mensaje
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> " + usu.resultado + "<br/>");
                return Content(sbInterest.ToString());
            }
        }

        [HttpPost]
        public ActionResult ValidarRegistrar()
        {
            string correo = Request["txtcorreo"].ToString();
            string nombres = Request["txtnombres"].ToString();
            string apellidos = Request["txtapellidos"].ToString();
            string direccion = Request["txtdireccion"].ToString();
            string contra = Request["txtcontra"].ToString();
            string contra2 = Request["txtcontra2"].ToString();
            if (!contra.Equals(contra2))
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> Las contraseñas no coinciden <br/>");
                return Content(sbInterest.ToString());
            }
            //Agregar.Usuario(correo, nombres, apellidos, direccion, 3, contra, "");
            //if (Agregar.resultado.Equals("exito"))
            //{
                return View("Login");
            /*}
            else
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> " + Agregar.resultado + "<br/>");
                return Content(sbInterest.ToString());
            }*/
        }

        public ActionResult Todos()
        {
            StringBuilder sbInterest = new StringBuilder();
            sbInterest.Append("<br><b>Resultado:</b>" + "" + "<br/>");
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


            LinkedList<Usuario> usuarios = new LinkedList<Usuario>();

            Usuario l = new Usuario();

            foreach (var obj in l.ObtenerUsuario())
            {
                //Agregamos a la lista
                usuarios.AddLast(obj);

            }

            ViewBag.Cliente= usuarios;
            return View();
        }

        public ActionResult EliminarCliente(int id)
        {

            int Eliminar = Usuario.EliminarUsuario(id);
            TempData["resultado"] = Eliminar.ToString();
            TempData["ambito"] = "deleteC";

            return RedirectToAction("Operacion", "Home");

        }

        public ActionResult DashBoard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Operacion()
        {
            ViewBag.Message = TempData["resultado"].ToString();
            ViewBag.Ambito = TempData["ambito"].ToString();

            return View();
        }

        public ActionResult Inventario()
        {
            LinkedList<Producto> productos = new LinkedList<Producto>();

            foreach (var obj in Obtener.Productos())
            {
                //Agregamos a la lista
                productos.AddLast(obj);

            }

            ViewBag.Producto = productos;
            return View();
        }

        public ActionResult EditarProducto()
        {

            string id = Request["id"].ToString();
            string nombre = Request["name"].ToString();
            string cantidad = Request["cantidad"].ToString();
            string descripcion = Request["des"].ToString();
            string precio = Request["precio"].ToString();
            string img1 = Request["img1"].ToString();
            string img2 = Request["img2"].ToString();
            string img3 = Request["img3"].ToString();
            string idI1 = Request["id1"].ToString();
            string idI2 = Request["id2"].ToString();
            string idI3 = Request["id3"].ToString();
            string categoria = Request["categoria"].ToString();
            string marca = Request["marca"].ToString();
            string promocion = Request["promo"].ToString();


            int resultado = Producto.EditarProducto(Int32.Parse(id), nombre, Int32.Parse(cantidad), Convert.ToDouble(precio), descripcion, Int32.Parse(marca), Int32.Parse(categoria), Int32.Parse(promocion), img1, img2, img3, Int32.Parse(idI1), Int32.Parse(idI2), Int32.Parse(idI3));

            TempData["resultado"] = resultado.ToString();
            TempData["ambito"] = "EditP";



            /*  StringBuilder sbInterest = new StringBuilder();
              sbInterest.Append("<br><b>Error:</b> " +id + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + nombre + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + cantidad + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + descripcion + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + precio + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + img1 + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + idI1 + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + img2 + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + idI2 + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + img3 + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + idI3 + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + categoria+ "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + marca + "<br/>");
              sbInterest.Append("<br><b>Error:</b> " + promocion + "<br/>");
              return Content(sbInterest.ToString());*/

            return RedirectToAction("Operacion", "Home");

        }

        public ActionResult EliminarProducto(int id)
        {

            int Eliminar = Producto.EliminarProducto(id);
            TempData["resultado"] = Eliminar.ToString();
            TempData["ambito"] = "deleteP";

            return RedirectToAction("Operacion", "Home");

        }

        public ActionResult Registro()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Carga()
        {
            //ViewBag.Message ="ejemplo aiyda";
            LinkedList<Marca> marca = new LinkedList<Marca>();
            LinkedList<Categoria> categoria = new LinkedList<Categoria>();
            LinkedList<Promocion> promocion = new LinkedList<Promocion>();
            foreach (var obj in Marca.ObtenerMarca())
            {
                //Agregamos a la lista
                marca.AddLast(obj);
            }
            foreach (var obj in Categoria.ObtenerCategoria())
            { //Agregamos a la lista
                categoria.AddLast(obj);
            }

            foreach (var obj in Promocion.ObtenerPromo())
            { //Agregamos a la lista
                promocion.AddLast(obj);
            }
            ViewBag.Marca = marca;
            ViewBag.Categoria = categoria;
            ViewBag.Promocion = promocion;


            return View();
        }

        [HttpPost]
        public ActionResult InsertProducto()
        {
            string nombre = Request["name"].ToString();
            string cantidad = Request["cantidad"].ToString();
            string descripcion = Request["des"].ToString();
            string precio = Request["precio"].ToString();
            string img1 = Request["img1"].ToString();
            string img2 = Request["img2"].ToString();
            string img3 = Request["img3"].ToString();
            string categoria = Request["categoria"].ToString();
            string marca = Request["marca"].ToString();
            string promocion = Request["promo"].ToString();

            int resultado = Agregar.ProductoConImagen(nombre, Int32.Parse(cantidad), Convert.ToDouble(precio), descripcion, Int32.Parse(marca), Int32.Parse(categoria), Int32.Parse(promocion), img1, img2, img3);
            TempData["resultado"] = resultado.ToString();
            TempData["ambito"] = "InsertP";
            return RedirectToAction("Operacion", "Home");
        }

        public ActionResult EdicionProducto(int id)
        {
            LinkedList<Producto> productos = new LinkedList<Producto>();
            LinkedList<Marca> marca = new LinkedList<Marca>();
            LinkedList<Categoria> categoria = new LinkedList<Categoria>();
            LinkedList<Promocion> promocion = new LinkedList<Promocion>();

            foreach (var obj in Obtener.Productos())
            {
                if (obj.id == id) {
                    productos.AddLast(obj);
                }
                
            }

            
            foreach (var obj in Marca.ObtenerMarca())
            {
                //Agregamos a la lista
                marca.AddLast(obj);
            }
            foreach (var obj in Categoria.ObtenerCategoria())
            { //Agregamos a la lista
                categoria.AddLast(obj);
            }

            foreach (var obj in Promocion.ObtenerPromo())
            { //Agregamos a la lista
                promocion.AddLast(obj);
            }
            ViewBag.Marca = marca;
            ViewBag.Categoria = categoria;
            ViewBag.Promocion = promocion;
            ViewBag.Edicion = productos;
            ViewBag.Id = id;

            return View();


            

        }
    }


}