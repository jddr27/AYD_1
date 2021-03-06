﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Categoria
    {
        public static string resultado { get; set; }
        public int id { get; set; }
        public string nombre { get; set; }

        private static bool connection_open;
        private static MySqlConnection connection;

        public Categoria(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;

        }
        public Categoria()
        {

        }

        public static LinkedList<Categoria> ObtenerCategoria()
        {
            int id;
            string nombre;
            LinkedList<Categoria> lista = new LinkedList<Categoria>();
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT `id_categoria`,`nombre_categoria` FROM `Categoria`;");
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(1) == false)
                            nombre = reader.GetString(1);
                        else
                            nombre = null;

                        if (reader.IsDBNull(0) == false)
                            id = int.Parse(reader.GetString(0));
                        else
                            id = -1;
                        lista.AddLast(new Categoria(id, nombre));
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
                resultado = MessageString;
            }

            connection.Close();
            return null;
        }

        public Categoria(int arg_id)
        {
            Get_Connection();
            id = arg_id;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT nombre_categoria FROM Categoria WHERE id_categoria = '{0}'", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    if (reader.IsDBNull(0) == false)
                        nombre = reader.GetString(0);
                    else
                        nombre = null;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    nombre = MessageString;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                nombre = MessageString;
            }

            connection.Close();
        }

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

        private static  bool Open_Local_Connection()
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
