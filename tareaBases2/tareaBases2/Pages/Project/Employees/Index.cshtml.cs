using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project.Employees
{
    public class IndexModel : PageModel
    {
        public List<infoEmpleyee> listEmployee = new List<infoEmpleyee>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1;Integrated Security=True;Encrypt=False";

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
                                info.Nombre = reader.GetString(1);
                                info.Salario = reader.GetDecimal(2);

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
        public string Nombre;
        public decimal Salario;

    }
}
