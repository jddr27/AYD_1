using System;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Carrito_Compras.Models
{
    public static class Agregar
    {
        public static string salida { get; set; }

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
                cmd.CommandText = string.Format("INSERT INTO `Ana1`.`Usuario` (`correo_usuario`,`nombres_usuario`,`apellidos_usuario`," +
                    "`direccion_usuario`,`rol_usuario`,`password_usuario`,`foto_usuario`) VALUES (\"" + correo + "\",\"" + nombres + "\"," +
                    "\"" + apellido + "\",\"" + direccion + "\"," + rol + ",\"" + bob + "\",\"" + foto + "\");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    salida = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    salida = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                salida = MessageString;
            }

            connection.Close();
        }

        public static void Producto(string correo, string nombres, string apellido, string direccion, int rol, string password, string foto)
        {
            Encriptador enc = new Encriptador(password);
            string bob = enc.enc;
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("INSERT INTO `Ana1`.`Usuario` (`correo_usuario`,`nombres_usuario`,`apellidos_usuario`," +
                    "`direccion_usuario`,`rol_usuario`,`password_usuario`,`foto_usuario`) VALUES (\"" + correo + "\",\"" + nombres + "\"," +
                    "\"" + apellido + "\",\"" + direccion + "\"," + rol + ",\"" + bob + "\",\"" + foto + "\");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    salida = "exito";
                    Console.WriteLine(reader.ToString());
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    salida = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                salida = MessageString;
            }

            connection.Close();
        }
    }
}