using Microsoft.Data.SqlClient;
using System;

namespace subcats.customClass
{
    public class Conection
    {
        public SqlConnection connection;
        //public SqlConnection connectionSms;
        public SqlCommand sqlCommand;
        public SqlDataReader sqlDataReader;
        public Conection()
        {

            string variableValue = Environment.GetEnvironmentVariable("LocalSQL");
            var machine = System.Environment.MachineName;

            connection = new SqlConnection(variableValue);
            //if (machine != "VMACDE1B5")
            //{
            //    variableValue = Environment.GetEnvironmentVariable("DEVELOPER");
            //    connection = new SqlConnection(variableValue);
            //}


        }
    }
}


