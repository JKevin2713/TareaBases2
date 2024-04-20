using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using tareaBases2.Pages.Project;


namespace tareaBases2.Pages.Project.Employees
{
    public class CreateModel : PageModel
    {
        public string idUser = "";
        // Objeto para almacenar la información del nuevo empleado
        public empleyee infoEmpleyee = new empleyee();
        public jobConnection jobs = new jobConnection();
        public insertarBitacora insertar = new insertarBitacora();
        // Mensaje de retroalimentación para el usuario
        public string message = "";
        public string tipoEvento = "";
        // Bandera para indicar si se creó correctamente el empleado
        public bool flag = false;
        // Método ejecutado al recibir una solicitud POST
        // Llamar al método puestos

        public void OnGet()
        {
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            jobs.conexion();
        }

        public void OnPost()
        {
            idUser = Request.Query["idUser"]; // Obtener el ID del empleado desde la solicitud HTTP
            string auxIdentificacion = Request.Form["identificacion"];
            string auxNombre = Request.Form["nombre"];
            String auxPuesto = Request.Form["puesto"];
            int resultCode = 0;
            string puestos = "";
            DateTime fechaContratacion = DateTime.Now;

            // Validar los datos ingresados
            if (ValidarNomSal(auxIdentificacion, auxNombre, auxPuesto) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                OnGet();
                return;
            }

            try
            {

                // Cadena de conexión a la base de datos
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2" +
                                          ";Integrated Security=True;Encrypt=False";

                // Establecer una conexión con la base de datos utilizando la cadena de conexión
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    // Especificar que el comando es un procedimiento almacenado
                    sqlConnection.Open(); // Abrir la conexión

                    // Asignar los valores validados al objeto infoEmpleyee
                    infoEmpleyee.idPuesto = int.Parse(auxPuesto);
                    infoEmpleyee.Identificacion = int.Parse(auxIdentificacion);
                    infoEmpleyee.Nombre = auxNombre;
                    infoEmpleyee.FechaContratacion = fechaContratacion;
                    infoEmpleyee.SaldoVaciones = 0;
                    infoEmpleyee.EsActivo = true;

                    // Crear un comando SQL para llamar al stored procedure "registroEmpleado"
                    using (SqlCommand command = new SqlCommand("registroEmpleado", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entraada
                        command.Parameters.AddWithValue("@idPuesto", infoEmpleyee.idPuesto);
                        command.Parameters.AddWithValue("@identificacion", infoEmpleyee.Identificacion);
                        command.Parameters.AddWithValue("@nombre", infoEmpleyee.Nombre);
                        command.Parameters.AddWithValue("@fechaContratacion", infoEmpleyee.FechaContratacion);
                        command.Parameters.AddWithValue("@SaldoVacaciones", infoEmpleyee.SaldoVaciones);
                        command.Parameters.AddWithValue("@EsActivo", infoEmpleyee.EsActivo);

                        SqlParameter outResultCodeParam = new SqlParameter("@OutResulTCode", SqlDbType.Int);
                        outResultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outResultCodeParam);


                        // Parámetro de salida para el ID del movimiento insertado
                        SqlParameter outPuesto = new SqlParameter("@OutPuesto", SqlDbType.VarChar, 64);
                        outPuesto.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outPuesto);


                        command.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        resultCode = Convert.ToInt32(outResultCodeParam.Value);
                        puestos = Convert.ToString(outPuesto.Value);

                    }
                    string messageBase = "";
                    // Evaluar el resultado del procedimiento almacenado
                    if (resultCode == 50006)
                    {
                        tipoEvento = "Login No Exitoso";
                        message = "Empleado con mismo nombre ya existe en inserción.";
                        messageBase = "Empleado con mismo nombre ya existe en inserción." +
                            "Cedula = " + infoEmpleyee.Identificacion +
                            " Nombre = " + infoEmpleyee.Nombre +
                            " Puesto = " + puestos;
                    }
                    else if (resultCode == 50007)
                    {
                        tipoEvento = "Login No Exitoso";
                        message = "Empleado con ValorDocumentoIdentidad ya existe en inserción.";
                        messageBase = "Empleado con ValorDocumentoIdentidad ya existe en inserción." +
                            "Cedula = " + infoEmpleyee.Identificacion +
                            " Nombre = " + infoEmpleyee.Nombre +
                            " Puesto = " + puestos;
                    }
                    else
                    {
                        flag = true;
                        tipoEvento = "Login Exitoso";
                        message = "Empleado insertado correctamente Cedula.";
                        messageBase = "Empleado insertado correctamente Cedula." +
                            "Cedula = " + infoEmpleyee.Identificacion +
                            " Nombre = " + infoEmpleyee.Nombre +
                            " Puesto = " + puestos;
                    }

                    // Aca para abajo es para la bitacora

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

        // Método para validar el nombre y salario ingresados
        public bool ValidarNomSal(string identificacion, string nombre, string puesto)
        {
            // Verificar que ambos campos no estén vacíos
            if ((nombre.Length != 0 || identificacion.Length != 0) && puesto != "")
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