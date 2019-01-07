using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{
    public class DataBase
    {

        public static void ExecuteNonQuery(string query)
        {
            var connection = new SqlConnection();
            connection.ConnectionString = Constants.sql_0;
            connection.Open();
           
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            
            connection.Close();
        }

        public static List<Dictionary<string, object>> ExecuteQuery(string query, params string[] props)
        {
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();


            var connection1 = new SqlConnection() { ConnectionString= Constants.sql_0 };
            var command1 = new SqlCommand() { Connection = connection1, CommandType = CommandType.Text };

            connection1.Open();
            command1.CommandText = query;
            using (SqlDataReader reader = command1.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (var i in props)
                            dict.Add(i, reader[i]);
                        res.Add(dict);
                    }
                        
               
            return res;
        }


    }
}