using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static XML;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Movements
{
    public class InsertMovementModel : PageModel
    {
        public List<tipoMovimiento> listaTipoMovimiento = new List<tipoMovimiento>();
        public List<movements> listaMovimientos = new List<movements>();
        public movements infoMovements = new movements();
        public empleyee infoEmpleyee = new empleyee();
        public string monto = "";
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

                    sqlRead = "SELECT * FROM TipoMovimiento";

                    using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
    }
}
