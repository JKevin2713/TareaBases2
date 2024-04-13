public class empleyee
{
    public Int32 id;
    public Int32 idPuesto;
    public int Identificacion;
    public string Nombre;
    public DateTime FechaContratacion;
    public decimal SaldoVaciones;
    public bool EsActivo;

}
public class jobs
{
    public Int32 id;
    public string NombrePuesto;
    public decimal SalarioxHora;
}

public class movements
{
    public Int32 id;
    public Int32 ValorDocId;
    public Int32 IdTipoMovimiento;
    public DateTime Fecha;
    public decimal Monto;
    public decimal NuevoSaldo;
    public Int32 PostByUser;
    public string PostInIp;
    public DateTime PostTime;
}

public class tipoMovimiento
{
    public Int32 id;
    public string Nombre;
    public string TipoAccion;
}

public class usuario
{
    public Int32 id;
    public string Username;
    public string Password;
}