using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Employees
{
    public class DetailsModel : PageModel
    {
        public string idUser = "";
        public jobs infoJobs = new jobs(); // Objeto para almacenar los detalles del puesto de trabajo
        public empleyee infoEmpleyee = new empleyee(); // Objeto para almacenar los detalles del empleado
        public string message = ""; // Mensaje para manejar errores o información adicional

        public void OnGet()
        {
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            try
            {
                // Cadena de conexión a la base de datos
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

                // Establecer una conexión con la base de datos utilizando la cadena de conexión
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open(); // Abrir la conexión

                    string id = Request.Query["id"]; // Obtener el ID del empleado desde la solicitud HTTP

                    // Llamar al procedimiento almacenado detallesEmpleado
                    using (SqlCommand command = new SqlCommand("detallesEmpleado", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Especificar que el comando es un procedimiento almacenado
                        command.Parameters.AddWithValue("@id", id); // Pasar el ID del empleado como parámetro

                        // Parámetro de salida para el código de resultado
                        SqlParameter outResultCodeParam = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outResultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outResultCodeParam);

                        // Ejecutar el comando SQL y leer los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Leer los datos del empleado
                                infoEmpleyee.id = reader.GetInt32(0);
                                infoEmpleyee.idPuesto = reader.GetInt32(1);
                                infoEmpleyee.Identificacion = reader.GetInt32(2);
                                infoEmpleyee.Nombre = reader.GetString(3);
                                infoEmpleyee.FechaContratacion = reader.GetDateTime(4);
                                infoEmpleyee.SaldoVaciones = reader.GetInt16(5);
                            }

                            // Mover al siguiente conjunto de resultados (datos del puesto de trabajo)
                            reader.NextResult();

                            while (reader.Read())
                            {
                                // Leer los datos del puesto de trabajo
                                infoJobs.id = reader.GetInt32(0); // ID del puesto
                                infoJobs.NombrePuesto = reader.GetString(1); // Nombre del puesto
                                infoJobs.SalarioxHora = reader.GetDecimal(2); // Salario por hora
                            }
                        }

                        // Leer el código de resultado
                        int resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                        Console.WriteLine("Código de resultado: " + resultCode);
                    }

                    // Cerrar la conexión después de haber terminado de trabajar con ella
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Capturar excepciones y almacenar el mensaje de error
                message = ex.Message;
            }
        }
    }
}
