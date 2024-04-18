using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using static XML;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Movements
{
    public class InsertMovementModel : PageModel
    {
        public string idUser = "";
        public List<tipoMovimiento> listaTipoMovimiento = new List<tipoMovimiento>();
        public List<movements> listaMovimientos = new List<movements>();
        public movements infoMovements = new movements();
        public empleyee infoEmpleyee = new empleyee();
        public string monto = "";
        public string message = "";
        public void OnGet()
        {
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string id = Request.Query["id"]; // Obtener el ID del empleado desde la solicitud HTTP

                    // Llamar al procedimiento almacenado detallesEmpleado
                    using (SqlCommand command = new SqlCommand("listaMovimientos", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Especificar que el comando es un procedimiento almacenado
                        command.Parameters.AddWithValue("@id", id); // Pasar el ID del empleado como parámetro
                        command.Parameters.AddWithValue("@bandera", 1);

                        // Parámetro de salida para el código de resultado
                        SqlParameter outResultCodeParam = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outResultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outResultCodeParam);

                        // Ejecutar el comando SQL y leer los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                infoEmpleyee.id = reader.GetInt32(0);
                                infoEmpleyee.idPuesto = reader.GetInt32(1);
                                infoEmpleyee.Identificacion = reader.GetInt32(2);
                                infoEmpleyee.Nombre = reader.GetString(3);
                                infoEmpleyee.FechaContratacion = reader.GetDateTime(4);
                                infoEmpleyee.SaldoVaciones = reader.GetInt16(5);
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                tipoMovimiento infoTipoMovimiento = new tipoMovimiento();

                                infoTipoMovimiento.id = reader.GetInt32(0);
                                infoTipoMovimiento.Nombre = reader.GetString(1);
                                infoTipoMovimiento.TipoAccion = reader.GetString(2);

                                listaTipoMovimiento.Add(infoTipoMovimiento);
                                Console.WriteLine(infoTipoMovimiento);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        public void OnPost()
        {
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
        }
    }
}
