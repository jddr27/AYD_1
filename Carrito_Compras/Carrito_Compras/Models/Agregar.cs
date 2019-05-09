using System;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Carrito_Compras.Models
{
    public static class Agregar
    {
        public static string resultado { get; set; }

        private static bool connection_open;
        private static MySqlConnection connection;

        private static void Get_Connection()
        {
            connection_open = false;
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            if (Open_Local_Connection())
            {
                connection_open = true;
            }
            else
            {
                //					MessageBox::Show("No database connection connection made...\n Exiting now", "Database Connection Error");
                //					 Application::Exit();
            }
        }

        private static bool Open_Local_Connection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void Usuario(string correo, string nombres, string apellido, string direccion, int rol, string password, string foto)
        {
            Encriptador enc = new Encriptador(password);
            string bob = enc.enc;
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("INSERT INTO `Usuario` (`correo_usuario`,`nombres_usuario`,`apellidos_usuario`," +
                    "`direccion_usuario`,`rol_usuario`,`password_usuario`,`foto_usuario`) VALUES (\"" + correo + "\",\"" + nombres + "\"," +
                    "\"" + apellido + "\",\"" + direccion + "\"," + rol + ",\"" + bob + "\",\"" + foto + "\");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    resultado = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
        }

        public static void Producto(string nombre, int cantidad, double precio, string descripcion, int marca, int categoria, int promocion)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("INSERT INTO `Producto` (`nombre_producto`,`cantidad_producto`,`precio_producto`," +
                    "`descripcion_producto`,`marca_producto`,`categoria_producto`,`promocion_producto`) VALUES (\"" + nombre + "\"," + cantidad + "," +
                    "" + precio + ",\"" + descripcion + "\"," + marca + "," + categoria + "," + promocion + ");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    resultado = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
        }

        public static int ProductoConImagen(string nombre, int cantidad, double precio, string descripcion, int marca, int categoria, int promocion,string img1,string img2, string img3)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("Select InsertarProductos('"+nombre+"',"+cantidad+","+precio+",'"+descripcion+"',"+marca+","+categoria+","+promocion+",'"+img1+"','"+img2+"','"+img3+"');");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    resultado = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();
                    return 1;

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;

                   
                    reader.Close();
                    return 0;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
                  
              
            }

            connection.Close();
            return 0;
        }

        public static void Promocion(string nombre, string descripcion, DateTime inicio, DateTime fin, double descuento)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("INSERT INTO `Promocion` (`nombre_promocion`,`descripcion_promocion`,`inicio_promocion`," +
                    "`fin_promocion`,`descuento_promocion`) VALUES (\"" + nombre + "\",\"" + descripcion + "\",'" + inicio + "','" + fin + 
                    "'," + descuento + ");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    resultado = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
        }

        public static void Carrito(int usuario)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("INSERT INTO `Carrito` (`usuario_carrito`,`total_carrito`,`estado_carrito`)" +
                    " VALUES (" + usuario + ", 0.00, 1);");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    resultado = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
        }

        public static void DetalleCarritoRapido(int carrito, int producto, double precio)
        {
            resultado = null;
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("INSERT INTO `Detalle_Carrito` (`carrito_detalle_carrito`,`producto_detalle_carrito`,"
                    + "`cantidad_detalle_carrito`,`precio_detalle_carrito`) VALUES (" + carrito + "," + producto + ",1," + precio + ");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    reader.Close();
                    resultado = "exito: " + cmd.CommandText;
                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
        }
        public static int AgregarReseña(string idUser,string idProducto,string reseña,string valoracion)
        {

            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                string cadena = "call EscribirR("+idUser+","+idProducto+",'"+reseña+"',"+valoracion+");";


                cmd.CommandText = string.Format(cadena);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    Console.WriteLine(reader.ToString());
                    reader.Close();
                    return 1;

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    reader.Close();
                    return 0;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;



            }

            connection.Close();
            return 0;
        }
    }
}