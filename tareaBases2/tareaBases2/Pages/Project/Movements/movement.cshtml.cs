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
        public List<tipoMovimiento> listaTipoMovimiento = new List<tipoMovimiento>();
        public List<usuario> listaUsuario = new List<usuario>();
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
                    sqlRead = "SELECT * FROM Movimiento WHERE ValorDocId=@identificacion";

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
                                infoMovements.PostInIp = reader.GetString(7);
                                infoMovements.PostTime = reader.GetDateTime(8);

                                listaMovimientos.Add(infoMovements);
                                Console.WriteLine(infoMovements);
                            }
                        }
                    }
                    foreach(var info in listaMovimientos)
                    {
                        String idTipoMovimiento = info.IdTipoMovimiento.ToString();
                        sqlRead = "SELECT * FROM TipoMovimiento WHERE id=@idTipoMovimiento";

                        using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                        {
                            command.Parameters.AddWithValue("@IdTipoMovimiento", idTipoMovimiento);
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

                        String postByUser = info.PostByUser.ToString();
                        sqlRead = "SELECT * FROM Usuario WHERE id=@postByUser";

                        using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                        {
                            command.Parameters.AddWithValue("@postByUser", postByUser);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
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
