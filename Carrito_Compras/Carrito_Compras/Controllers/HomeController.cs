using Carrito_Compras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Carrito_Compras.Models;
using Carrito_Compras.Clase_Reportes;

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

       

       
        public ActionResult detalles(int prod, double precio)
        {
            int carrito = Convert.ToInt32(Session["CarritoId"]);
            Agregar.DetalleCarritoRapido(carrito,prod, precio);
            if(Agregar.resultado.Equals("exito"))
            {
               
                ViewBag.idx = "Se agrego el producto al carrito";
                System.Diagnostics.Debug.WriteLine(Agregar.resultado);
                ViewBag.Listado = Obtener.Productos();
                Session["subtotal"] = Convert.ToDouble(Session["subtotal"]) + precio;
                return View("Principal");
            }
            else{
                //StringBuilder sbInterest = new StringBuilder();
              //  sbInterest.Append("<br><b>Error:</b> " + Agregar.resultado + "<br/>");

                return RedirectToAction("Descripcion", new { id = prod, precio = precio });
               // return Content(sbInterest.ToString());
            }
        }

        [HttpPost]
        public ActionResult Detalle2()
        {
              int prod=  Convert.ToInt32(Request["idProducto"]);
              double precio= Convert.ToDouble(Request["PrecioProducto"]);
              int cantidad = Convert.ToInt32(Request["cantidad"]);
              //System.Diagnostics.Debug.WriteLine("id:"+prod+"precio:"+precio + "cant:"+ cantidad);
              precio = precio * cantidad;

              int carrito = Convert.ToInt32(Session["CarritoId"]);
              Agregar.DetalleCarrito(carrito, prod, cantidad, precio);
              if (Agregar.resultado.Equals("exito"))
              {
                  ViewBag.Listado = Obtener.Productos();
                  Session["subtotal"] = Convert.ToDouble(Session["subtotal"]) + precio;
                  return RedirectToAction("Descripcion", new {id = prod, precio = precio});
              }
              else
              {
                  StringBuilder sbInterest = new StringBuilder();
                  sbInterest.Append("<br><b>Error:</b> " + Agregar.resultado + "<br/>");
                  return Content(sbInterest.ToString());
              }
              
        }

        public ActionResult Principal()
        {
            //Variable Contador de producto encontrados
            int contadorProductos = 0;
            Session["CarritoId"] = 1;
           
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
                
                //ViewBag.prueba = Session["id_user"].ToString();


            }
            return View();

        }

         public ActionResult EliminarDetalle(int id)
        {
            
            Eliminar.Detalle_Carrito(id);
            if (Eliminar.resultado.Equals("exito"))
            {
                
             LinkedList<Detalle_Carrito> detalle = Obtener.Detalles(Convert.ToInt32(Session["CarritoId"]));
             double descontar=0;
             foreach (var obj in detalle)
            {
                    if (obj.id_prod.Equals(id))
                    {
                      descontar=obj.precio;
                    }

                 }

                ViewBag.detalles = Obtener.Detalles(Convert.ToInt32(Session["CarritoId"]));
                ViewBag.prods = Obtener.Productos();
                Session["subtotal"] = Convert.ToDouble(Session["subtotal"])-descontar ;
                return View("Carrito");
            }
            else
            {
                //No pudo eliminar el detalle
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> " + Eliminar.resultado + "<br/>");
                return Content(sbInterest.ToString());
            }
        }


        public ActionResult Carrito()
        {
          
            int user = Convert.ToInt32(Session["id_user"]);
            int carrito = Convert.ToInt32(Session["CarritoId"]);
            Session["subtotal"] = 0;
            Carrito car = new Carrito(carrito);
            if(car.usuario.Equals(user)) {
            ViewBag.compras = 1;
            ViewBag.detalles = Obtener.Detalles(Convert.ToInt32(Session["CarritoId"]));
            ViewBag.prods = Obtener.Productos();

            LinkedList<Detalle_Carrito> detalle = Obtener.Detalles(Convert.ToInt32(Session["CarritoId"]));
            LinkedList<Producto> prods = Obtener.Productos();
            foreach (var obj in detalle)
            {
                foreach (var obj2 in prods)
                {


                    if (obj.id_prod.Equals(obj2.id))
                    {
                        foreach (var img in obj2.imagenes)
                        {
                            System.Diagnostics.Debug.WriteLine("foto:" + img);
                            break;

                        }
                    }

                }

              
                Session["subtotal"] = Convert.ToDouble(Session["subtotal"]) + obj.precio;
                System.Diagnostics.Debug.WriteLine("idproducto:" + obj.id_prod + "precio:" + obj.precio + "total" + Convert.ToDouble(Session["subtotal"]));

            }
            }

            else
            {
                ViewBag.compras =0;
            }



            return View();
        }

        public ActionResult ReporteComentarios() {
            ReporteComentarios reporte = new ReporteComentarios();
            byte[] abytes = reporte.PrepareReport();
            return File(abytes, "application/pdf");
        }


        public ActionResult ReporteClientes()
        {
            ReporteUsuarios reporte = new ReporteUsuarios();
            byte[] abytes = reporte.PrepareReport();
            return File(abytes, "application/pdf");
        }


        public ActionResult ReporteProductos()
        {
            ReporteProductos reporte = new ReporteProductos();
            byte[] abytes = reporte.PrepareReport();
            return File(abytes, "application/pdf");
        }

        public ActionResult Descripcion(int id,double precio)
        {
            LinkedList<Comentario> Reseña = new LinkedList<Comentario>();            

            ViewBag.prods = Obtener.Productos();
            ViewBag.idProducto = id;           

            foreach (var obj in Comentario.ObtenerReseña())
            {
                //Agregamos a la lista
                Reseña.AddLast(obj);

            }
          

            foreach (Comentario obj in Reseña)
            {
                //Agregamos a la lista
                var usu = new Usuario(obj.usuario_comentario);
                
                
                    obj.usuario = usu;                   
             

            }

            ViewBag.Reseña = Reseña;
            ViewBag.idProducto = id;
            ViewBag.precioProducto= precio;

            try {
                ViewBag.idUser = Session["id_user"].ToString();
                ViewBag.precio = Session["precio"].ToString();
                ViewBag.Valor = Comentario.Verificar_Comentario(Int32.Parse(Session["id_user"].ToString()), id);
            }
            catch (System.NullReferenceException e) {
                ViewBag.idUser = null;
            }
            

            return View();
        }

        public ActionResult AgregarReseña()
        {

            string idUser = Request["idUser"].ToString();
            string idProducto = Request["idProducto"].ToString();
            string valoracion = Request["valoracion"].ToString();
            string reseña = Request["reseña"].ToString();

            int resultado = Agregar.AgregarReseña(idUser,idProducto,reseña,valoracion);
            TempData["resultado"] = resultado.ToString();
            TempData["id_producto"] = idProducto.ToString();
            TempData["ambito"] = "AddR";


            return RedirectToAction("Operacion", "Home");

           


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
                Session["id_user"] = usu.id.ToString();
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
                //StringBuilder sbInterest = new StringBuilder();
                //sbInterest.Append("<br><b>Error:</b> " + usu.resultado + "<br/>");
                ViewBag.error = 0;
                return View("Login");
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
            string foto = Request["txtfoto"].ToString();
            if (!contra.Equals(contra2))
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> Las contraseñas no coinciden <br/>");
                return Content(sbInterest.ToString());
            }
            Agregar.Usuario(correo, nombres, apellidos, direccion, 3, contra, foto);
            if (Agregar.resultado.Equals("exito"))
            {
                return View("Login");
            }
            else
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> " + Agregar.resultado + "<br/>");
                return Content(sbInterest.ToString());
            }
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
            return View();
        }

        [HttpPost]
        public ActionResult TarjetaValidar()
        {
            string nombre = Request["txtnombre"].ToString();
            string numero = Request["txtnumero"].ToString();
            string codigo = Request["txtcodigo"].ToString();
            string fecha = Request["txtfecha"].ToString();
            #region VALIDACIONES
            if (codigo.Length == 4)
            {
                foreach(Char c in codigo.ToCharArray())
                {
                    if(Char.IsDigit(c) == false)
                    {
                        StringBuilder sbInterest = new StringBuilder();
                        sbInterest.Append("<br><b>Error:</b> El código de seguridad debe tener solamente dígitos<br/>");
                        return Content(sbInterest.ToString());
                    }
                }
            }
            else
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> El código de seguridad debe tener 4 dígitos<br/>");
                return Content(sbInterest.ToString());
            }
            if (fecha.Length == 5)
            {
                if(Char.IsDigit(fecha.ElementAt(0)) == false){
                    StringBuilder sbInterest = new StringBuilder();
                    sbInterest.Append("<br><b>Error:</b> La fecha solo debe tener 2 dígitos para el mes y 2 dígitos para el año, separados por una diagonal<br/>");
                    return Content(sbInterest.ToString());
                }
                if (Char.IsDigit(fecha.ElementAt(1)) == false)
                {
                    StringBuilder sbInterest = new StringBuilder();
                    sbInterest.Append("<br><b>Error:</b> La fecha solo debe tener 2 dígitos para el mes y 2 dígitos para el año, separados por una diagonal<br/>");
                    return Content(sbInterest.ToString());
                }
                if (fecha.ElementAt(2).Equals('/') == false)
                {
                    StringBuilder sbInterest = new StringBuilder();
                    sbInterest.Append("<br><b>Error:</b> La fecha solo debe tener 2 dígitos para el mes y 2 dígitos para el año, separados por una diagonal<br/>");
                    return Content(sbInterest.ToString());
                }
                if (Char.IsDigit(fecha.ElementAt(3)) == false)
                {
                    StringBuilder sbInterest = new StringBuilder();
                    sbInterest.Append("<br><b>Error:</b> La fecha solo debe tener 2 dígitos para el mes y 2 dígitos para el año, separados por una diagonal<br/>");
                    return Content(sbInterest.ToString());
                }
                if (Char.IsDigit(fecha.ElementAt(4)) == false)
                {
                    StringBuilder sbInterest = new StringBuilder();
                    sbInterest.Append("<br><b>Error:</b> La fecha solo debe tener 2 dígitos para el mes y 2 dígitos para el año, separados por una diagonal<br/>");
                    return Content(sbInterest.ToString());
                }
            }
            else
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> La fecha solo debe tener 2 dígitos para el mes y 2 dígitos para el año, separados por una diagonal<br/>");
                return Content(sbInterest.ToString());
            }
            #endregion
            string limpio = ValidarTarjeta.NormalizeCardNumber(numero);
            if (ValidarTarjeta.IsCardNumberValid(limpio))
            {
                int carrito = Convert.ToInt32(Session["CarritoId"]);
                double total = Convert.ToDouble(Session["total"]);
                // Agregar.Facturacion(carrito,total,1);
                Agregar.resultado = "exito";
                if (Agregar.resultado.Equals("exito"))
                {
                    Editar.TerminarCarrito(carrito);
                    if (Editar.resultado.Equals("exito"))
                    {
                        return RedirectToAction("Principal", "Home");
                    }
                    else
                    {
                        StringBuilder sbInterest = new StringBuilder();
                        sbInterest.Append("<br><b>Error:</b>" + Editar.resultado + "<br/>");
                        return Content(sbInterest.ToString());
                    }
                }
                else
                {
                    StringBuilder sbInterest = new StringBuilder();
                    sbInterest.Append("<br><b>Error:</b>" + Agregar.resultado +  "<br/>");
                    return Content(sbInterest.ToString());
                }
            }
            else
            {
                StringBuilder sbInterest = new StringBuilder();
                sbInterest.Append("<br><b>Error:</b> La tarjeta de creditro no es válida<br/>");
                return Content(sbInterest.ToString());
            }
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

        public ActionResult EditarCliente()
        {
            string id = Request["id"].ToString();
            string nombres = Request["nombres"].ToString();
            string apellidos = Request["apellidos"].ToString();
            string correo = Request["correo"].ToString();
            string direccion = Request["direccion"].ToString();
            string rol = Request["rol"].ToString();
            string url = Request["url"].ToString();
            string estado = Request["estado"].ToString();


            int resultado = Usuario.EditarCliente(id, correo, nombres, apellidos, direccion, rol,url,estado);
            TempData["resultado"] = resultado.ToString();
            TempData["ambito"] = "EditC";


            return RedirectToAction("Operacion", "Home");


        }

        public ActionResult AgregarCliente()
        {
            
            string nombres = Request["nombres"].ToString();
            string apellidos = Request["apellidos"].ToString();
            string correo = Request["correo"].ToString();
            string direccion = Request["direccion"].ToString();
            string password1 = Request["password1"].ToString();
            string password2 = Request["password2"].ToString();
            string rol = Request["rol"].ToString();
            string url = Request["url"].ToString();
            TempData["ambito"] = "AddUser";

            if (!password1.Equals(password2))
            {
                TempData["resultado"] = "3";
                return RedirectToAction("Operacion", "Home");
            }
            Agregar.Usuario(correo, nombres, apellidos, direccion,Int32.Parse(rol),password1,url);
            if (Agregar.resultado.Equals("exito"))
            {
                TempData["resultado"] = "1";
            }
            else
            {
                TempData["resultado"] = "0";
            }
            

            return RedirectToAction("Operacion", "Home");


        }

        public ActionResult DashBoard()
        {

            LinkedList<Comentario> Comentarios = new LinkedList<Comentario>();            
            foreach (var obj in Comentario.ObtenerReseña())
            {                
                Comentarios.AddLast(obj);
            }
            foreach (Comentario obj in Comentarios)
            {
                var usu = new Usuario(obj.usuario_comentario);
                obj.usuario = usu;
            }

            foreach (Comentario obj in Comentarios)
            {
                var producto = new Producto(obj.producto_comentario);
                obj.producto =producto;
            }

            ViewBag.Comentarios = Comentarios;            

            return View();

            
        }

        public ActionResult Operacion()
        {
            ViewBag.Message = TempData["resultado"].ToString();
            ViewBag.Ambito = TempData["ambito"].ToString();
            if (TempData["id_producto"] != null) {
                ViewBag.Id_Producto = TempData["id_producto"].ToString();
                ViewBag.precio = TempData["precio"].ToString();
            }

            

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

        public ActionResult MiCuenta()
        {

            if (Session["id_user"]!=null)
            {
          
            int user = Convert.ToInt32(Session["id_user"]);
            Usuario usuario = new Usuario(user);
            ViewBag.email = usuario.correo;
            ViewBag.nombres = usuario.nombres;
            ViewBag.apellidos = usuario.apellidos;
            ViewBag.direccion = usuario.direccion;
            ViewBag.foto = usuario.foto;
                  }
            else
            {

                return RedirectToAction("Principal", "Home");
            }
            
            return View();
        }


        public ActionResult Marcas()
        {

            LinkedList<Marca> lista = new LinkedList<Marca>();
            foreach (var obj in Marca.ObtenerMarca())
            {
               
                //Agregamos a la lista
                lista.AddLast(obj);
            }
           
            ViewBag.Listado = lista;
            return View("Principal");
        }
        public ActionResult Categorias()
        {
            LinkedList<Categoria> lista = new LinkedList<Categoria>();
            foreach (var obj in Categoria.ObtenerCategoria())
            { //Agregamos a la lista
                lista.AddLast(obj);
            }

            ViewBag.Listado = lista;
            return RedirectToAction("Principal", "Home");
        }

        public ActionResult Promociones()
        {
            LinkedList<Promocion> lista = new LinkedList<Promocion>();
            foreach (var obj in Promocion.ObtenerPromo())
            { //Agregamos a la lista
                lista.AddLast(obj);

            }
            ViewBag.Listado = lista;
            return RedirectToAction("Principal", "Home");
        }

        /*public ActionResult Ofertas()
        {
            LinkedList<Promocion> lista = new LinkedList<Promocion>();
            foreach (var obj in Promocion.ObtenerPromo())
            { //Agregamos a la lista
                lista.AddLast(obj);
            }
            ViewBag.Listado = lista;
            return View();
        }*/
    }
}