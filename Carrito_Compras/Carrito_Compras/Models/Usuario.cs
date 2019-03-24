using MySql.Data.MySqlClient;
using System;
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
                cmd.CommandText = string.Format("select nombre_rol from Rol where id_rol = '{0}'", RolID);

                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();

                    if (reader.IsDBNull(0) == false)
                        Nombre = reader.GetString(0);
                    else
                        Nombre = null;

                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    Nombre = MessageString;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                Nombre = MessageString;
            }




            connection.Close();


        }

        private void Get_Connection()
        {
            connection_open = false;

            connection = new MySqlConnection();
            //connection = DB_Connect.Make_Connnection(ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString);
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;

            //            if (db_manage_connnection.DB_Connect.OpenTheConnection(connection))
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
