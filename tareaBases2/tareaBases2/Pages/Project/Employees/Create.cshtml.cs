using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace tareaBases2.Pages.Project.Employees
{
    public class CreateModel : PageModel
    {
        // Objeto para almacenar la informaci�n del nuevo empleado
        public infoEmpleyee info = new infoEmpleyee();

        // Mensaje de retroalimentaci�n para el usuario
        public string message = "";

        // Bandera para indicar si se cre� correctamente el empleado
        public bool flag = false;

        // M�todo ejecutado al recibir una solicitud POST
        public void OnPost()
        {
            string auxNombre = Request.Form["nombre"];
            string auxSalario = Request.Form["salario"];
            int resultCode = 0;

            // Validar los datos ingresados
            if (ValidarNomSal(auxNombre, auxSalario) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                return;
            }
            // Asignar los valores validados al objeto infoEmpleyee
            info.Salario = decimal.Parse(auxSalario);
            info.Nombre = auxNombre;

            try
            {

                // Cadena de conexi�n a la base de datos
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1" +
                                          ";Integrated Security=True;Encrypt=False";

                // Establecer una conexi�n con la base de datos utilizando la cadena de conexi�n
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    // Especificar que el comando es un procedimiento almacenado
                    sqlConnection.Open(); // Abrir la conexi�n

                    string sqlInfo = "INSERT INTO Empleado" +
                                     "(Nombre, Salario) VALUES" +
                                     "(@nombre, @salario);";

                    // Crear un comando SQL para llamar al procedimiento almacenado "----"
                    using (SqlCommand command = new SqlCommand(sqlInfo, sqlConnection))
                    {
                        //command.CommandType = CommandType.StoredProcedure;

                        // Par�metros de entrada
                        command.Parameters.AddWithValue("@nombre", info.Nombre);
                        command.Parameters.AddWithValue("@salario", info.Salario);
                        command.ExecuteNonQuery();
                        //Aca a bajo es para cuando esta en el SP
                        /*
                        // Par�metro de salida
                        command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();

                        // Obtener el valor del par�metro de salida
                        resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                        Console.WriteLine("C�digo de resultado: " + resultCode);
                        */
                    }
                    // Cerrar la conexi�n despu�s de haber terminado de trabajar con ella
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepci�n que pueda ocurrir e imprimir el mensaje de error
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

        // M�todo para validar el nombre y salario ingresados
        public bool ValidarNomSal(string nombre, string salario)
        {
            // Verificar que ambos campos no est�n vac�os
            if (nombre.Length != 0 || salario.Length != 0)
            {
                // Utilizar expresiones regulares para verificar el formato del nombre y salario
                if (Regex.IsMatch(nombre, @"^[a-zA-Z\s]+$") && Regex.IsMatch(salario, @"^[0-9]+$"))
                {
                    // Convertir el salario a decimal y verificar que sea mayor que cero
                    decimal AuxSalario = decimal.Parse(salario);
                    if (AuxSalario > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}