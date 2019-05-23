using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string password { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public Rol rol { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

        public Usuario()
        {

        }

        public Usuario(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT nombres_usuario, apellidos_usuario, password_usuario, correo_usuario," 
                    + " direccion_usuario, rol_usuario FROM Usuario WHERE id_usuario = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        nombres = reader.GetString(0);
                    else
                        nombres = null;
                    if (reader.IsDBNull(1) == false)
                        apellidos = reader.GetString(1);
                    else
                        apellidos = null;
                    if (reader.IsDBNull(2) == false)
                        password = reader.GetString(2);
                    else
                        password = null;
                    if (reader.IsDBNull(3) == false)
                        correo = reader.GetString(3);
                    else
                        correo = null;
                    if (reader.IsDBNull(4) == false)
                        direccion = reader.GetString(4);
                    else
                        direccion = null;
                    if (reader.IsDBNull(5) == false)
                        rol = new Rol(int.Parse(reader.GetString(5)));
                    else
                        rol = null;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    nombres = MessageString;
                    apellidos = password = correo = direccion = null;
                    rol = null;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                nombres = MessageString;
                apellidos = password = correo = direccion = null;
                rol = null;
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
