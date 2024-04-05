using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace tareaBases2.Pages.Project.Employees
{
    public class IndexModel : PageModel
    {
        public connection conexion = new connection();

        public void OnGet()
        {
            conexion.connectionTable();
        }
    }
}