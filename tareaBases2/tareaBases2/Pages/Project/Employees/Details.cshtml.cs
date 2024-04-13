using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Employees
{
    public class DetailsModel : PageModel
    {
        public jobs infoJobs = new jobs();
        public infoEmpleyee info = new infoEmpleyee();
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
                                info.id = reader.GetInt32(0);
                                info.idPuesto = reader.GetInt32(1);
                                info.Identificacion = reader.GetInt32(2);
                                info.Nombre = reader.GetString(3);
                                info.FechaContratacion = reader.GetDateTime(4);
                                info.SaldoVaciones = reader.GetInt16(5);
                            }
                        }
                    }

                    String idPuesto = info.idPuesto.ToString();
                    sqlRead = "SELECT * FROM Puestos WHERE id=@idPuesto";

                    using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@idPuesto", idPuesto);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                infoJobs.id = reader.GetInt32(0);
                                infoJobs.NombrePuesto = reader.GetString(1);
                                infoJobs.SalarioxHora = reader.GetDecimal(2);

                                Console.WriteLine(infoJobs.NombrePuesto);
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
