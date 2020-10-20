using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Presentation
{
    public class BackendController
    {
        public IService Service { get; private set; }

        public BackendController(IService service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new Service();
            Response res = Service.LoadData();
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

        }

        /// <summary>
        /// Remove all persistent data.
        /// </summary>
        /// <returns></returns>
        internal void DeleteData()
        {
            Response res = Service.DeleteData();
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("YOU FUCKING DELETED ALL DATA YOU IDIOT");
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>User Model with a value set to the user if exsist</returns>
        internal UserModel Login(string username, string password)
        {
            Response<User> user = Service.Login(username, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(user.Value.Email, user.Value.Nickname);
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        internal void Logout(string email)
        {
            Response res = Service.Logout(email);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<param name="nickname">the user nickname.</param>
        internal void Register(string email, string password, string nickname)
        {
            Response res = Service.Register(email, password, nickname);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Registred succssefully");
        }

        /// <summary>
        /// Registers a new user and joins the user to an existing board.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="emailHost">The email address of the host user which owns the board</param>
        internal void Register(string email, string password, string nickname, string hostEmail)
        {
            Response res = Service.Register(email, password, nickname,hostEmail);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Registred succssefully");
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        internal BoardModel GetBoard(string email)
        {
            Response<Board> board = Service.GetBoard(email);
            if (board.ErrorOccured)
                throw new Exception(board.ErrorMessage);
            ObservableCollection<ColumnModel> columns = new ObservableCollection<ColumnModel>();
            int columnOrdinal = 0;
            foreach (String columnName in board.Value.ColumnsNames)
            {
                columns.Add(GetColumn(email, columnOrdinal));
                columnOrdinal = columnOrdinal + 1;
            }
            return new BoardModel(board.Value.emailCreator,columns,email);
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        internal TaskModel AddTask(string email, string title, string description, DateTime dueDate)
        {
            Response<Task> task = Service.AddTask(email, title, description, dueDate);
            if (task.ErrorOccured)
                throw new Exception(task.ErrorMessage);
            return new TaskModel(task.Value.Id,title,description,task.Value.CreationTime,dueDate,task.Value.emailAssignee,email,0);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        internal void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Response res = Service.AdvanceTask(email, columnOrdinal, taskId);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        internal void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response res = Service.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Assignee was updated successfully");
        }


        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        		
        internal void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Response res = Service.DeleteTask(email, columnOrdinal, taskId);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Task deleted successfully");

        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        internal void UpdateTaskDescription(string userEmail,int columnOrdinal, int taskId, string value)
        {
            Response res = Service.UpdateTaskDescription(userEmail, columnOrdinal, taskId, value);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            MessageBox.Show("Task descripition updated successfully");
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        internal void UpdateTaskTitle(string userEmail, int columnOrdinal, int taskId, string value)
        {
            Response res = Service.UpdateTaskTitle(userEmail, columnOrdinal, taskId, value);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            MessageBox.Show("Task title updated successfully");
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        internal void UpdateTaskDueDate(string userEmail, int columnOrdinal, int taskId, DateTime value)
        {
            Response res = Service.UpdateTaskDueDate(userEmail, columnOrdinal, taskId, value);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            MessageBox.Show("Task duedate updated successfully");
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        internal void RemoveColumn(string email, int columnOrdinal)
        {
            Response res = Service.RemoveColumn(email, columnOrdinal);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Column deleted successfully");
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        internal ColumnModel AddColumn(string email, int columnOrdinal, string Name)
        {
            Response<Column> column = Service.AddColumn(email, columnOrdinal, Name);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);

            MessageBox.Show("Column added successfully");



            return new ColumnModel(new ObservableCollection<TaskModel>(), Name,column.Value.Limit,columnOrdinal,email);
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        internal ColumnModel GetColumn(string email, string columnName)
        {
            Response<Column> column = Service.GetColumn(email, columnName);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            BoardModel boardModel = GetBoard(email);
            return boardModel.GetColumnModel(columnName);
        }


        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        internal ColumnModel GetColumn(string email, int columnOrdinal)
        {
            Response<Column> column = Service.GetColumn(email, columnOrdinal);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            ObservableCollection < TaskModel >  tasks = new ObservableCollection<TaskModel>();
            foreach (Task task in column.Value.Tasks)
            {
                tasks.Add(new TaskModel(task.Id, task.Title, task.Description, task.CreationTime, task.DueDate, task.emailAssignee, email, columnOrdinal));
            }
            ColumnModel col = new ColumnModel(tasks, column.Value.Name, column.Value.Limit, columnOrdinal, email);
            return col;
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        internal ColumnModel  MoveColumnRight(string email, int columnOrdinal)
        {
            Response<Column> column = Service.MoveColumnRight(email, columnOrdinal);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            return GetColumn(email, column.Value.Name);
        }


        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        internal ColumnModel MoveColumnLeft(string email, int columnOrdinal)
        {
            Response<Column> column = Service.MoveColumnLeft(email, columnOrdinal);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            return GetColumn(email, column.Value.Name);
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        internal void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Response res = Service.LimitColumnTasks(email, columnOrdinal, limit);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Column limit updated succssefully");
        }

        /// <summary>
        /// Change the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">The new name.</param>
        internal void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            Response res = Service.ChangeColumnName(email, columnOrdinal, newName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            MessageBox.Show("Column name updated succssefully");
        }

    }
}
