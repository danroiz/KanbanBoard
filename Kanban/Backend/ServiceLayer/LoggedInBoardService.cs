using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class LoggedInBoardService
    {
        BusinessLayer.LoggedInBoardController loggedInController;

        /// <summary>
        /// parameterless constructor for the startup of the system
        /// </summary>
        internal LoggedInBoardService()
        {
            loggedInController = new BusinessLayer.LoggedInBoardController();
        }

        /// <summary>
        /// will logout logged user if there is, before deletion of all data.
        /// </summary>
        internal void PrepareDelete()
        {
            loggedInController = new BusinessLayer.LoggedInBoardController();
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="BLUser">User to log in</param>
        internal void Login(BusinessLayer.User BLUser)
        {
            loggedInController.Login(BLUser);
        }
        
        /// <summary>
        /// Log out an logged in user.
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        public void Logout(string email)
        {
            loggedInController.Logout(email);       
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>returns the board's columns</returns>
        public LinkedList<string> GetBoard(string email)
        {
            LinkedList<string> boardList = loggedInController.GetBoard(email);
            return boardList;
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            loggedInController.LimitColumnTasks(email, columnOrdinal, limit);
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>returns the new added task object</returns>
        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            return new Task(loggedInController.AddTask(email, title, description, dueDate));
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            loggedInController.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            loggedInController.UpdateTaskTitle(email, columnOrdinal, taskId, title);
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            loggedInController.UpdateTaskDescription(email, columnOrdinal, taskId, description);
        }

        internal void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            loggedInController.AssignTask(email, columnOrdinal, taskId, emailAssignee);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            loggedInController.AdvanceTask(email, columnOrdinal, taskId);
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>returns the column</returns>
        public Column GetColumn(string email, string columnName)
        {
           return new Column(loggedInController.GetColumn(email, columnName));
        }

        internal void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            loggedInController.DeleteTask(email, columnOrdinal, taskId);
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>return the column</returns>
        public Column GetColumn(string email, int columnOrdinal)
        {
            return new Column(loggedInController.GetColumn(email, columnOrdinal));
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        public void RemoveColumn(string email, int columnOrdinal)
        {
            loggedInController.RemoveColumn(email, columnOrdinal);
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
        public Column AddColumn(string email, int columnOrdinal, string Name)
        {
            return new Column(loggedInController.AddColumn(email, columnOrdinal, Name));
        }

        internal string GetCreator()
        {
            return loggedInController.GetCreator();
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Column MoveColumnRight(string email, int columnOrdinal)
        {
            return new Column(loggedInController.MoveColumnRight(email, columnOrdinal));
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Column MoveColumnLeft(string email, int columnOrdinal)
        {
            return new Column(loggedInController.MoveColumnLeft(email, columnOrdinal));
        }

        internal void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            loggedInController.ChangeColumnName(email, columnOrdinal, newName);
        }
    }
}