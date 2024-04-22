
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class XML
{
    public void CargarPuestos(List<Puestos> listaPuestos, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarPuestos @InNombre, @InSalarioxHora, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var puesto in listaPuestos)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InNombre", puesto.Nombre);
                    command.Parameters.AddWithValue("@InSalarioxHora", puesto.SalarioxHora);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }



    public void CargarTipoEvento(List<TiposEvento> listaTiposEvento, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarTipoEvento @InId, @InNombre, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var tipoEvento in listaTiposEvento)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InId", tipoEvento.Id);
                    command.Parameters.AddWithValue("@InNombre", tipoEvento.Nombre);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }


    public void CargarTipoMovimiento(List<TiposMovimientos> listaTiposMovimientos, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarTipoMovimiento @InId, @InNombre, @InTipoAccion, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var tipoMovimiento in listaTiposMovimientos)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InId", tipoMovimiento.Id);
                    command.Parameters.AddWithValue("@InNombre", tipoMovimiento.Nombre);
                    command.Parameters.AddWithValue("@InTipoAccion", tipoMovimiento.TipoAccion);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }


    public void CargarError(List<Error> listaErrores, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarError @InCodigo, @InDescripcion, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var error in listaErrores)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InCodigo", error.Codigo);
                    command.Parameters.AddWithValue("@InDescripcion", error.Descripcion);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }


    public int ObtenerIdPuesto(string nombrePuesto, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC ObtenerIdPuesto @InNombre, @OutId OUTPUT, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InNombre", nombrePuesto);
                SqlParameter outIdParam = new SqlParameter("@OutId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outIdParam);
                SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outParam);

                command.ExecuteNonQuery();

                int resultCode = (int)command.Parameters["@OutResulTCode"].Value;

                if (resultCode == 0)
                {
                    return (int)command.Parameters["@OutId"].Value;
                }
                else
                {
                    throw new Exception("No se encontró un puesto con el nombre especificado.");
                }
            }
        }
    }


    public void CargarEmpleado(List<infoEmpleyee> listaEmpleyees, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarEmpleado @InIdPuesto, @InValorDocumentoIdentidad, @InNombre, @InFechaContratacion, @InSaldoVacaciones, @InEsActivo, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var empleado in listaEmpleyees)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InIdPuesto", empleado.IdPuesto);
                    command.Parameters.AddWithValue("@InValorDocumentoIdentidad", empleado.ValorDocumentoIdentidad);
                    command.Parameters.AddWithValue("@InNombre", empleado.Nombre);
                    command.Parameters.AddWithValue("@InFechaContratacion", empleado.FechaContratacion);
                    command.Parameters.AddWithValue("@InSaldoVacaciones", empleado.SaldoVacaciones);
                    command.Parameters.AddWithValue("@InEsActivo", empleado.EsActivo);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }


    public void CargarUsuario(List<Usuarios> listaUsuarios, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarUsuario @InId, @InUsername, @InPassword, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var usuario in listaUsuarios)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InId", usuario.Id);
                    command.Parameters.AddWithValue("@InUsername", usuario.Nombre);
                    command.Parameters.AddWithValue("@InPassword", usuario.Pass);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }


    public int ObtenerIdTipoMovimiento(string nombreTipoMovimiento, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC ObtenerIdTipoMovimiento @InNombre, @OutId OUTPUT, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InNombre", nombreTipoMovimiento);
                SqlParameter outIdParam = new SqlParameter("@OutId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outIdParam);
                SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outParam);

                command.ExecuteNonQuery();

                int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                if (resultCode == 0)
                {
                    return (int)command.Parameters["@OutId"].Value;
                }
                else
                {
                    throw new Exception("No se encontró un tipo de movimiento con el nombre especificado.");
                }
            }
        }
    }

    public int ObtenerPostByUser(string nombreUsuario, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC ObtenerIdUsuario @InUsername, @OutPostByUser OUTPUT, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InUsername", nombreUsuario);
                SqlParameter outPostByUserParam = new SqlParameter("@OutPostByUser", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outPostByUserParam);
                SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outParam);

                command.ExecuteNonQuery();

                int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                if (resultCode == 0)
                {
                    return (int)command.Parameters["@OutPostByUser"].Value;
                }
                else
                {
                    throw new Exception("No se encontró un usuario con el nombre especificado.");
                }
            }
        }
    }

    public void CargarMovimiento(List<Movimientos> listaMovimientos, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CargarMovimiento @InValorDocId, @InIdTipoMovimiento, @InFecha, @InMonto, @InNuevoSaldo, @InPostByUser, @InPostInIp, @InPostTime, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var movimiento in listaMovimientos)
                {
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@InValorDocId", movimiento.ValorDocId);
                    command.Parameters.AddWithValue("@InIdTipoMovimiento", movimiento.IdTipoMovimiento);
                    command.Parameters.AddWithValue("@InFecha", movimiento.Fecha);
                    command.Parameters.AddWithValue("@InMonto", movimiento.Monto);
                    command.Parameters.AddWithValue("@InNuevoSaldo", movimiento.NuevoSaldo);
                    command.Parameters.AddWithValue("@InPostByUser", movimiento.PostByUser);
                    command.Parameters.AddWithValue("@InPostInIp", movimiento.PostInIP);
                    command.Parameters.AddWithValue("@InPostTime", movimiento.PostTime);
                    SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                    // Aquí puedes manejar el resultCode según sea necesario
                }
            }
        }
    }


    public bool ObtenerEstado(string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC ObtenerEstadoCarga @OutEstado OUTPUT, @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                SqlParameter outEstadoParam = new SqlParameter("@OutEstado", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outEstadoParam);
                SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outParam);

                command.ExecuteNonQuery();

                int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                if (resultCode == 0)
                {
                    return (bool)command.Parameters["@OutEstado"].Value;
                }
                else
                {
                    throw new Exception("No se encontró ningún registro en la tabla.");
                }
            }
        }
    }

    public void CambiarEstado(string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = "EXEC CambiarEstadoCarga @OutResulTCode OUTPUT";
            using (var command = new SqlCommand(query, connection))
            {
                SqlParameter outParam = new SqlParameter("@OutResulTCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outParam);

                command.ExecuteNonQuery();

                int resultCode = (int)command.Parameters["@OutResulTCode"].Value;
                // Aquí puedes manejar el resultCode según sea necesario
            }
        }
    }





    public void Cargar()
    {
        List<Puestos> listaPuestos = new List<Puestos>();
        List<TiposEvento> listaTipoEvento = new List<TiposEvento>();
        List<TiposMovimientos> listaTiposMovimientos = new List<TiposMovimientos>();
        List<Error> listaError = new List<Error>();
        List<infoEmpleyee> listaEmpleyees = new List<infoEmpleyee>();
        List<Usuarios> listaUsuarios = new List<Usuarios>();
        List<Movimientos> listaMovimientos = new List<Movimientos>();

        string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

        XmlDocument doc = new XmlDocument();
        doc.Load("Pages/Project/datos.xml");

        bool Estado = ObtenerEstado(connectionString);

        if (Estado == false)
        {
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {

                if (node.HasChildNodes && node.Name == "Puestos")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        string puesto = node2.Attributes["Nombre"].InnerText;
                        string SalarioT = node2.Attributes["SalarioxHora"].InnerText;
                        //Console.WriteLine(SalarioT);
                        decimal Salario;
                        if (decimal.TryParse(SalarioT, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Salario))
                        {
                            listaPuestos.Add(new Puestos { Nombre = puesto, SalarioxHora = Salario });
                        }
                    }
                    CargarPuestos(listaPuestos, connectionString);

                }
                else if (node.HasChildNodes && node.Name == "TiposEvento")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        int id = int.Parse(node2.Attributes["Id"].InnerText);
                        string NombreT = node2.Attributes["Nombre"].InnerText;
                        listaTipoEvento.Add(new TiposEvento { Id = id, Nombre = NombreT });
                    }
                    CargarTipoEvento(listaTipoEvento, connectionString);

                }
                else if (node.HasChildNodes && node.Name == "TiposMovimientos")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        int id = int.Parse(node2.Attributes["Id"].InnerText);
                        string Nombre = node2.Attributes["Nombre"].InnerText;
                        string TipoAccion = node2.Attributes["TipoAccion"].InnerText;
                        listaTiposMovimientos.Add(new TiposMovimientos { Id = id, Nombre = Nombre, TipoAccion = TipoAccion });
                    }
                    CargarTipoMovimiento(listaTiposMovimientos, connectionString);

                }
                else if (node.HasChildNodes && node.Name == "Error")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        int Codigo = int.Parse(node2.Attributes["Codigo"].InnerText);
                        string Descripcion = node2.Attributes["Descripcion"].InnerText;
                        listaError.Add(new Error { Codigo = Codigo, Descripcion = Descripcion });

                    }
                    CargarError(listaError, connectionString);

                }
                else if (node.HasChildNodes && node.Name == "Empleados")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        string PuestoNombre = node2.Attributes["Puesto"].InnerText;

                        int Puesto = ObtenerIdPuesto(PuestoNombre, connectionString);

                        int ValorDocumentoIdentidad = int.Parse(node2.Attributes["ValorDocumentoIdentidad"].InnerText);
                        string Nombre = node2.Attributes["Nombre"].InnerText;
                        string Fecha = node2.Attributes["FechaContratacion"].InnerText;
                        DateTime FechaContratacion = DateTime.ParseExact(Fecha, "yyyy-MM-dd", null);
                        int saldo = 0;
                        int Activo = 1;

                        listaEmpleyees.Add(new infoEmpleyee { IdPuesto = Puesto, ValorDocumentoIdentidad = ValorDocumentoIdentidad, Nombre = Nombre, FechaContratacion = FechaContratacion.Date, SaldoVacaciones = saldo, EsActivo = Activo });

                    }

                    CargarEmpleado(listaEmpleyees, connectionString);
                }
                else if (node.HasChildNodes && node.Name == "Usuarios")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        int Id = int.Parse(node2.Attributes["Id"].InnerText);
                        string Nombre = node2.Attributes["Nombre"].InnerText;
                        string Pass = node2.Attributes["Pass"].InnerText;
                        listaUsuarios.Add(new Usuarios { Id = Id, Nombre = Nombre, Pass = Pass });

                    }

                    CargarUsuario(listaUsuarios, connectionString);

                }
                else if (node.HasChildNodes && node.Name == "Movimientos")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        int ValorDocId = int.Parse(node2.Attributes["ValorDocId"].InnerText);
                        string IdTipoMovimientoNombre = node2.Attributes["IdTipoMovimiento"].InnerText;

                        int IdTipoMovimiento = ObtenerIdTipoMovimiento(IdTipoMovimientoNombre, connectionString);

                        string Fecha2 = node2.Attributes["Fecha"].InnerText;
                        DateTime FechaAux = DateTime.ParseExact(Fecha2, "yyyy-MM-dd", null);

                        string Monto = node2.Attributes["Monto"].InnerText;
                        decimal MontoAux;
                        if (decimal.TryParse(Monto, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out MontoAux))
                        {
                        }

                        string PostByUserAux = node2.Attributes["PostByUser"].InnerText;
                        int PostByUser = ObtenerPostByUser(PostByUserAux, connectionString);
                        string PosInIP = node2.Attributes["PostInIP"].InnerText;
                        string PostTime = node2.Attributes["PostTime"].InnerText;
                        DateTime PostTimeAux;
                        if (DateTime.TryParseExact(PostTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out PostTimeAux))
                        {
                        }

                        listaMovimientos.Add(new Movimientos
                        {
                            ValorDocId = ValorDocId,
                            IdTipoMovimiento = IdTipoMovimiento,
                            Fecha = FechaAux,
                            Monto = MontoAux,
                            PostByUser = PostByUser,
                            PostInIP = PosInIP,
                            PostTime = PostTimeAux
                        });
                    }
                    CargarMovimiento(listaMovimientos, connectionString);
                }
            }
            CambiarEstado(connectionString);
        }
        else
        {
            Console.WriteLine("Ya se cargaron");
        }
    }
}
public class infoEmpleyee
{
    public int Id { get; set; }
    public int IdPuesto { get; set; }
    public int ValorDocumentoIdentidad { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaContratacion { get; set; }
    public int SaldoVacaciones { get; set; }
    public int EsActivo { get; set; }
}

public class Puestos
{
    //public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal SalarioxHora { get; set; }
}

public class TiposEvento
{
    public int Id { get; set; }
    public string Nombre { get; set; }
}

public class TiposMovimientos
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string TipoAccion { get; set; }
}

public class Error
{
    public int Id { get; set; }
    public int Codigo { get; set; }
    public string Descripcion { get; set; }
}

public class Usuarios
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Pass { get; set; }
}

public class Movimientos
{
    public int Id { get; set; }
    public int ValorDocId { get; set; }
    public int IdTipoMovimiento { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public decimal NuevoSaldo { get; set; }
    public int PostByUser { get; set; }
    public string PostInIP { get; set; }
    public DateTime PostTime { get; set; }
}
