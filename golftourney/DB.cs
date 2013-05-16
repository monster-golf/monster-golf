using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace MonsterGolf
{
   class DB
   {
      public static DataSet GetDataSet(string sql)
      {
         // EXAMPLE Connection String: Provider=Microsoft.Jet.OLEDB.4.0;Data Source=MonsterScoring.mdb
         OleDbConnection conn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["DBConnection"]);
         conn.Open();
         OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
         DataSet ds = new DataSet();
         da.Fill(ds);
         da.Dispose();
         conn.Close();
         return ds;
      }

      public static void Execute(string sql)
      {
         // EXAMPLE Connection String: Provider=Microsoft.Jet.OLEDB.4.0;Data Source=MonsterScoring.mdb
         OleDbConnection conn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["DBConnection"]);
         conn.Open();
         OleDbCommand command = new OleDbCommand(sql, conn);
         command.ExecuteNonQuery();
         command.Dispose();
         conn.Close();
      }

      public static bool IsEmpty(DataSet ds)
      {
          return ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0;
      }
      public static string Str(DataRow dr, string colName)
      {
          if (dr.IsNull(colName)) return "";
          else return dr[colName].ToString();
      }
      public static string ExecStr(string str)
      {
          return str.Replace("'", "''");
      }
   }

   class WEBDB
   {
       public static DataSet GetDataSet(string sql)
       {
           SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["WEBDBConnection"]);
           conn.Open();
           SqlDataAdapter da = new SqlDataAdapter(sql, conn);
           DataSet ds = new DataSet();
           da.Fill(ds);
           da.Dispose();
           conn.Close();
           return ds;
       }
       public static void Execute(string sql)
       {
           SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["WEBDBConnection"]);
           conn.Open();
           SqlCommand command = new SqlCommand(sql, conn);
           command.ExecuteNonQuery();
           command.Dispose();
           conn.Close();
       }
   }

}
