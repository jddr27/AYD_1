using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Obtener
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

        public static LinkedList<Producto> Productos()
        {
            int id;
            string nombre;
            int cantidad;
            double precio;
            string descripcion;
            int marca;
            int categoria;
            int promocion;
            string salida = "";
            LinkedList<Producto> lista = new LinkedList<Producto>();
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT `nombre_producto`,`cantidad_producto`,`precio_producto`," +
                    "`descripcion_producto`,`marca_producto`,`categoria_producto`,`promocion_producto`,`id_producto` FROM `Producto`;");
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
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
                            marca = int.Parse(reader.GetString(4));
                        else
                            marca = -1;
                        if (reader.IsDBNull(5) == false)
                            categoria = int.Parse(reader.GetString(5));
                        else
                            categoria = -1;
                        if (reader.IsDBNull(6) == false)
                            promocion = int.Parse(reader.GetString(6));
                        else
                            promocion = -1;
                        if (reader.IsDBNull(7) == false)
                            id = int.Parse(reader.GetString(7));
                        else
                            id = -1;
                        lista.AddLast(new Producto(id, nombre, cantidad, precio, descripcion, marca, categoria, promocion));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();
                foreach(var prod in lista)
                {
                    prod.imagenes = Imagenes(prod.id);
                }
                foreach (var prod in lista)
                {
                    prod.promocion = new Promocion(prod.promocion_id);
                }
                foreach (var prod in lista)
                {
                    prod.marca = new Marca(prod.marca_id);
                }
                foreach (var prod in lista)
                {
                    prod.categoria = new Categoria(prod.categoria_id);
                }
                return lista;
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
            return null;
        }

        public static LinkedList<string> Imagenes(int produ)
        {
            string ruta;
            string salida = "";
            LinkedList<string> lista = new LinkedList<string>();
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT `ruta_img_producto` FROM `Img_Producto` WHERE `prod_img_producto` = " + produ + ";");
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) == false)
                            ruta = reader.GetString(0);
                        else
                            ruta = null;
                        lista.AddLast(ruta);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();
                return lista;
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                lista.AddLast(MessageString);
            }

            connection.Close();
            return lista;
        }
    }
}