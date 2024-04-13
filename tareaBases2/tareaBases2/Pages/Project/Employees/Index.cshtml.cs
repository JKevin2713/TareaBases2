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
        public connection conexion = new connection();
        public XML xmlLoad = new XML();
        public List<movements> listaMovimientos = new List<movements>();
        public List<empleyee> listaFiltrada = new List<empleyee>();

        public void OnGet()
        {
            buscarInput("");
            xmlLoad.Cargar();
        }
        
        public void OnPost(string buscar)
        {
            // Lógica para filtrar con el valor recibido
            buscarInput(buscar);
        }
        public void buscarInput(string buscar)
        {
            string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string sqlRead = "";

                if (validarBusquedaNombre(buscar) == true)
                {
                    sqlRead = "SELECT * FROM Empleado WHERE Nombre LIKE '%' + @buscar + '%';";
                }
                else if (validarBusquedaCedula(buscar) == true)
                {
                    sqlRead = "SELECT * FROM Empleado WHERE ValorDocumentoIdentidad LIKE '%' + @buscar + '%';";
                }
                else
                {
                    sqlRead = "SELECT * FROM Empleado;";
                    buscar = "";
                }

                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(sqlRead, sqlConnection))
                {
                    command.Parameters.AddWithValue("@buscar", buscar);
                    
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
                            infoEmpleyee.SaldoVaciones = reader.GetInt16(5);
                            infoEmpleyee.EsActivo = reader.GetBoolean(6);

                            listaFiltrada.Add(infoEmpleyee);
                            Console.Write(infoEmpleyee);
                        }
                    }
                }
                sqlConnection.Close();
            }
        }
        public bool validarBusquedaNombre(string buscar)
        {
            // Verificar que ambos campos no estén vacíos
            try
            {
                if (buscar.Length != 0)
                {
                    if (Regex.IsMatch(buscar, @"^[a-zA-Z\s]+$"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool validarBusquedaCedula(string buscar)
        {
            try
            { 
                // Verificar que ambos campos no estén vacíos
                if (buscar.Length != 0)
                {
                    if (Regex.IsMatch(buscar, @"^[0-9]+$"))
                    {
                        return true;
                    }
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