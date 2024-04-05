using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public class connection
{
    public List<infoEmpleyee> listEmployee = new List<infoEmpleyee>();

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
                            infoEmpleyee info = new infoEmpleyee();
                            info.id = reader.GetInt32(0);
                            info.idPuesto = reader.GetInt32(1);
                            info.Identificacion = reader.GetInt32(2);
                            info.Nombre = reader.GetString(3);
                            info.FechaContratacion = reader.GetDateTime(4);
                            info.SaldoVaciones = reader.GetDecimal(5);
                            info.EsActivo = reader.GetBoolean(6);

                            listEmployee.Add(info);
                            Console.Write(info);
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
public class infoEmpleyee
{
    public Int32 id;
    public Int32 idPuesto;
    public int Identificacion;
    public string Nombre;
    public DateTime FechaContratacion;
    public decimal SaldoVaciones;
    public bool EsActivo;

}