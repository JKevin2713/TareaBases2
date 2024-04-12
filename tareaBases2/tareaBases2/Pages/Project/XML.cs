
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class XML
{
    public void Cargar()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Pages/Project/datos.xml");

        List<Puestos> listaPuestos = new List<Puestos>();
        List<TiposEvento> listaTipoEvento = new List<TiposEvento>();
        List<TiposMovimientos> listaTiposMovimientos = new List<TiposMovimientos>();
        List<Error> listaError = new List<Error>();
        List<infoEmpleyee> listaEmpleyees = new List<infoEmpleyee>();
        List<Usuarios> listaUsuarios = new List<Usuarios>();
        List<Movimientos> listaMovimientos = new List<Movimientos>();

        foreach (XmlNode node in doc.DocumentElement.ChildNodes)
        {

            if (node.HasChildNodes && node.Name == "Puestos")
            {
                //Console.WriteLine($"{node.Name}");
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
                    //Console.WriteLine(puesto);
                }
            }
            else if (node.HasChildNodes && node.Name == "TiposEvento")
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    int id = int.Parse(node2.Attributes["Id"].InnerText);
                    string NombreT = node2.Attributes["Nombre"].InnerText;
                    listaTipoEvento.Add(new TiposEvento { Id = id, Nombre = NombreT });
                }
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
            }
            else if (node.HasChildNodes && node.Name == "Error")
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    int Codigo = int.Parse(node2.Attributes["Codigo"].InnerText);
                    string Descripcion = node2.Attributes["Descripcion"].InnerText;
                    listaError.Add(new Error { Codigo = Codigo, Descripcion = Descripcion });

                }
            }
            else if (node.HasChildNodes && node.Name == "Empleados")
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    string Puesto = node2.Attributes["Puesto"].InnerText;
                    int ValorDocumentoIdentidad = int.Parse(node2.Attributes["ValorDocumentoIdentidad"].InnerText);
                    string Nombre = node2.Attributes["Nombre"].InnerText;
                    string Fecha = node2.Attributes["FechaContratacion"].InnerText;
                    DateTime FechaContratacion = DateTime.ParseExact(Fecha, "yyyy-MM-dd", null);
                    Console.WriteLine("Fecha C: {0}", FechaContratacion);

                    listaEmpleyees.Add(new infoEmpleyee { idPuesto = Puesto, ValorDocumentoIdentidad = ValorDocumentoIdentidad, Nombre = Nombre, FechaContratacion = FechaContratacion.Date });
                    //Console.WriteLine(FechaContratacion.Date);
                }
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

            }
            else if (node.HasChildNodes && node.Name == "Movimientos")
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    int ValorDocId = int.Parse(node2.Attributes["ValorDocId"].InnerText);
                    string IdTipoMovimiento = node2.Attributes["IdTipoMovimiento"].InnerText;
                    string Fecha2 = node2.Attributes["Fecha"].InnerText;
                    DateTime FechaAux = DateTime.ParseExact(Fecha2, "yyyy-MM-dd", null);

                    Console.WriteLine("Fecha C: {0}", FechaAux);

                    string Monto = node2.Attributes["Monto"].InnerText;
                    decimal MontoAux;
                    if (decimal.TryParse(Monto, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out MontoAux))
                    {
                    }
                    string PostByUser = node2.Attributes["PostByUser"].InnerText;
                    string PosInIP = node2.Attributes["PostInIP"].InnerText;
                    string PostTime = node2.Attributes["PostTime"].InnerText;
                    DateTime PostTimeAux;
                    if (DateTime.TryParseExact(PostTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out PostTimeAux))
                    {
                    }
                    Console.WriteLine("Fecha A: {0}", PostTimeAux);
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

            }


        }
        foreach (var elemento in listaMovimientos)
        {

        }

    }

    public class infoEmpleyee
    {
        public string idPuesto { get; set; }
        public int ValorDocumentoIdentidad { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaContratacion { get; set; }

    }

    public class Puestos
    {
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
        public int ValorDocId { get; set; }
        public string IdTipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string PostByUser { get; set; }
        public string PostInIP { get; set; }
        public DateTime PostTime { get; set; }
    }
}
