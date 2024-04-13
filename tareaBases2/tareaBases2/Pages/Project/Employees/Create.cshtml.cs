using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using tareaBases2.Pages.Project;


namespace tareaBases2.Pages.Project.Employees
{
    public class CreateModel : PageModel
    {
        // Objeto para almacenar la informaci�n del nuevo empleado
        public empleyee infoEmpleyee = new empleyee();
        public jobConnection jobs = new jobConnection();
        // Mensaje de retroalimentaci�n para el usuario
        public string message = "";
        // Bandera para indicar si se cre� correctamente el empleado
        public bool flag = false;
        // M�todo ejecutado al recibir una solicitud POST
        // Llamar al m�todo puestos
        
        public void OnGet()
        {
            jobs.conexion();
        }

        public void OnPost()
        {
            string auxIdentificacion = Request.Form["identificacion"];
            string auxNombre = Request.Form["nombre"];
            string puesto = Request.Form["puesto"];
            DateTime fechaContratacion = DateTime.Now;

            int resultCode = 0;

            // Validar los datos ingresados
            if (ValidarNomSal(auxIdentificacion, auxNombre) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                return;
            }
            // Asignar los valores validados al objeto infoEmpleyee
            infoEmpleyee.idPuesto = int.Parse(puesto);
            infoEmpleyee.Identificacion = int.Parse(auxIdentificacion);
            infoEmpleyee.Nombre = auxNombre;
            infoEmpleyee.FechaContratacion = fechaContratacion;
            infoEmpleyee.SaldoVaciones = 0;
            infoEmpleyee.EsActivo = true;

            try
            {

                // Cadena de conexi�n a la base de datos
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2" +
                                          ";Integrated Security=True;Encrypt=False";

                // Establecer una conexi�n con la base de datos utilizando la cadena de conexi�n
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    // Especificar que el comando es un procedimiento almacenado
                    sqlConnection.Open(); // Abrir la conexi�n

                    string sqlInfo = "INSERT INTO Empleado" +
                                     "(IdPuesto, ValorDocumentoIdentidad, Nombre, FechaContratacion, SaldoVacaciones, EsActivo) VALUES" +
                                     "(@idPuesto, @identificacion, @nombre, @fechaContratacion, @SaldoVaciones, @EsActivo );";

                    // Crear un comando SQL para llamar al procedimiento almacenado "----"
                    using (SqlCommand command = new SqlCommand(sqlInfo, sqlConnection))
                    {;
                        //command.CommandType = CommandType.StoredProcedure;

                        // Par�metros de entrada
                        command.Parameters.AddWithValue("@idPuesto", infoEmpleyee.idPuesto);
                        command.Parameters.AddWithValue("@identificacion", infoEmpleyee.Identificacion);
                        command.Parameters.AddWithValue("@nombre", infoEmpleyee.Nombre);
                        command.Parameters.AddWithValue("@fechaContratacion", infoEmpleyee.FechaContratacion);
                        command.Parameters.AddWithValue("@SaldoVaciones", infoEmpleyee.SaldoVaciones);
                        command.Parameters.AddWithValue("@EsActivo", infoEmpleyee.EsActivo);
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
        public bool ValidarNomSal(string identificacion, string nombre)
        {
            // Verificar que ambos campos no est�n vac�os
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