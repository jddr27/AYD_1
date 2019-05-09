using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Carrito_Compras.Models
{
    public class Usuario
    {
        
        public int id { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string encriptada { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public string foto { get; set; }        
        public int rol { get; set; } //1=admin, 2=empleado, 3=cliente
        public string rol_name{ get; set; } 
        public string resultado { get; set; }

        private static bool connection_open;
        private static MySqlConnection connection;

        public Usuario(int id, string nombres,string apellidos,string correo,string direccion,int rol,string foto)
        {
            this.id = id;
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.correo = correo;
            this.direccion = direccion;
            this.rol = rol;
            this.foto = foto;
            
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
                    + " direccion_usuario, rol_usuario, foto_usuario FROM Usuario WHERE id_usuario = '{0}'", id);
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
                        encriptada = reader.GetString(2);
                    else
                        encriptada = null;
                    if (reader.IsDBNull(3) == false)
                        correo = reader.GetString(3);
                    else
                        correo = null;
                    if (reader.IsDBNull(4) == false)
                        direccion = reader.GetString(4);
                    else
                        direccion = null;
                    if (reader.IsDBNull(5) == false)
                        rol = int.Parse(reader.GetString(5));
                    else
                        rol = -1;
                    if (reader.IsDBNull(6) == false)
                        foto = reader.GetString(6);
                    else
                        foto = null;
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    //MessageBox.Show(MessageString, "SQL Read Error");
                    reader.Close();
                    nombres = MessageString;
                    apellidos = encriptada = correo = direccion = null;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                nombres = MessageString;
                apellidos = encriptada = correo = direccion = null;
            }

            connection.Close();
        }

        public Usuario(string arg_cor, string arg_pas)
        {
            Get_Connection();
            correo = arg_cor;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT nombres_usuario, apellidos_usuario, password_usuario, id_usuario,"
                    + " direccion_usuario, rol_usuario FROM Usuario WHERE correo_usuario = '{0}'", correo);
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
                        encriptada = reader.GetString(2);
                    else
                        encriptada = null;
                    if (reader.IsDBNull(3) == false)
                        id = int.Parse(reader.GetString(3));
                    else
                        id = -1;
                    if (reader.IsDBNull(4) == false)
                        direccion = reader.GetString(4);
                    else
                        direccion = null;
                    if (reader.IsDBNull(5) == false)
                        rol = int.Parse(reader.GetString(5));
                    else
                        rol = -1;

                    string[] aux = encriptada.Split(',');
                    byte[] arreglo = new byte[aux.Length];
                    for (int i = 0; i < aux.Length; i++)
                    {
                        arreglo[i] = byte.Parse(aux[i]);
                    }
                    Encriptador enc = new Encriptador(arreglo);
                    resultado = !enc.des.Equals(arg_pas) ? "Contraseña inválida" : "exito";
                    reader.Close();

                }
                catch (MySqlException e)
                {
                    resultado = "No se encontró al usuario específicado";
                    reader.Close();
                    nombres = apellidos = encriptada = correo = direccion = null;
                    rol = -1;
                    id = -1;
                }
            }
            catch (MySqlException e)
            {
                resultado = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;
                nombres = apellidos = encriptada = correo = direccion = null;
                rol = -1;
                id = -1;
            }

            connection.Close();
        }

        public Usuario()
        {
        }

        public  LinkedList<Usuario> ObtenerUsuario()
        {
            int id;
            string nombres;
            string apellidos;
            string correo;
            string direccion;
            int rol;
            string foto; 
            LinkedList<Usuario> lista = new LinkedList<Usuario>();
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT * FROM `Usuario`;");
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(2) == false)
                            nombres= reader.GetString(2);
                        else
                            nombres = null;
                        if (reader.IsDBNull(1) == false)
                            correo = reader.GetString(1);
                        else
                            correo = null;
                        if (reader.IsDBNull(3) == false)
                            apellidos = reader.GetString(3);
                        else
                            apellidos = null;
                        if (reader.IsDBNull(4) == false)
                            direccion = reader.GetString(4);
                        else
                            direccion = null;
                        if (reader.IsDBNull(5) == false)
                            rol = Int32.Parse(reader.GetString(5));
                        else
                            rol = -1;
                        if (reader.IsDBNull(7) == false)
                            foto = reader.GetString(7);
                        else
                            foto = null;
                        if (reader.IsDBNull(0) == false)
                            id = int.Parse(reader.GetString(0));
                        else
                            id = -1;
                        
                        lista.AddLast(new Usuario(id,nombres,apellidos,correo,direccion,rol,foto));
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
                
            }

            connection.Close();
            return null;
        }

        public static int EditarCliente(string id, string correo ,string nombres,string apellidos ,string direccion,string rol ,string foto)
        {

            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                string cadena = "call EditarC("+id+",'"+correo+"','"+nombres+"','"+apellidos+"','"+direccion+"',"+rol+",'"+foto+"');";


                cmd.CommandText = string.Format(cadena);
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    Console.WriteLine(reader.ToString());
                    reader.Close();
                    return 1;

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    reader.Close();
                    return 0;
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "*********************** The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;



            }

            connection.Close();
            return 0;
        }

        public static int EliminarUsuario(int id)
        {
            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("call EliminarC(" + id + ");");
                MySqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    reader.Read();
                    Console.WriteLine(reader.ToString());
                    reader.Close();
                    return 1;

                }
                catch (MySqlException e)
                {
                    string MessageString = "***************** Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
                    reader.Close();
                    return 0;
                }
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
