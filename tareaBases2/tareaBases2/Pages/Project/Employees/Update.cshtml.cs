using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace tareaBases2.Pages.Project.Employees
{
    public class UpdateModel : PageModel
    {
        public jobConnection jobs = new jobConnection();
        public infoEmpleyee info = new infoEmpleyee();
        public string message = "";
        public bool flag = false;

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                jobs.conexion();
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

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
                                info.idPuesto = reader.GetInt32(1);
                                info.Identificacion = reader.GetInt32(2);
                                info.Nombre = reader.GetString(3);
                                info.FechaContratacion = reader.GetDateTime(4);
                                info.SaldoVaciones = reader.GetDecimal(5);
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
            string auxIdentificacion = Request.Form["identificacion"];
            string auxNombre = Request.Form["nombre"];
            int resultCode = 0;

            // Validar los datos ingresados
            if (ValidarNomSal(auxIdentificacion, auxNombre) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                return;
            }
            // Asignar los valores validados al objeto infoEmpleyee
            info.Identificacion = int.Parse(auxIdentificacion);
            info.Nombre = auxNombre;

            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1" +
                                          ";Integrated Security=True;Encrypt=False";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sqlInfo = "UPDATE Empleado " +
                                    "SET ValorDocumentoIdentidad=@identificacion, Nombre=@nombre " +
                                    "WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sqlInfo, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@identificacion", info.Identificacion);
                        command.Parameters.AddWithValue("@nombre", info.Nombre);
                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir e imprimir el mensaje de error
                message = ex.Message;
                return;
            }
            // Evaluar el resultado del procedimiento almacenado
            if (resultCode == 50006)
            {
                message = "Error, el empleado que desea agregar ya existe";
            }
            else
            {
                flag = true;
                message = "Se a creado correctamente el empleado";
            }
        }
        public bool ValidarNomSal(string identificacion, string nombre)
        {
            // Verificar que ambos campos no estén vacíos
            if (nombre.Length != 0 || identificacion.Length != 0)
            {
                // Utilizar expresiones regulares para verificar el formato del nombre y salario
                if (Regex.IsMatch(nombre, @"^[a-zA-Z\s]+$") && Regex.IsMatch(identificacion, @"^[0-9]+$"))
                {
                    // Convertir el salario a decimal y verificar que sea mayor que cero
                    decimal auxIdentificacion = int.Parse(identificacion);
                    if (auxIdentificacion > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
