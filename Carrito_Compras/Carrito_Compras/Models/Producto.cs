using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public string descripcion { get; set; }
        public int marca_id { get; set; }
        public int categoria_id { get; set; }
        public int promocion_id { get; set; }
        public LinkedList<string> imagenes { get; set; }
        public LinkedList<string> Idimagenes { get; set; }
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
        public Promocion promocion { get; set; }

        private static bool connection_open;
        private static MySqlConnection connection;

        public Producto()
        {

        }

        public Producto(int id, string nombre, int cantidad, double precio, string descripcion, int marca_id, int categoria_id, int promocion_id)
        {
            this.id = id;
            this.nombre = nombre;
            this.cantidad = cantidad;
            this.precio = precio;
            this.descripcion = descripcion;
            this.marca_id = marca_id;
            this.categoria_id = categoria_id;
            this.promocion_id = promocion_id;
        }

        public Producto(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT nombre_producto, cantidad_producto, precio_producto, descripcion_producto,"
                    + " marca_producto, categoria_producto, promocion_producto FROM Producto WHERE id_producto = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        nombre = reader.GetString(0);
                    else
                        nombre = null;
                    if (reader.IsDBNull(1) == false)
                        cantidad = int.Parse(reader.GetString(1));
                    else
                        cantidad = -1;
                    if (reader.IsDBNull(2) == false)
                        precio = double.Parse(reader.GetString(2));
                    else
                        precio = -1.0;
                    if (reader.IsDBNull(3) == false)
                        descripcion = reader.GetString(3);
                    else
                        descripcion = null;
                    if (reader.IsDBNull(4) == false)
                        marca_id = int.Parse(reader.GetString(4));
                    else
                        marca_id = -1;
                    if (reader.IsDBNull(5) == false)
                        categoria_id = int.Parse(reader.GetString(5));
                    else
                        categoria_id = -1;
                    if (reader.IsDBNull(6) == false)
                        promocion_id = int.Parse(reader.GetString(6));
                    else
                        promocion_id = -1;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    id = -1;
                    nombre = MessageString;
                    cantidad = -1;
                    precio = -1.0;
                    descripcion = null;
                    marca_id = categoria_id =  promocion_id = -1;
                    connection.Close();
                    return;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                id = -1;
                nombre = MessageString;
                cantidad = -1;
                precio = -1.0;
                descripcion = null;
                marca_id = categoria_id = promocion_id = -1;
                connection.Close();
                return;
            }

            connection.Close();
            promocion = new Promocion(promocion_id);
            marca = new Marca(marca_id);
            categoria = new Categoria(categoria_id);
        }

        public static int EditarProducto(int idP,string nombre, int cantidad, double precio, string descripcion, int marca, int categoria, int promocion, string img1, string img2, string img3,int idI1,int idI2,int idI3)
        {
                                                                                                    
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                string cadena = "call EditarP(" + idP + ",'" + nombre + "'," + cantidad + "," + precio + ",'" + descripcion + "'," + marca + "," + categoria + "," + promocion + ",'" + img1 + "','" + img2 + "','" + img3 + "'," + idI1 + "," + idI2 + "," + idI3 + ");";
                

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

        public static int EliminarProducto(int id)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("call EliminarP("+id+");");
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
    }
}
