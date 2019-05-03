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
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
        public Promocion promocion { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

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
                    nombre = MessageString;
                    cantidad = -1;
                    precio = -1.0;
                    descripcion = null;
                    marca_id = categoria_id =  promocion_id = -1;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                nombre = MessageString;
                cantidad = -1;
                precio = -1.0;
                descripcion = null;
                marca_id = categoria_id = promocion_id = -1;
            }

            connection.Close();
            promocion = new Promocion(promocion_id);
            marca = new Marca(marca_id);
            categoria = new Categoria(categoria_id);
        }

        private void Get_Connection()
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

        private bool Open_Local_Connection()
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
