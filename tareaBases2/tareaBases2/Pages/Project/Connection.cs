using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using static XML;

public class connection
{
    public List<empleyee> listEmployee = new List<empleyee>();

    public void connectionTable()
    {
        try
        {
            string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("tablaEmpleado", sqlConnection)) // Llamar al procedimiento almacenado
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetro de salida
                    SqlParameter outParameter = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                    outParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outParameter);

                    // Agregar parámetro de entrada
                    command.Parameters.AddWithValue("@buscar", "");

                    // Ejecutar el comando y procesar los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Crear objeto empleyee y llenarlo con datos del resultado de la consulta
                            empleyee infoEmpleyee = new empleyee();
                            infoEmpleyee.id = reader.GetInt32(0);
                            infoEmpleyee.idPuesto = reader.GetInt32(1);
                            infoEmpleyee.Identificacion = reader.GetInt32(2);
                            infoEmpleyee.Nombre = reader.GetString(3);
                            infoEmpleyee.FechaContratacion = reader.GetDateTime(4);
                            infoEmpleyee.SaldoVaciones = reader.GetInt16(5);
                            infoEmpleyee.EsActivo = reader.GetBoolean(6);

                            // Agregar el objeto empleyee a la lista filtrada
                            listEmployee.Add(infoEmpleyee);
                        }
                    }

                    // Recuperar el valor del parámetro de salida
                    int resultCode = Convert.ToInt32(outParameter.Value);
                    // Manejar el código de resultado según sea necesario
                }
                sqlConnection.Close();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}