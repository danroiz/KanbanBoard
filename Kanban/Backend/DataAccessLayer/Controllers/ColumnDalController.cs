using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    class ColumnDalController : Controllers.DalController
    {
        private const string ColumnsTableName = "Columns";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Empty Consturctor, using the father constructor
        /// </summary>
        public ColumnDalController() : base(ColumnsTableName)
        {

        }

        /// <summary>
        /// return logged in user's columns
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List of columns from sql</returns>
        public List<ColumnDTO> SelectAllColumns(string email)
        {
            List<ColumnDTO> result = Select(email).Cast<ColumnDTO>().ToList();

            return result;
        }
        
        /// <summary>
        /// return all users columns
        /// </summary>
        /// <returns>list of all the columns from sql </returns>
        public List<ColumnDTO> SelectAllColumns()
        {
            List<ColumnDTO> result = Select().Cast<ColumnDTO>().ToList();

            return result;
        }

        /// <summary>
        /// inserting a new board to the data base
        /// </summary>
        /// <param name="column"></param>
        /// <returns>true if the isertion succeded and false if wasnt</returns>
        public bool Insert(ColumnDTO column)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                   
           
                    command.CommandText = $"INSERT INTO {ColumnsTableName} ({ColumnDTO.EmailColumnName} ,{ColumnDTO.NameColumnName}, {ColumnDTO.ColumnOrdinalColumnName}, {ColumnDTO.LimitColumnName}, {ColumnDTO.BoardIDColumnName}) " +
                        $"VALUES (@emailVal,@nameVal,@ordinalVal,@limitVal,@boardVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", column.Email);                  
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", column.Name);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", column.ColumnOrdinal);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.Limit);
                    SQLiteParameter boardParam = new SQLiteParameter(@"boardVal", column.BoardID);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(boardParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Failed to insert to {tableName}. error thrown is: " + e.Message);
                    throw e;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        
        /// <summary>
        /// convert sql data to business object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>ColumnDTO with the same data from sql</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result;
            try
            {
                result = new ColumnDTO(reader.GetString(0), reader.GetString(1), (long)reader.GetValue(2), (long)reader.GetValue(3), (long)reader.GetValue(4));
            }
            catch (Exception e)
            {
                log.Error($"Failed to convert from {tableName}. error thrown is: " + e.Message);
                throw e;
            }

            return result;
 

        }
    }
}
