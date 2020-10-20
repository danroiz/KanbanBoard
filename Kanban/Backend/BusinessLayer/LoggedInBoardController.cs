using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// This is the object to control the current logged in user.
    /// It allows executing all the functions required from the logged in user. 
    /// </summary>
    class LoggedInBoardController
    {
        // ===================================FIELDS===========================================================
        private User loggedIn;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ===================================CONSTRUCTOR======================================================
        /// <summary>
        /// empty constructor
        /// </summary>
        internal LoggedInBoardController()
        {
            loggedIn = null;
        }

        // ===================================VERIFICATIONS====================================================
        /// <summary>
        /// privet method to varify that given email match the loggedIn email
        /// </summary>
        /// <param name="email">the email to comper</param>
        private void IsLoggedIn(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
            {
                log.Debug("input email is null or whitespace");
                throw new Exception("don't mess around with null input");
            }
            email = email.ToLower(); // our convention - user email's saved with lower case char's only
            if (loggedIn == null)
            {
                log.Debug("tried to do action while there is no user logged in");
                throw new Exception("you're not logged in");
            }
            
            else if (!email.Equals(loggedIn.Email))
            {
                log.Debug("tried to do action to a different user's board");
                throw new Exception("can not access other user's board");
            }
            
        }
        
        /// <summary>
        /// after varify the email and password,
        /// this function allows to loggin a user if one not looged in already
        /// </summary>
        /// <param name="user"> the user to loggin</param>
        internal void Login(User user)
        {
            if (loggedIn != null)
            {
                log.Info("tried to log in while there is someonelogged in");
                throw new Exception("Can't login more than one user at a time, logout first.");               
            }
            this.loggedIn = user;
            log.Info($"logged in user by email: {user.Email}");
        }

        /// <summary>
        ///  Log out an logged in user.
        ///  varify that the given email matches the currnet user email if exsist 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        internal void Logout(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
            {
                log.Debug("tried to log out with invalid email");
                throw new Exception("don't mess around with null input");
            }
            email = email.ToLower();
            if (loggedIn == null)
            {
                log.Debug("tried to log out while no user is logged in");
                throw new Exception("no user logged in");
            }
            else if (!loggedIn.Email.Equals(email))
            {
                log.Debug("tried to log out a different user");
                throw new Exception("can't logout other user");
            }
            
            loggedIn = null;            
            log.Info($"logged out user by email: {email}");
        }

        // ===================================METHODS==========================================================
        /// <summary>
        /// Returns the board of a user as string list. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>LinkedList that contains the columns name's</returns>
        internal LinkedList<string> GetBoard(string email)
        {
            IsLoggedIn(email);
            return loggedIn.Board.GetColumnNames();
        }

        /// <summary>
        /// After varify the email limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        internal void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            
            IsLoggedIn(email);
            loggedIn.Board.SetLimit(email, columnOrdinal, limit);
            
        }

        internal void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            IsLoggedIn(email);
            loggedIn.Board.AssignTask(email, columnOrdinal, taskId, emailAssignee);
        }

        /// <summary>
        /// After varify the email add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>The new Task that created</returns>
        internal Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            IsLoggedIn(email);
            Task tsk = loggedIn.Board.AddTask(title, description, dueDate, loggedIn.Email);
            log.Info($"added a new task to user by email: {email}");
            return tsk;
        }

        internal void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            IsLoggedIn(email);
            loggedIn.Board.DeleteTask(email, columnOrdinal, taskId);
        }

        /// <summary>
        /// After varify the email update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        internal void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            IsLoggedIn(email);           
            loggedIn.Board.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
            
        }

        /// <summary>
        /// After varify the email update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        internal void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            IsLoggedIn(email);           
            loggedIn.Board.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            log.Info($"Write to file, edited task: {taskId}");
        }

        /// <summary>
        /// After varify the email update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        internal void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            IsLoggedIn(email);           
            loggedIn.Board.UpdateTaskDescription(email, columnOrdinal, taskId, description);     
            log.Info($"Write to file, edited task: {taskId}");
        }

        internal string GetCreator()
        {
            return loggedIn.Board.Creator;
        }

        /// <summary>
        /// After varify the email advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        internal void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            IsLoggedIn(email);
            loggedIn.Board.AdvanceTask(columnOrdinal, taskId,email);          
            log.Info($"Write to file, edited task: {taskId}");
        }

        /// <summary>
        /// After varify the email returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The column with that name</returns>
        internal Column GetColumn(string email, string columnName)
        {
            IsLoggedIn(email);         
            return loggedIn.Board.GetColumn(columnName);
        }

        internal void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            IsLoggedIn(email);
            loggedIn.Board.ChangeColumnName(email,columnOrdinal,newName);
        }

        /// <summary>
        /// After varify the email returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>The column with that Id</returns>
        internal Column GetColumn(string email, int columnOrdinal)
        {
            IsLoggedIn(email);
            return loggedIn.Board.GetColumn(columnOrdinal);
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>The column that was added</returns>
        public Column AddColumn(string email, int columnOrdinal, string Name)
        {
            IsLoggedIn(email);
            return loggedIn.Board.AddColumn(columnOrdinal, Name, email);
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        public void RemoveColumn(string email, int columnOrdinal)
        {
            IsLoggedIn(email);
            loggedIn.Board.RemoveColumn(email, columnOrdinal);
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>The moved Column</returns>
        public Column MoveColumnRight(string email, int columnOrdinal)
        {
            IsLoggedIn(email);
            return loggedIn.Board.MoveColumnRight(email, columnOrdinal);
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>The moved Column</returns>
        public Column MoveColumnLeft(string email, int columnOrdinal)
        {
            IsLoggedIn(email);
            return loggedIn.Board.MoveColumnLeft(email, columnOrdinal);
        }
    }
    
}
