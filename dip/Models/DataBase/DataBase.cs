﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{
    public class DataBase
    {


        /// <summary>
        /// выполнение tsql запроса который ничего не возвращает
        /// </summary>
        /// <param name="query - будет использоваться только если command_==null"></param>
        /// <param name="command_ - если не null то не закрывается"></param>
        public static void ExecuteNonQuery(string query, SqlCommand command_ = null)
        {
            var command = command_;
            if (command_ == null)
            {
                var connection = new SqlConnection() { ConnectionString = Constants.sql_0 };
                connection.Open();
                command = new SqlCommand(query, connection);

            }
            command.ExecuteNonQuery();

            if (command_ == null)
            {
                command.Connection.Close();
                command.Dispose();
            }


            ////using (var command = command_?? new SqlCommand(query, connection))// new SqlCommand(query, connection))
            //var command = command_ ?? new SqlCommand(query, connection);
            //    command.ExecuteNonQuery();

            //if (connection_ == null)
            //    connection.Close();
        }

        /// <summary>
        /// выполнение tsql запроса(например выборка данных) который возвращает данные. при command_-null - откроет новое подключение
        /// </summary>
        /// <param name="query - будет использоваться только если command_==null"></param>
        /// <param name="command_ - если не null то не закрывается"></param>
        /// <param name="props - список свойств"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ExecuteQuery(string query,SqlCommand command_, params string[] props)
        {
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();


            var command = command_;
            if (command_ == null)
            {
                var connection = new SqlConnection() { ConnectionString = Constants.sql_0 };
                connection.Open();
                command = new SqlCommand(query, connection) { CommandType = CommandType.Text };

            }

            
            using (SqlDataReader reader = command.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (var i in props)
                            dict.Add(i, reader[i]);
                        res.Add(dict);
                    }
            if (command_ == null)
            {
                command.Connection.Close();
                command.Dispose();
            }

            return res;
        }


    }
}