using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carrito_Compras.Models
{
    public static class Editar
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

        public static string TerminarCarrito(int id)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("UPDATE `Carrito` SET `estado_carrito` = 2 WHERE `id_carrito` = " + id + ";");
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    reader.Read();
                    reader.Close();
                    resultado = "exito";
                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
            return "exito";
        }

        public static string CambiarTotal(int id, double nuevo)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("UPDATE `Carrito` SET `total_carrito` = " + nuevo + " WHERE `id_carrito` = " + id + ";");
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    reader.Read();
                    reader.Close();
                    resultado = "exito";
                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
            return "exito";
        }

        public static string RestarInventario(int prod, int canti)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("UPDATE `Producto` SET `cantidad_producto` = " + canti + " WHERE `id_producto` = " + prod + ";");
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    reader.Read();
                    reader.Close();
                    resultado = "exito";
                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    resultado = MessageString;
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                resultado = MessageString;
            }

            connection.Close();
            return "exito";
        }
    }
}