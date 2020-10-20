using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    class BoardDalController : DalController
    {
        private const string BoardTableName = "Board";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Empty Consturctor, using the father constructor
        /// </summary>
        public BoardDalController() : base(BoardTableName)
        {

        }

        /// <summary>
        /// return all users boards
        /// </summary>
        /// <returns>list of all the boards from sql </returns>
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();

            return result;
        }

        /// <summary>
        /// return logged in user's boards
        /// </summary>
        /// <param name="email"></param>
        /// <returns>list of user's boards from sql</returns>
        public List<BoardDTO> SelectAllBoards(string email)
        {
            List<BoardDTO> result = Select(email).Cast<BoardDTO>().ToList();

            return result;
        }

        /// <summary>
        /// inserting a new board to the data base
        /// </summary>
        /// <param name="board"></param>
        /// <returns>true if the isertion succeded and false if wasnt</returns>
        public bool Insert(BoardDTO board)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({BoardDTO.EmailColumnName} ,{BoardDTO.BoardIDColumnName}, {BoardDTO.TaskIdCountColumnName}) " +
                        $"VALUES (@emailVal,@boardidVal,@taskidcountVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", board.Email);
                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardidVal", board.boardID);
                    SQLiteParameter taskidcountParam = new SQLiteParameter(@"taskidcountVal", board.taskIdCount);
                    

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(boardidParam);
                    command.Parameters.Add(taskidcountParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
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
        /// <returns>BoardDTO with the same data from sql</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result;
            try
            {
                result = new BoardDTO(reader.GetString(0), (long)reader.GetValue(1), (long)reader.GetValue(2));
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
