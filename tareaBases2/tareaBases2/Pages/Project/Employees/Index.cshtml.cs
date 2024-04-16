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
        // Instancia de la clase para establecer la conexi�n a la base de datos
        public connection conexion = new connection();
        // Lista de objetos 'movements'
        public List<movements> listaMovimientos = new List<movements>();
        // Lista de objetos 'empleyee' filtrados
        public List<empleyee> listaFiltrada = new List<empleyee>();
        // M�todo ejecutado cuando se realiza una solicitud GET
        public void OnGet()
        {
            // Llama a buscarInput con una cadena vac�a
            buscarInput("");
        }

        // M�todo ejecutado cuando se realiza una solicitud POST
        public void OnPost(string buscar)
        {
            // L�gica para filtrar con el valor recibido
            if (buscar == null)
            {
                buscarInput("");
            }
            else
            {
                buscarInput(buscar);
            }
        }

        // M�todo para buscar empleados en la base de datos seg�n un criterio de b�squeda
        public void buscarInput(string buscar)
        {
            // Cadena de conexi�n a la base de datos
            string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

            // Establecer conexi�n a la base de datos
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("tablaEmpleado", sqlConnection)) // Llamar al procedimiento almacenado
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar par�metro de salida
                    SqlParameter outParameter = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                    outParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outParameter);

                    // Agregar par�metro de entrada
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

                    // Recuperar el valor del par�metro de salida
                    int resultCode = Convert.ToInt32(outParameter.Value);
                    // Manejar el c�digo de resultado seg�n sea necesario
                }
                sqlConnection.Close();
            }
        }

        // M�todo para validar el formato del criterio de b�squeda por nombre
        public bool validarBusquedaNombre(string buscar)
        {
            try
            {
                // Verificar que el campo no est� vac�o y contenga solo letras y espacios
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

        // M�todo para validar el formato del criterio de b�squeda por c�dula
        public bool validarBusquedaCedula(string buscar)
        {
            try
            {
                // Verificar que el campo no est� vac�o y contenga solo d�gitos
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