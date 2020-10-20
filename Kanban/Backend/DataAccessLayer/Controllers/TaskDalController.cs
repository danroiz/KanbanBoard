using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    class TaskDalController : Controllers.DalController
    {
        private const string TasksTableName = "Tasks";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Empty Consturctor, using the father constructor
        /// </summary>
        public TaskDalController() : base(TasksTableName) { }

        /// <summary>
        /// return all users tasks
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List of tasks from sql</returns>
        public List<TaskDTO> SelectAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();

            return result;
        }

        /// <summary>
        /// return logged in user's tasks
        /// </summary>
        /// <returns>list of all the tasks from sql </returns>
        public List<TaskDTO> SelectAllTasks(string email)
        {
            List<TaskDTO> result = Select(email).Cast<TaskDTO>().ToList();

            return result;
        }
        
        /// <summary>
        /// inserting a new task to the data base
        /// </summary>
        /// <param name="task"></param>
        /// <returns>true if the isertion succeded and false if wasnt</returns>
        public bool Insert(TaskDTO task)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName}({TaskDTO.EmailColumnName}, {TaskDTO.ColumnOrdinalColumnName}, {TaskDTO.IDColumnName}, {TaskDTO.TitleColumnName}," +
                        $"{TaskDTO.DescriptionColumnName}, {TaskDTO.DueDateColumnName}, {TaskDTO.CreationTimeColumnName}, {TaskDTO.BoardIDColumnName}, {TaskDTO.EmailAssigneeColumnName}) "
                        + $"VALUES (@emailVal,@columnOrdinalVal,@idVal,@titleVal,@descriptionVal,@dueDateVal,@creationTimeVal,@boardVal,@emailAssigneeVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", task.Email);
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.Id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", task.ColumnOrdinal);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", task.CreationTime);
                    SQLiteParameter boardParam = new SQLiteParameter(@"boardVal", task.BoardID);
                    SQLiteParameter emailAssigneeParam = new SQLiteParameter(@"emailAssigneeVal", task.EmailAssignee);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(boardParam);
                    command.Parameters.Add(emailAssigneeParam);

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
        /// <returns>TaskDTO with the same data from sql</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result;
            try
            {
                result = new TaskDTO(reader.GetString(0), (long)reader.GetValue(1), (long)reader.GetValue(2),
                reader.GetString(3), reader.GetValue(4).ToString(), DateTime.Parse(reader.GetString(5)), DateTime.Parse(reader.GetString(6)), (long)reader.GetValue(7), reader.GetString(8));
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
