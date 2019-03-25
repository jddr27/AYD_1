using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Facturacion
    {
        public int id { get; set; }
        public Carrito carrito { get; set; }
        public double total { get; set; }
        public DateTime? fecha { get; set; }
        public Tipo_Pago tipo { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

        public Facturacion()
        {

        }

        public Facturacion(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT carrito_facturacion, total_facturacion, fecha_facturacion, tipo_pago_facturacion,"
                    + " FROM Facturacion WHERE id_facturacion = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        carrito = new Carrito(int.Parse(reader.GetString(0)));
                    else
                        carrito = null;
                    if (reader.IsDBNull(1) == false)
                        total = double.Parse(reader.GetString(1));
                    else
                        total = -1.0;
                    if (reader.IsDBNull(2) == false)
                        fecha = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null);
                    else
                        fecha = null;
                    if (reader.IsDBNull(3) == false)
                        tipo = new Tipo_Pago(int.Parse(reader.GetString(3)));
                    else
                        tipo = null;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    //nombres = MessageString;
                    carrito = null;
                    total = -1.0;
                    fecha = null;
                    tipo = null;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                //nombres = MessageString;
                carrito = null;
                total = -1.0;
                fecha = null;
                tipo = null;
            }

            connection.Close();
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
