using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using static XML;

namespace tareaBases2.Pages.Project.Employees
{
    public class IndexModel : PageModel
    {
        // Instancia de la clase para establecer la conexión a la base de datos
        public connection conexion = new connection();
        // Lista de objetos 'movements'
        public List<movements> listaMovimientos = new List<movements>();
        // Lista de objetos 'empleyee' filtrados
        public List<empleyee> listaFiltrada = new List<empleyee>();
        // Método ejecutado cuando se realiza una solicitud GET
        public void OnGet()
        {
            // Llama a buscarInput con una cadena vacía
            buscarInput("");
        }

        // Método ejecutado cuando se realiza una solicitud POST
        public void OnPost(string buscar)
        {
            // Lógica para filtrar con el valor recibido
            if (buscar == null)
            {
                buscarInput("");
            }
            else
            {
                buscarInput(buscar);
            }
        }

        // Método para buscar empleados en la base de datos según un criterio de búsqueda
        public void buscarInput(string buscar)
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

            // Establecer conexión a la base de datos
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
                    command.Parameters.AddWithValue("@buscar", buscar);

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
                            listaFiltrada.Add(infoEmpleyee);
                        }
                    }

                    // Recuperar el valor del parámetro de salida
                    int resultCode = Convert.ToInt32(outParameter.Value);
                    // Manejar el código de resultado según sea necesario
                }
                sqlConnection.Close();
            }
        }

        // Método para validar el formato del criterio de búsqueda por nombre
        public bool validarBusquedaNombre(string buscar)
        {
            try
            {
                // Verificar que el campo no esté vacío y contenga solo letras y espacios
                if (buscar.Length != 0 && Regex.IsMatch(buscar, @"^[a-zA-Z\s]+$"))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Método para validar el formato del criterio de búsqueda por cédula
        public bool validarBusquedaCedula(string buscar)
        {
            try
            {
                // Verificar que el campo no esté vacío y contenga solo dígitos
                if (buscar.Length != 0 && Regex.IsMatch(buscar, @"^[0-9]+$"))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}