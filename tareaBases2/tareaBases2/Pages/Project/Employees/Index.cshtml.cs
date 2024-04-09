using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace tareaBases2.Pages.Project.Employees
{
    public class IndexModel : PageModel
    {
        public connection conexion = new connection();
        public List<infoEmpleyee> listaFiltrada = new List<infoEmpleyee>();

        public void OnGet()
        {
            buscarInput("");
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
                            infoEmpleyee info = new infoEmpleyee();
                            info.id = reader.GetInt32(0);
                            info.idPuesto = reader.GetInt32(1);
                            info.Identificacion = reader.GetInt32(2);
                            info.Nombre = reader.GetString(3);
                            info.FechaContratacion = reader.GetDateTime(4);
                            info.SaldoVaciones = reader.GetDecimal(5);
                            info.EsActivo = reader.GetBoolean(6);

                            listaFiltrada.Add(info);
                            Console.Write(info);
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