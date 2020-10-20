using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    class UserDalController : DalController
    {
        private const string UsersTableName = "Users";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Empty Consturctor, using the father constructor
        /// </summary>
        public UserDalController() : base(UsersTableName) { }
        
        /// <summary>
        /// return all users data
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List of tasks from sql</returns>
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();

            return result;
        }
        
        /// <summary>
        /// inserting a new user to the data base
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if the isertion succeded and false if wasnt</returns>
        public bool Insert(UserDTO user)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);

           
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({UserDTO.EmailColumnName}, {UserDTO.PasswordColumnName}, {UserDTO.NicknameColumnName}, {UserDTO.HostEmailColumnName}) " +
                        $"VALUES (@emailVal,@passwordVal,@nicknameVal,@hostEmailVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    SQLiteParameter nicknameParam = new SQLiteParameter(@"nicknameVal", user.Nickname);
                    SQLiteParameter hostEmailParam = new SQLiteParameter(@"hostEmailVal", user.HostEmail);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(nicknameParam);
                    command.Parameters.Add(hostEmailParam);
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
        /// <returns>UserDTO with the same data from sql</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result;
            try
            {
                result = new UserDTO(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
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
