using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Movements
{
    public class movementModel : PageModel
    {

        public jobs infoJobs = new jobs();
        public empleyee infoEmpleyee = new empleyee();
        public List<movements> listaMovimientos = new List<movements>();
        public string message = "";
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    String id = Request.Query["id"];
                    string sqlRead = "SELECT * FROM Empleado WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
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
                        }
                    }

                    String identificacion = infoEmpleyee.Identificacion.ToString();
                    sqlRead = "SELECT * FROM Movimietos WHERE ValorDocId=@identificacion";

                    using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@identificacion", identificacion);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                                infoMovements.PostInIp = reader.GetInt32(7);
                                infoMovements.PostTime = reader.GetDateTime(8);

                                listaMovimientos.Add(infoMovements);
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
