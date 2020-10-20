using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System;


namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{

    public abstract class DalController
    {
        protected readonly string connectionString;
        protected readonly string tableName;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// creating an new DAL Controller, creat DB in working directory if needed
        /// </summary>
        /// <param name="tableName"></param>
        public DalController(string tableName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db");
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = tableName;
        
        }
       
        /// <summary>
        /// init a new DB
        /// </summary>
        /// <returns>true if succeded and false if failed</returns>
        public bool InitiallizeDB()
        {
            int res = -1;
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"))) // Init only if DB not exist.
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    try
                    {
                        SQLiteConnection.CreateFile("KanbanDB.db");
                        connection.Open();
                        string UsersTable = "CREATE TABLE Users( Email TEXT NOT NULL UNIQUE, Password  TEXT NOT NULL, Nickname  TEXT NOT NULL, HostEmail	TEXT NOT NULL, PRIMARY KEY(Email));";
                        string BoardTable = "CREATE TABLE Board (Email TEXT NOT NULL, BoardID INTEGER NOT NULL, TaskIdCount	INTEGER, PRIMARY KEY(BoardID,Email), FOREIGN KEY(Email) REFERENCES Users(Email) ON DELETE CASCADE);";
                        string ColumnsTable = "CREATE TABLE Columns (Email TEXT NOT NULL, Name TEXT NOT NULL, ColumnOrdinal INTEGER NOT NULL, ColumnLimit	INTEGER NOT NULL, BoardID	INTEGER, FOREIGN KEY(Email,BoardID) REFERENCES Board(Email,BoardID) ON DELETE CASCADE, PRIMARY KEY(ColumnOrdinal,Email,BoardID));";
                        string TasksTable = "CREATE TABLE Tasks ( Email	TEXT NOT NULL, ColumnOrdinal	INTEGER NOT NULL, TaskID	INTEGER NOT NULL, Title	TEXT NOT NULL, Description	TEXT, DueDate	TEXT NOT NULL, CreationTime	TEXT NOT NULL, BoardID	INTEGER NOT NULL,EmailAssignee	TEXT NOT NULL, FOREIGN KEY(BoardID,ColumnOrdinal,Email) REFERENCES Columns(BoardID,ColumnOrdinal,Email) ON DELETE CASCADE ON UPDATE CASCADE, PRIMARY KEY(Email,ColumnOrdinal,TaskID,BoardID));";

                        command.CommandText = UsersTable + BoardTable + ColumnsTable + TasksTable;
                        command.Prepare();
                        res = command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        log.Error("Failed to create relevant tables in the DB. error thrown is: " + e.Message);
                        throw e;

                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }

                }
            }
            return res > 0;
        }
        
        /// <summary>
        /// creat the specification for each elemnt
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="columns"></param>
        /// <returns>where string for the sql quary</returns>
        public string MultipleWhereSQL(string[] keys, string[] columns)
        {
            string output = "where ";
            for (int i = 0; i < keys.Length; i++)
            {
                output = output + $"{columns[i]}=\'{keys[i]}\'";
                if (i < keys.Length - 1)
                    output = output + " and ";
            }
            return output;
        }
        
        /// <summary>
        /// update string attribute in sql
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="columnNames"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns>true if succeded and false if failed</returns>
        public bool Update(string[] keys, string[] columnNames, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"PRAGMA foreign_keys = ON; UPDATE {tableName} set [{attributeName}]=@updatedVal {MultipleWhereSQL(keys, columnNames)}";
                    SQLiteParameter updateParam = new SQLiteParameter(@"updatedVal", attributeValue);
                    command.Parameters.Add(updateParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Failed to update in {tableName}. tried to set {attributeValue} in {attributeName}. error thrown is: " + e.Message);
                    throw e;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        
        /// <summary>
        /// update long attirbute in sql
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="columnNames"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns>true if succeded and false if failed</returns>
        public bool Update(string[] keys, string[] columnNames, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
  
                try
                {
                    connection.Open();
                    command.CommandText = $"PRAGMA foreign_keys = ON; UPDATE {tableName} set [{attributeName}]=@updatedVal {MultipleWhereSQL(keys, columnNames)}";
                    SQLiteParameter updateParam = new SQLiteParameter(@"updatedVal", attributeValue);
                    command.Parameters.Add(updateParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Failed to update in {tableName}. tried to set {attributeValue} in {attributeName}. error thrown is: " + e.Message);
                    throw e;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception e)
                {
                    log.Error($"Failed to select from {tableName}. error thrown is: " + e.Message);
                    throw e;
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        protected List<DTO> Select(string email)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {tableName} WHERE {DTO.EmailColumnName} = '{email}';";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception e)
                {
                    log.Error($"Failed to select from {tableName} where email is {email}. error thrown is: " + e.Message);
                    throw e;
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        /// <summary>
        /// Delete specific elemnt from sql
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="columnNames"></param>
        /// <returns>true if succeded and false if failed</returns>
        public bool Delete(string[] keys, string [] columnNames)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} {MultipleWhereSQL(keys, columnNames)}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Failed to delete from {tableName}. Exception message" + e.Message);
                    throw e;
       
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        
        /// <summary>
        /// Delete all data from sql
        /// </summary>
        /// <returns>true if succeded and false if failed</returns>
        public bool DeleteAllData()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"PRAGMA foreign_keys = ON; delete from {tableName}"
                };
              
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Failed to delete all Data. Exception message" + e.Message);
                    throw e;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

    }
}