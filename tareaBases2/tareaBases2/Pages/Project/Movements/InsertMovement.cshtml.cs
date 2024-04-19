using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using static XML;
using System.Data.SqlClient;
using System.Net;

namespace tareaBases2.Pages.Project.Movements
{
    public class InsertMovementModel : PageModel
    {
        public string idUser = "";
        public string id = "";
        public List<tipoMovimiento> listaTipoMovimiento = new List<tipoMovimiento>();
        public List<movements> listaMovimientos = new List<movements>();
        public movements infoMovements = new movements();
        public empleyee infoEmpleyee = new empleyee();
        public string monto = "";
        public string message = "";
        public bool flag = false;
        public void OnGet()
        {
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            id = Request.Query["id"]; // Obtener el ID del empleado desde la solicitud HTTP
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    // Llamar al procedimiento almacenado detallesEmpleado
                    using (SqlCommand command = new SqlCommand("listaMovimientos", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Especificar que el comando es un procedimiento almacenado
                        command.Parameters.AddWithValue("@id", id); // Pasar el ID del empleado como parámetro
                        command.Parameters.AddWithValue("@bandera", 1);

                        // Parámetro de salida para el código de resultado
                        SqlParameter outResultCodeParam = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outResultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outResultCodeParam);

                        // Ejecutar el comando SQL y leer los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                infoEmpleyee.id = reader.GetInt32(0);
                                infoEmpleyee.idPuesto = reader.GetInt32(1);
                                infoEmpleyee.Identificacion = reader.GetInt32(2);
                                infoEmpleyee.Nombre = reader.GetString(3);
                                infoEmpleyee.FechaContratacion = reader.GetDateTime(4);
                                infoEmpleyee.SaldoVaciones = reader.GetInt16(5);
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                tipoMovimiento infoTipoMovimiento = new tipoMovimiento();

                                infoTipoMovimiento.id = reader.GetInt32(0);
                                infoTipoMovimiento.Nombre = reader.GetString(1);
                                infoTipoMovimiento.TipoAccion = reader.GetString(2);

                                listaTipoMovimiento.Add(infoTipoMovimiento);
                                Console.WriteLine(infoTipoMovimiento);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        public void OnPost()
        {

            int resultCode = 0;
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            id = Request.Query["id"]; // Obtener el ID del empleado desde la solicitud HTTP

            string tipoMovimiento = Request.Form["tipoMovimiento"];
            string monto = Request.Form["monto"];

            DateTime fechaDelRegistro = DateTime.Now;
            DateTime fecha = DateTime.Now.Date;


            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            if(monto == "" || tipoMovimiento == "")
            {
                message = "Error en los datos, revise los datos ingresados";
                OnGet();
                return;
            }


            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                    Console.WriteLine(localIP);
                    ;
                }
            }

            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    // Llamar al procedimiento almacenado detallesEmpleado
                    using (SqlCommand command = new SqlCommand("insertarMovimiento", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Especificar que el comando es un procedimiento almacenado
                        command.Parameters.AddWithValue("@idEmpleado", int.Parse(id));
                        command.Parameters.AddWithValue("@idTipoMovimiento", int.Parse(tipoMovimiento)); // Convertir tipoMovimiento a entero
                        command.Parameters.AddWithValue("@fecha", fecha);
                        command.Parameters.AddWithValue("@monto", decimal.Parse(monto)); // Convertir monto a decimal
                        command.Parameters.AddWithValue("@idPostByuser", int.Parse(idUser));
                        command.Parameters.AddWithValue("@postInIp", localIP);
                        command.Parameters.AddWithValue("@postTime", fechaDelRegistro);

                        // Parámetro de salida para el código de resultado
                        SqlParameter outResultCodeParam = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outResultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outResultCodeParam);

                        command.ExecuteNonQuery(); // Ejecutar el comando

                        resultCode = Convert.ToInt32(outResultCodeParam.Value); // Obtener el valor del parámetro de salida
                        Console.WriteLine("Código de resultado: " + resultCode);
                    }

                    if(resultCode == 0)
                    {
                        message = "Insercion de movimiento existosa.";
                    }
                    else if (resultCode == 50011)
                    {
                        message = "Error, el saldo no puede ser negativo.";
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            OnGet();
        }
    }
}
