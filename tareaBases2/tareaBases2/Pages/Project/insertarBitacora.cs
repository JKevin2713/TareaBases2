
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace tareaBases2.Pages.Project;
public class insertarBitacora
{

    public bitacoraEvento bitacora = new bitacoraEvento();
    public void insertarBitacoraEventos(SqlConnection sqlConnection, string mensaje, string tipoEvento, string idUser)
    {
        try
        {
            DateTime fechaContratacion = DateTime.Now;
            int resultCode = 0;
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                    Console.WriteLine(localIP);
                    ;
                }
            }
            bitacora.idTipoEvento = tipoEvento;
            bitacora.descripcion = mensaje;
            bitacora.PostByUser = int.Parse(idUser);
            bitacora.PostInIp = localIP;
            bitacora.PostTime = fechaContratacion;

            using (SqlCommand command = new SqlCommand("insertarBitacora", sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idTipoEvento", bitacora.idTipoEvento);
                command.Parameters.AddWithValue("@Descripcion", bitacora.descripcion);
                command.Parameters.AddWithValue("@IdPostByUser", bitacora.PostByUser);
                command.Parameters.AddWithValue("@PostInIP", bitacora.PostInIp);
                command.Parameters.AddWithValue("@PostTime", bitacora.PostTime);

                // Parámetro de salida
                command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();

                // Obtener el valor del parámetro de salida
                resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                Console.WriteLine("Código de resultado: " + resultCode);

            }
        }
        catch (Exception ex) 
        {

        }
    }
}