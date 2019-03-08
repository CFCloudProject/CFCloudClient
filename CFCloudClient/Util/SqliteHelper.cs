using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Util
{
    public class SqliteHelper
    {
        private static SQLiteConnection conn;
        private static string databaseFileName;

        public static void Init(string path)
        {
            databaseFileName = path + ".db";
            if (File.Exists(databaseFileName))
                File.Delete(databaseFileName);
            SQLiteConnection.CreateFile(databaseFileName);
            conn = new SQLiteConnection("Data Source=" + databaseFileName);
            conn.Open();
            string sql = "create table files (Path varchar, ModifiedTime varchar, Version varchar, Modifier varchar, IsShared varchar)";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Connect(string path)
        {
            databaseFileName = path + ".db";
            conn = new SQLiteConnection("Data Source=" + databaseFileName);
            conn.Open();
        }

        public static void Close()
        {
            conn.Close();
        }

        public static void Insert(Models.SQLDataType sdt)
        {
            lock (conn)
            {
                if (Select(sdt.Path) != null)
                    Update(sdt);
                else
                {
                    string sql = "insert into files (Path, ModifiedTime, Version, Modifier, IsShared) values + (\'" 
                        + sdt.Path + "\', \'" 
                        + sdt.ModifiedTime + "\', \'" 
                        + sdt.Version + "\', \'" 
                        + sdt.Modifier + "\', \'"
                        + sdt.IsShared + "\')";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Models.SQLDataType Select(string path)
        {
            lock (conn)
            {
                string sql = "select * from files where Path = \'" + path + "\'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Models.SQLDataType result = new Models.SQLDataType(
                        reader["Path"].ToString(),
                        reader["ModifiedTime"].ToString(),
                        reader["Version"].ToString(),
                        reader["Modifier"].ToString(),
                        reader["IsShared"].ToString());
                    return result;
                }
                else
                    return null;
            }
        }

        public static void Update(Models.SQLDataType sdt)
        {
            lock (conn)
            {
                string sql = "update files set ModifiedTime = \'" + sdt.ModifiedTime 
                    + "\', Version = \'" + sdt.Version 
                    + "\', Modifier = \'" + sdt.Modifier 
                    + "\', IsShared = \'" + sdt.IsShared
                    + "\' where Path = \'" + sdt.Path + "\'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(string path)
        {
            lock (conn)
            {
                if (Select(path) == null)
                    return;
                string sql = "delete from files where Path = \'" + path + "\'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
