using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carrito_Compras.Models
{
    public class Comentario
    {
        public static string resultado { get; set; }
        public int usuario_comentario { get; set; }
        public int producto_comentario { get; set; }
        public string texto_comentario { get; set; }
        public string fecha_comentario { get; set; }
        public int valoracion_comentario { get; set; }    
        public Usuario usuario { get; set; }

        private static bool connection_open;
        private static MySqlConnection connection;

        public Comentario(int id_usuario,int id_producto,string texto,string fecha,int valoracion)
        {
            this.usuario_comentario = id_usuario;
            this.producto_comentario = id_producto;
            this.texto_comentario = texto;
            this.fecha_comentario = fecha;
            this.valoracion_comentario = valoracion;


        }


        public static LinkedList<Comentario> ObtenerReseña()
        {
            int id_usuario;
            int id_producto;
            string texto;
            string fecha;
            int valoracion;
            LinkedList<Comentario> lista = new LinkedList<Comentario>();
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT * FROM Comentarios;;");
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(1) == false)
                            id_usuario = Int32.Parse(reader.GetString(1));
                        else
                            id_usuario = -1;

                        if (reader.IsDBNull(2) == false)
                            id_producto = int.Parse(reader.GetString(2));
                        else
                            id_producto = -1;
                        if (reader.IsDBNull(3) == false)
                            texto = reader.GetString(3);
                        else
                            texto =null;

                        if (reader.IsDBNull(4) == false)
                            fecha = reader.GetString(4);
                        else
                            fecha = null;
                        if (reader.IsDBNull(5) == false)
                            valoracion = int.Parse(reader.GetString(5));
                        else
                            valoracion = -1;



                        

                        lista.AddLast(new Comentario(id_usuario,id_producto,texto,fecha,valoracion));
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

        public static int Verificar_Comentario(int id_user,int id_producto)
        {
            int valor=0;
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("Select VerificarComentario("+id_user+","+id_producto+");");
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) == false)
                            valor = Int32.Parse(reader.GetString(0));
                        else
                            valor = -1;

                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();
                return valor;
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                
            }

            connection.Close();
            return 0;
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
    }
}