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
        public List<infoEmpleyee> ResultadoFiltrado = new List<infoEmpleyee>();
        public List<infoEmpleyee> listaFiltrada = new List<infoEmpleyee>();

        public void OnGet()
        {
            conexion.connectionTable();

        }
        
        public void OnPost(string buscar)
        {
            // Lógica para filtrar con el valor recibido
            conexion.connectionTable();
            ResultadoFiltrado = conexion.listEmployee.Where(e => e.Nombre.Contains(buscar)).ToList();
            Console.WriteLine(ResultadoFiltrado.Count);
        }
        public void jee(string buscar)
        {
            conexion.connectionTable(); 
            Console.WriteLine(buscar);
            if (validarBusquedaNombre(buscar) == true)
            { 
                foreach (infoEmpleyee i in conexion.listEmployee)
                {
                    if(i.Nombre == buscar)
                    {
                        listaFiltrada.Add(i);
                        Console.WriteLine(i.Nombre);
                    }
                }
            }
            else if(validarBusquedaCedula(buscar) == true)
            {
                int auxBuscar = int.Parse(buscar);
                foreach (infoEmpleyee i in conexion.listEmployee)
                {
                    if (i.Identificacion == auxBuscar)
                    {
                        listaFiltrada.Add(i);
                        Console.WriteLine(i.Identificacion);
                    }
                }
            }
            else
            {
               OnGet();
            }
        }
        public bool validarBusquedaNombre(string buscar)
        {
            // Verificar que ambos campos no estén vacíos
            if (buscar.Length != 0)
            {
                if (Regex.IsMatch(buscar, @"^[a-zA-Z\s]+$"))
                {
                    return true;
                }
            }
            return false;
        }
        public bool validarBusquedaCedula(string buscar)
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

    }
}