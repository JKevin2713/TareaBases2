using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static XML;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System;
using System.Reflection;

namespace tareaBases2.Pages.Project.Account
{
    public class LoginModel : PageModel
    {
        // Instancia de la clase para cargar XML
        public XML xmlLoad = new XML();
        public usuario infoUsuario = new usuario();
        public insertarBitacora insertar = new insertarBitacora();
        public int id;
        public int intentos;
        public bool bandera = false;
        public string message = "";
        public string tiempo = "";

        public void OnGet()
        {
            xmlLoad.Cargar();
        }

        public void OnPost()
        {
            string auxUsername = Request.Form["username"];
            string auxPassword = Request.Form["password"];
            int resultCode = 0;

            if (auxUsername == "" || auxPassword == "")
            {
                message = "Error, ingrese datos";
                Console.WriteLine(message);
                return;
            }

            // Asignar los valores validados al objeto infoUsuario
            infoUsuario.Username = auxUsername;
            infoUsuario.Password = auxPassword;

            try
            {
                // Cadena de conexión a la base de datos
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2" +
                                          ";Integrated Security=True;Encrypt=False";

                // Establecer una conexión con la base de datos utilizando la cadena de conexión
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open(); // Abrir la conexión

                    // Crear un comando SQL para llamar al stored procedure "inicioSesion"
                    using (SqlCommand command = new SqlCommand("inicioSesion", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        command.Parameters.AddWithValue("@username", infoUsuario.Username);
                        command.Parameters.AddWithValue("@password", infoUsuario.Password);

                        // Parámetro de salida
                        command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Intentos", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                        id = Convert.ToInt32(command.Parameters["@Id"].Value);
                        intentos = Convert.ToInt32(command.Parameters["@Intentos"].Value);
                    }

                    string tipoEvento = "";
                    string mensaje = "";

                    if(intentos <= 5)
                    {
                        // Evaluar el resultado del procedimiento almacenado
                        if (resultCode == 50006)
                        {
                            tipoEvento = "Login No Exitoso";
                            message = "Error, la contraseña ingresada no coincide";
                            mensaje = "Error, la contraseña ingresada no coincide." +
                                " Numero de intentos = " + intentos;
                        }
                        else if (resultCode == 50007)
                        {
                            tipoEvento = "Login No Exitoso";
                            message = "Error, usuario no existe";
                            mensaje = "Error, usuario no existe" +
                                " Numero de intentos = " + intentos;
                        }
                        else if (resultCode == 1)
                        {
                            bandera = true;
                            tipoEvento = "Login Exitoso";
                            message = "Inicio de sesión exitoso";
                            mensaje = "Inicio de sesión exitoso" +
                                " Numero de intentos = " + intentos;
                        }

                        Console.WriteLine(intentos);
                        insertar.insertarBitacoraEventos(sqlConnection, mensaje, tipoEvento, "1");
                        // Cerrar la conexión después de haber terminado de trabajar con ella

                    }
                    else
                    {
                        Console.WriteLine(intentos);
                        Environment.Exit(0);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir e imprimir el mensaje de error
                message = ex.Message;
                return;
            }
        }
    }
}

