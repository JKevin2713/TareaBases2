﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace tareaBases2.Pages.Project;
public class jobConnection
{
    public List<jobs> ListJobs = new List<jobs>();

    public void conexion()
    {
        try
        {
            string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea2;Integrated Security=True;Encrypt=False";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string sqlReadPuesto = "SELECT * FROM Puesto";

                using (SqlCommand command = new SqlCommand(sqlReadPuesto, sqlConnection))
                {
                    using (SqlDataReader readerPuesto = command.ExecuteReader())
                    {
                        while (readerPuesto.Read())
                        {
                            jobs infoJobs = new jobs();
                            infoJobs.id = readerPuesto.GetInt32(0);
                            infoJobs.NombrePuesto = readerPuesto.GetString(1);
                            infoJobs.SalarioxHora = readerPuesto.GetDecimal(2);

                            ListJobs.Add(infoJobs);
                            Console.Write(infoJobs.SalarioxHora);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public List<jobs> ObtenerListaDeTrabajos()
    {
        return ListJobs;
    }
}
public class jobs
{
    public Int32 id;
    public string NombrePuesto;
    public decimal SalarioxHora;
}