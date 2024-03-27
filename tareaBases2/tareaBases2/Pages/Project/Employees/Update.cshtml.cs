using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Employees
{
    public class UpdateModel : PageModel
    {
        public infoEmpleyee info = new infoEmpleyee();
        public string message = "";
        public bool flag = false;

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1;Integrated Security=True;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sqlRead = "SELECT * FROM Empleado WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                info.id = reader.GetInt32(0);
                                info.Nombre = reader.GetString(1);
                                info.Salario = reader.GetDecimal(2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        public void OnPost()
        {
            String id = Request.Query["id"];
            info.Nombre = Request.Form["nombre"];
            string salario = Request.Form["salario"];
            info.Salario = decimal.Parse(salario);

            if (info.Nombre.Length == 0 || salario.Length == 0)
            {
                message = "Se necesita llenar todos los campos";
                return;
            }
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1" +
                                          ";Integrated Security=True;Encrypt=False";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sqlInfo = "UPDATE Empleado " +
                                    "SET Nombre=@nombre, Salario=@salario " +
                                    "WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sqlInfo, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@nombre", info.Nombre);
                        command.Parameters.AddWithValue("@salario", info.Salario);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return;
            }



            flag = true;
            info.Nombre = "";
            info.Salario = 0;
            salario = "";
            message = "Se modifico el empleado correctamente";
        }
    }
}
