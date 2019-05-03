using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Promocion
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public DateTime? inicio { get; set; }
        public DateTime? fin { get; set; }
        public double descuento { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

        public Promocion(int id, string nombre, string descripcion, DateTime inicio, DateTime fin, double descuento)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.inicio = inicio;
            this.fin = fin;
            this.descuento = descuento;
        }

        public Promocion(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT nombre_promocion, descripcion_promocion, inicio_promocion, fin_promocion,"
                    + " descuento_promocion, FROM Promocion WHERE id_promocion = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        nombre = reader.GetString(0);
                    else
                        nombre = null;
                    if (reader.IsDBNull(1) == false)
                        descripcion = reader.GetString(1);
                    else
                        descripcion = null;
                    if (reader.IsDBNull(2) == false)
                        inicio = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null);
                    else
                        inicio = null;
                    if (reader.IsDBNull(3) == false)
                        fin = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd", null);
                    else
                        fin = null;
                    if (reader.IsDBNull(4) == false)
                        descuento = double.Parse(reader.GetString(4));
                    else
                        descuento = -1;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    nombre = MessageString;
                    descripcion = null;
                    inicio = fin = null;
                    descuento = -1;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                nombre = MessageString;
                descripcion = null;
                inicio = fin = null;
                descuento = -1;
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
