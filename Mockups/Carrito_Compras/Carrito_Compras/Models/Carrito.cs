using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Carrito
    {
        public int id { get; set; }
        public Usuario usuario { get; set; }
        public double total { get; set; }
        public Estado estado { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

        public Carrito()
        {

        }

        public Carrito(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT usuario_carrito, total_carrito, estado_carrito FROM Carrito WHERE id_carrito = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        usuario = new Usuario(int.Parse(reader.GetString(0)));
                    else
                        usuario = null;
                    if (reader.IsDBNull(1) == false)
                        total = double.Parse(reader.GetString(1));
                    else
                        total = -1.0;
                    if (reader.IsDBNull(2) == false)
                        estado = new Estado(int.Parse(reader.GetString(2)));
                    else
                        estado = null;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    //nombres = MessageString;
                    total = -1;
                    usuario = null;
                    estado = null;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                //nombres = MessageString;
                total = -1;
                usuario = null;
                estado = null;
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
