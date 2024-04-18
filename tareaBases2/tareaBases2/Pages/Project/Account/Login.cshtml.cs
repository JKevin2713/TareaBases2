using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static XML;
using System.Data.SqlClient;
using System.Data;

namespace tareaBases2.Pages.Project.Account
{
    public class LoginModel : PageModel
    {
        // Instancia de la clase para cargar XML
        public XML xmlLoad = new XML();
        public usuario infoUsuario = new usuario();
        public int id;
        public bool bandera = false;
        public string message = "";
        public insertarBitacora insertar = new insertarBitacora();

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

                // Cadena de conexi�n a la base de datos
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2" +
                                          ";Integrated Security=True;Encrypt=False";

                // Establecer una conexi�n con la base de datos utilizando la cadena de conexi�n
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    // Especificar que el comando es un procedimiento almacenado
                    sqlConnection.Open(); // Abrir la conexi�n

                    // Crear un comando SQL para llamar al stored procedure "registroEmpleado"
                    using (SqlCommand command = new SqlCommand("inicioSesion", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Par�metros de entraada
                        command.Parameters.AddWithValue("@username", infoUsuario.Username);
                        command.Parameters.AddWithValue("@password", infoUsuario.Password);

                        // Par�metro de salida
                        command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();

                        // Obtener el valor del par�metro de salida
                        resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                        id = Convert.ToInt32(command.Parameters["@Id"].Value);
                        // Asignar un valor a la propiedad Id
                        Console.WriteLine("C�digo de resultado: " + resultCode);

                    }

                    string tipoEvento = "";
                    string mensaje = "";
                    // Evaluar el resultado del procedimiento almacenado
                    if (resultCode == 50006)
                    {
                        tipoEvento = "Login No Exitoso";
                        mensaje = "Error, la contrase�a ingresado no incide";
                    }
                    else if (resultCode == 50007)
                    {
                        tipoEvento = "Login No Exitoso";
                        mensaje = "Error, usuario no existe";

                    }
                    else if (resultCode == 1)
                    {
                        bandera = true;
                        tipoEvento = "Login Exitoso";
                        mensaje = "Inicia de sesion exitosa";
                    }

                    insertar.insertarBitacoraEventos(sqlConnection, mensaje, tipoEvento, "1");
                    // Cerrar la conexi�n despu�s de haber terminado de trabajar con ella
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepci�n que pueda ocurrir e imprimir el mensaje de error
                message = ex.Message;
                return;
            }

            Console.WriteLine();
        }
    }
}

