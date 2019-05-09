using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Detalle_Carrito
    {
        public int id { get; set; }
        public int carrito { get; set; }
        public int id_prod { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

        public Detalle_Carrito(int id, int carrito, int id_prod, int cantidad, double precio)
        {
            this.id = id;
            this.carrito = carrito;
            this.id_prod = id_prod;
            this.cantidad = cantidad;
            this.precio = precio;
        }

        public Detalle_Carrito(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT carrito_detalle_carrito, producto_detalle_carrito, cantidad_detalle_carrito, "
                    + " precio_detalle_carrito FROM Detalle_Carrito WHERE id_detalle_carrito = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        carrito = int.Parse(reader.GetString(0));
                    else
                        carrito = -1;
                    if (reader.IsDBNull(1) == false)
                        id_prod = int.Parse(reader.GetString(1));
                    else
                        id_prod = -1;
                    if (reader.IsDBNull(2) == false)
                        cantidad = int.Parse(reader.GetString(2));
                    else
                        cantidad = -1;
                    if (reader.IsDBNull(3) == false)
                        precio = double.Parse(reader.GetString(3));
                    else
                        precio = -1.0;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    //nombres = MessageString;
                    precio = -1.0;
                    cantidad = -1;
                    carrito = id_prod = -1;
                    producto = null;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                //nombres = MessageString;
                precio = -1.0;
                cantidad = -1;
                carrito = id_prod = -1;
                producto = null;
            }

            connection.Close();
            producto = new Producto(id_prod);
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
