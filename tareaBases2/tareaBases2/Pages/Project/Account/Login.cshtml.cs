using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static XML;
using System.Data.SqlClient;
using System.Data;

namespace tareaBases2.Pages.Project.Account
{
    public class LoginModel : PageModel
    {

        public usuario infoUsuario = new usuario();
        public bool bandera = false;
        public string message = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            string auxUsername = Request.Form["username"];
            string auxPassword = Request.Form["password"];
            int resultCode = 0;  

            if(auxUsername == "" || auxPassword == "")
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
                        command.ExecuteNonQuery();

                        // Obtener el valor del par�metro de salida
                        resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                        Console.WriteLine("C�digo de resultado: " + resultCode);

                    }
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

            // Evaluar el resultado del procedimiento almacenado
            if (resultCode == 50006)
            {
                message = "Error, la contrase�a ingresado no incide";
            }
            else if (resultCode == 50007)
            {
                message = "Error, usuario no existe";
            }
            else if (resultCode == 1)
            {
                bandera = true;
                message = "Inicia de sesion exitosa";
            }
        }
    }
}
