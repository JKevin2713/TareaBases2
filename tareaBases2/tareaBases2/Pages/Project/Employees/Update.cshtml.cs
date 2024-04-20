using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static XML;

namespace tareaBases2.Pages.Project.Employees
{
    public class UpdateModel : PageModel
    {
        public string idUser = "";
        public string id = "";
        public jobConnection jobs = new jobConnection();
        public empleyee infoEmpleyee = new empleyee();
        public insertarBitacora insertar = new insertarBitacora();
        public string message = "";
        public bool flag = false;


        public int identidadAntesEditar = 0;
        public string nombreAntesEditar = "";
        public int puestoAntesEditar = 0;
        public void OnGet()
        {
            id = Request.Query["id"];
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            try
            {
                jobs.conexion();
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand("modificarEmpleado", sqlConnection)) // Llamar al procedimiento almacenado
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetro de salida
                        SqlParameter outParameter = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outParameter);


                        // Parámetro de salida para el ID del movimiento insertado
                        SqlParameter outCedulaAnterior = new SqlParameter("@OutCedulaAnterior", SqlDbType.Int);
                        outCedulaAnterior.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outCedulaAnterior);

                        // Parámetro de salida para el nuevo saldo
                        SqlParameter outNombreAnterior = new SqlParameter("@OutNombreAnterior", SqlDbType.VarChar, 64);
                        outNombreAnterior.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outNombreAnterior);

                        // Parámetro de salida para el saldo actual
                        SqlParameter outPuestoAnterior = new SqlParameter("@OutPuestoAnterior", SqlDbType.VarChar, 64);
                        outPuestoAnterior.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outPuestoAnterior);

                        // Parámetro de salida para el saldo actual
                        SqlParameter outPuestoDespues = new SqlParameter("@OutPuestoDespues", SqlDbType.VarChar, 64);
                        outPuestoDespues.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outPuestoDespues);

                        // Agregar parámetro de entrada
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@bandera", 0); // Bandera para indicar que se va a modificar el empleado
                        command.Parameters.AddWithValue("@idPuesto", 0);
                        command.Parameters.AddWithValue("@identificacion", 0);
                        command.Parameters.AddWithValue("@nombre", "");

                        // Ejecutar el comando y procesar los resultados
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
                        }
                        identidadAntesEditar = infoEmpleyee.Identificacion;
                        nombreAntesEditar = infoEmpleyee.Nombre;
                        puestoAntesEditar = infoEmpleyee.idPuesto;
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
            int cedulaAnterior = 0;
            string nombreAnterior = "";
            string puestoAnterior = "";
            string puestoDespues = "";


            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            id = Request.Query["id"];
            string auxPuesto = Request.Form["puesto"];
            string auxIdentificacion = Request.Form["identificacion"];
            string auxNombre = Request.Form["nombre"];
            int resultCode = 0;


            // Validar los datos ingresados
            if (ValidarNomSal(auxIdentificacion, auxNombre, auxPuesto) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                OnGet();
                return;
            }
            // Asignar los valores validados al objeto infoEmpleyee
            infoEmpleyee.Identificacion = int.Parse(auxIdentificacion);
            infoEmpleyee.Nombre = auxNombre;
            infoEmpleyee.idPuesto = int.Parse(auxPuesto);

            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2" +
                                          ";Integrated Security=True;Encrypt=False";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string sqlInfo = "modificarEmpleado"; // Nombre del procedimiento almacenado

                    using (SqlCommand command = new SqlCommand(sqlInfo, sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@bandera", 1); // Bandera para indicar que se va a modificar el empleado
                        command.Parameters.AddWithValue("@idPuesto", infoEmpleyee.idPuesto);
                        command.Parameters.AddWithValue("@identificacion", infoEmpleyee.Identificacion);
                        command.Parameters.AddWithValue("@nombre", infoEmpleyee.Nombre);

                        // Parámetro de salida para el código de resultado
                        SqlParameter outResultCodeParam = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outResultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outResultCodeParam);


                        // Parámetro de salida para el ID del movimiento insertado
                        SqlParameter outCedulaAnterior = new SqlParameter("@OutCedulaAnterior", SqlDbType.Int);
                        outCedulaAnterior.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outCedulaAnterior);

                        // Parámetro de salida para el nuevo saldo
                        SqlParameter outNombreAnterior = new SqlParameter("@OutNombreAnterior", SqlDbType.VarChar, 64);
                        outNombreAnterior.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outNombreAnterior);

                        // Parámetro de salida para el saldo actual
                        SqlParameter outPuestoAnterior = new SqlParameter("@OutPuestoAnterior", SqlDbType.VarChar, 64);
                        outPuestoAnterior.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outPuestoAnterior);

                        // Parámetro de salida para el saldo actual
                        SqlParameter outPuestoDespues = new SqlParameter("@OutPuestoDespues", SqlDbType.VarChar, 64);
                        outPuestoDespues.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outPuestoDespues);


                        command.ExecuteNonQuery(); // Ejecutar el comando

                        // Obtener el valor de los parámetros de salida
                        resultCode = Convert.ToInt32(outResultCodeParam.Value);
                        cedulaAnterior = Convert.ToInt32(outCedulaAnterior.Value);
                        nombreAnterior = Convert.ToString(outNombreAnterior.Value);
                        puestoAnterior = Convert.ToString(outPuestoAnterior.Value);
                        puestoDespues = Convert.ToString(outPuestoDespues.Value);

                        // Imprimir los valores obtenidos
                        Console.WriteLine("Código de resultado: " + resultCode);
                        Console.WriteLine("cedula: " + cedulaAnterior);
                        Console.WriteLine("Nombre: " + nombreAnterior);
                        Console.WriteLine("Nombre tipo de movimiento antes: " + puestoAnterior);
                        Console.WriteLine("Nombre tipo de movimiento modifica: " + puestoDespues);

                    }
                    string messageBase = "";
                    string tipoEvento = "";
                    // Evaluar el resultado del procedimiento almacenado
                    if (resultCode == 50006)
                    {
                        tipoEvento = "Update no exitoso";
                        message = "Empleado con ValorDocumentoIdentidad ya existe en actualizacion";
                        messageBase = "Empleado con ValorDocumentoIdentidad ya existe en actualizacion" +
                            " Cedula anterior = " + cedulaAnterior +
                            " Nombre anterior = " + nombreAnterior +
                            " Puesto anterior = " + puestoAnterior +
                            " Cedula = " + infoEmpleyee.Identificacion +
                            " Nombre = " + infoEmpleyee.Nombre +
                            " Puesto = " + puestoDespues;
                    }
                    else if (resultCode == 50007)
                    {
                        tipoEvento = "Update no exitoso";
                        message = "Empleado con mismo nombre ya existe en actualización";
                        messageBase = "Empleado con mismo nombre ya existe en actualización" +
                            " Cedula anterior = " + cedulaAnterior +
                            " Nombre anterior = " + nombreAnterior +
                            " Puesto anterior = " + puestoAnterior +
                            " Cedula = " + infoEmpleyee.Identificacion +
                            " Nombre = " + infoEmpleyee.Nombre +
                            " Puesto = " + puestoDespues;
                    }
                    else
                    {
                        flag = true;
                        tipoEvento  = "Update exitoso";
                        message = "Se a creado correctamente el empleado";
                        messageBase = "Se a creado correctamente el empleado" +
                            " Cedula anterior = " + cedulaAnterior +
                            " Nombre anterior = " + nombreAnterior +
                            " Puesto anterior = " + puestoAnterior +
                            " Cedula = " + infoEmpleyee.Identificacion +
                            " Nombre = " + infoEmpleyee.Nombre +
                            " Puesto = " + puestoDespues;
                    }

                    insertar.insertarBitacoraEventos(sqlConnection, messageBase, tipoEvento, idUser);

                    sqlConnection.Close();
                }
            }

            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir e imprimir el mensaje de error
                message = ex.Message;
                return;
            }

            OnGet();
        }
        public bool ValidarNomSal(string identificacion, string nombre, string auxPuesto)
        {
            // Verificar que ambos campos no estén vacíos
            if ((nombre.Length != 0 || identificacion.Length != 0) && auxPuesto != "")
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
