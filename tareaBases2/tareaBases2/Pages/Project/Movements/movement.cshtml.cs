using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace tareaBases2.Pages.Project.Movements
{
    public class movementModel : PageModel
    {
        public string idUser = "";
        public jobs infoJobs = new jobs();
        public empleyee infoEmpleyee = new empleyee();
        public List<movements> listaMovimientos = new List<movements>();
        public List<tipoMovimiento> listaTipoMovimiento = new List<tipoMovimiento>();
        public List<usuario> listaUsuario = new List<usuario>();
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
                        command.Parameters.AddWithValue("@bandera", 0);

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
                                movements infoMovements = new movements();
                                infoMovements.id = reader.GetInt32(0);
                                infoMovements.ValorDocId = reader.GetInt32(1);
                                infoMovements.IdTipoMovimiento = reader.GetInt32(2);
                                infoMovements.Fecha = reader.GetDateTime(3);
                                infoMovements.Monto = reader.GetDecimal(4);
                                infoMovements.NuevoSaldo = reader.GetDecimal(5);
                                infoMovements.PostByUser = reader.GetInt32(6);
                                infoMovements.PostInIp = reader.GetString(7);
                                infoMovements.PostTime = reader.GetDateTime(8);

                                listaMovimientos.Add(infoMovements);
                                Console.WriteLine(infoMovements);
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
                            reader.NextResult();

                            while (reader.Read())
                            {
                                usuario infoUsuario = new usuario();

                                infoUsuario.id = reader.GetInt32(0);
                                infoUsuario.Username = reader.GetString(1);
                                infoUsuario.Password = reader.GetString(2);

                                listaUsuario.Add(infoUsuario);
                                Console.WriteLine(infoUsuario);
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
    }
}
