using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project;
public class jobConnection
{
    public List<jobs> ListJobs = new List<jobs>();

    public void conexion()
    {
        try
        {
            string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string sqlReadPuestoSP = "tablaPuestos";
                using (SqlCommand command = new SqlCommand(sqlReadPuestoSP, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetro de salida
                    SqlParameter outParameter = new SqlParameter();
                    outParameter.ParameterName = "@OutResulTCode";
                    outParameter.SqlDbType = SqlDbType.Int;
                    outParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outParameter);

                    using (SqlDataReader readerPuesto = command.ExecuteReader())
                    {
                        while (readerPuesto.Read())
                        {
                            jobs infoJobs = new jobs();
                            infoJobs.id = readerPuesto.GetInt32(0);
                            infoJobs.NombrePuesto = readerPuesto.GetString(1);
                            infoJobs.SalarioxHora = readerPuesto.GetDecimal(2);

                            ListJobs.Add(infoJobs);
                            Console.Write(infoJobs.SalarioxHora);
                        }
                    }

                    // Obtener el valor del parámetro de salida después de ejecutar el procedimiento almacenado
                    int resultCode = Convert.ToInt32(outParameter.Value);
                    // Manejar el resultado, si es necesario
                    if (resultCode != 0)
                    {
                        // Aquí puedes manejar el código de resultado de salida, si es diferente de cero
                        Console.WriteLine("Resultado del procedimiento almacenado: " + resultCode);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
