﻿using MySql.Data.MySqlClient;
using System;
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
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
        public Promocion promocion { get; set; }

        private bool connection_open;
        private MySqlConnection connection;

        public Producto()
        {

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
                        marca = new Marca(int.Parse(reader.GetString(4)));
                    else
                        marca = null;
                    if (reader.IsDBNull(5) == false)
                        categoria = new Categoria(int.Parse(reader.GetString(5)));
                    else
                        categoria = null;
                    if (reader.IsDBNull(6) == false)
                        promocion = new Promocion(int.Parse(reader.GetString(6)));
                    else
                        promocion = null;
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
                    marca = null;
                    categoria = null;
                    promocion = null;
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
                marca = null;
                categoria = null;
                promocion = null;
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
