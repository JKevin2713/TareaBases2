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
                string sqlRead = "SELECT * FROM Empleado";

                using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empleyee infoEmpleyee = new empleyee();
                            infoEmpleyee.id = reader.GetInt32(0);
                            infoEmpleyee.idPuesto = reader.GetInt32(1);
                            infoEmpleyee.Identificacion = reader.GetInt32(2);
                            infoEmpleyee.Nombre = reader.GetString(3);
                            infoEmpleyee.FechaContratacion = reader.GetDateTime(4);
                            infoEmpleyee.SaldoVaciones = reader.GetDecimal(5);
                            infoEmpleyee.EsActivo = reader.GetBoolean(6);

                            listEmployee.Add(infoEmpleyee);
                            Console.Write(infoEmpleyee);
                        }
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