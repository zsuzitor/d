using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class Constants
    {
        //бд проекта
        public const string sql_0 = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\aspnet-dip-20181007084703.mdf;Integrated Security=True";

        // сервер 2014 локал бд
        public const string sql_1 = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SOFI.mdf;Integrated Security=True";

        // сервер 2017++ 
        public const string sql_2 = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=C:\rub\d_bd\1\TechnicalFunctions.mdf;Integrated Security=True;User Instance=False";


        public const  int CountForLoad=10;


    }
}