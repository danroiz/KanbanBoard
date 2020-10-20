
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board : IPersistedObject<BoardDTO>
    {
        // ===================================FIELDS========================================================
        private ArrayList columns;
        private int boardID;
        private int tasksIdCount; // unique ID for each task in the board
        private string creator;

        //DTO
        private BoardDTO thisDTO;

        // const
        private const int DEFUALT_BOARD = 0;
        private const int FIRST_COLUMN_DEFAULT = 0;
        private const int SECOND_COLUMN_DEFAULT = 1;
        private const int THIRD_COLUMN_DEFAULT = 2;
        private const int MINIMUM_COLUMN_DEFAULT = 2;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        // ===================================CONSTRUCTORS=====================================================
        /// <summary>
        /// classic constructor
        /// </summary>   
        /// <returns> new object of Board </returns>
        internal Board(string email)
        {
            tasksIdCount = 0;
            boardID = DEFUALT_BOARD;
            creator = email;
            thisDTO = new BoardDTO(email, boardID, tasksIdCount);
            thisDTO.Insert();

            columns = new ArrayList(3);
            columns.Insert(FIRST_COLUMN_DEFAULT, new Column(email, "backlog",boardID, FIRST_COLUMN_DEFAULT));
            columns.Insert(SECOND_COLUMN_DEFAULT, new Column(email,"in progress", boardID, SECOND_COLUMN_DEFAULT));
            columns.Insert(THIRD_COLUMN_DEFAULT, new Column(email,"done", boardID, THIRD_COLUMN_DEFAULT));
        }
        
        /// <summary>
        /// convert boardDTO to board
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <param name="DTOcolumns"></param>
        /// <param name="DTOtasks"></param>
        internal Board(BoardDTO boardDTO, List<ColumnDTO> DTOcolumns, List<TaskDTO> DTOtasks)
        {
            tasksIdCount = (int)boardDTO.taskIdCount;
            boardID = (int)boardDTO.boardID;
            creator = boardDTO.email;
            thisDTO = boardDTO;

            columns = new ArrayList();
            for(int i=0; i<DTOcolumns.Count;i++) // initialize list with null values
            {
                columns.Insert(i, null);
            }

            foreach (ColumnDTO DTOColumn in DTOcolumns) // convert each D.A.L column to B.L column
            {
                columns[(int)DTOColumn.columnOrdinal] = new Column(DTOColumn); // replace the null values with Columns
            }
            foreach (TaskDTO DTOtask in DTOtasks)
            {
                ((Column)columns[(int)DTOtask.columnOrdinal]).AddExistingTask(new Task(DTOtask));
            }

        }

        // ===================================PROPERTIES=======================================================
        /// <summary>
        /// TaskIdCount properties
        /// </summary>
        internal int TaskIdCount {
            get { return tasksIdCount; }
        }
        
        /// <summary>
        /// BoardId properties
        /// </summary>
        internal int BoardID
        {
            get { return boardID; }
        }

        /// <summary>
        /// BoardCreator properties
        /// </summary>
        internal string Creator
        {
            get { return creator; }
        }

        // ===================================VERIFICATIONS=====================================================
        /// <summary>
        /// Checks if board has the given column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        private void VerifyValidColumnID(int columnOrdinal)
        {
            if (columnOrdinal >= columns.Count | columnOrdinal < FIRST_COLUMN_DEFAULT)
            {
                log.Debug("tried to do action on non exist column");
                throw new Exception("illigal column id"); // no such column
            }
        }
        
        /// <summary>
        /// Checks if given column is not the "done" column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        private void VerifyNotLastColumn(int columnOrdinal)
        {
            if (columnOrdinal == columns.Count - 1)
            {
                log.Debug("user tried to do illegal action on last column");
                throw new Exception("unable to modify last column");
            }
        }

        /// <summary>
        /// Get the names of all the board's columns
        /// </summary>
        /// <returns> a List of strings of the board's columns names  </string> </returns>
        internal LinkedList<string> GetColumnNames()
        {
            LinkedList<string> nameList = new LinkedList<string>();
            foreach (Column column in columns)
            {
                nameList.AddLast(column.Name);
            }
            return nameList;
        }
        /// <summary>
        /// Assigne a task to a different user in board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="emailAssignee"></param>
        internal void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            VerifyValidColumnID(columnOrdinal);
            VerifyNotLastColumn(columnOrdinal);
            ((Column)columns[columnOrdinal]).GetTask(taskId).AssignTask(email,emailAssignee);
            
        }

        // ===================================TASK_METHODS=====================================================
        /// <summary>
        /// Add a new task to the first column.
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A Task if added succssefully</returns>
        internal Task AddTask(String title, String descriotion, DateTime DueDate,string email)
        {
            Task task = new Task(creator, boardID, FIRST_COLUMN_DEFAULT, tasksIdCount, title, descriotion, DueDate, email); // throws exception if neccessary
            ((Column)columns[FIRST_COLUMN_DEFAULT]).AddTask(task); // throws exception if neccessary
            thisDTO.TaskIdCount = (long) (tasksIdCount +1);
            tasksIdCount++;
            return task;
        }

        /// <summary>
        /// Delete task from board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        internal void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            VerifyValidColumnID(columnOrdinal);
            VerifyNotLastColumn(columnOrdinal);
            ((Column)columns[columnOrdinal]).DeleteTask(email, taskId);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="col">the column of which the Task is CURRENT at</param>
        /// <param name="id">The task to be updated identified task ID</param>
        internal void AdvanceTask(int col, int id, string email)
        {
            VerifyValidColumnID(col);
            VerifyNotLastColumn(col);
            
            Task toAdvance = ((Column)columns[col]).GetTask(id); // throws exception if task not found
            toAdvance.VerifyAssignee(email);
            ((Column)columns[col + 1]).AddExistingTask(toAdvance);// throws exception if task can't be advanced
            ((Column)columns[col]).RemoveTask(id);

            toAdvance.UpdateDTOOrdinal(col + 1);  
        }
        
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="Title">New title for the task</param>
        internal void UpdateTaskTitle(string email, int columnOrdinal, int taskId, String Title)
        {
            VerifyValidColumnID(columnOrdinal);
            VerifyNotLastColumn(columnOrdinal);
            Task TaskToUpdate = ((Column)columns[columnOrdinal]).GetTask(taskId);
            TaskToUpdate.VerifyAssignee(email);
            TaskToUpdate.Title = Title; // throws exception if task not exist
        }
        
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        internal void UpdateTaskDescription(string email, int columnOrdinal, int taskId, String description)
        {
            VerifyValidColumnID(columnOrdinal);
            VerifyNotLastColumn(columnOrdinal);
            Task TaskToUpdate = ((Column)columns[columnOrdinal]).GetTask(taskId);
            TaskToUpdate.VerifyAssignee(email);
            TaskToUpdate.Description = description;
        }
        
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        internal void UpdateTaskDueDate (string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            VerifyValidColumnID(columnOrdinal);
            VerifyNotLastColumn(columnOrdinal);
            Task TaskToUpdate = ((Column)columns[columnOrdinal]).GetTask(taskId);
            TaskToUpdate.VerifyAssignee(email);
            TaskToUpdate.DueDate = dueDate;
        }

        /// <summary>
        /// Change the column name
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="newName"></param>
        internal void ChangeColumnName(string email,int columnOrdinal, string newName)
        {
            VerifyCreator(email);
            VerifyValidColumnID(columnOrdinal);
            VerifyColumnName(newName);

            ((Column)columns[columnOrdinal]).Name = newName;
        }

        // ===================================COLUMN_METHODS=====================================================
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A Column with same name as the input string</returns>
        internal Column GetColumn(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                log.Debug("user gave illegal column name");
                throw new Exception("columnName can't be null");
            }
            foreach (Column column in columns)
            {
                if (column.Name.Equals(columnName)) // not case sensisitive 
                    return column;
            }         
            log.Debug("user gave illegal column name");
            throw new Exception("illegal column name");
        }
        
        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A Column that match the given int</returns>
        internal Column GetColumn(int columnOrdinal)
        {
            VerifyValidColumnID(columnOrdinal);
            return (Column)columns[columnOrdinal];
        }
        
        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        internal void SetLimit(string email, int columnOrdinal, int newLimit)
        {
            VerifyCreator(email);
            VerifyValidColumnID(columnOrdinal);
            ((Column)columns[columnOrdinal]).Limit = newLimit;

        }

        /// <summary>
        /// Check if a column name already exist in the board
        /// </summary>
        /// <param name="name"></param>
        internal void VerifyColumnName(string name)
        {
            foreach (Column col in columns)
            {
                if (col.Name.Equals(name)) // name already exist
                {
                    log.Debug("tried to add column with existing name");
                    throw new Exception("this name is already used in your board");

                }
            }
        }

        /// <summary>
        /// adding a new column to board
        /// </summary>
        /// <param name="ColumnOrdinal"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal  Column AddColumn(int ColumnOrdinal, string name, string email)
        {
            VerifyCreator(email);
            if (ColumnOrdinal!=0) VerifyValidColumnID(ColumnOrdinal-1); // ordinal can be in the range of [0,n] where n is number of columns in the list
            VerifyColumnName(name);
            Column column = new Column(email, name, boardID, -1);
            for (int i = columns.Count-1; i >= ColumnOrdinal; i--) // update the relevants column's new ordinal
            {
                ((Column)columns[i]).UpdatedOrdinal(i+1);
            }
            column.UpdatedOrdinal(ColumnOrdinal);
            columns.Insert(ColumnOrdinal , column);
            log.Info($"Added new Column: {email} {name} {BoardID}");
            return (Column)columns[ColumnOrdinal];
        }

        /// <summary>
        /// deleting a column from board
        /// </summary>
        /// <param name="ColumnOrdinal"></param>
        internal void RemoveColumn (string email, int ColumnOrdinal)
        {
            VerifyCreator(email);
            VerifyValidColumnID(ColumnOrdinal);

            if (columns.Count <= MINIMUM_COLUMN_DEFAULT)
            {
                log.Debug($"failed to remove column {ColumnOrdinal} due minimum number of columns in board");
                throw new Exception("Reached minimum number of columns");
            }
            if (ColumnOrdinal == FIRST_COLUMN_DEFAULT) // merge right
            {
                ((Column)columns[SECOND_COLUMN_DEFAULT]).Merge((Column)columns[FIRST_COLUMN_DEFAULT], SECOND_COLUMN_DEFAULT);
            }

            else // merge left
            {
                ((Column)columns[ColumnOrdinal - 1]).Merge((Column)columns[ColumnOrdinal], ColumnOrdinal - 1);
            }

            ((Column)columns[ColumnOrdinal]).ToDalObject().Delete(); // delete the column in the DB

            for (int i = ColumnOrdinal+1; i < columns.Count; i++) // update the relevants column's new ordinal in the DB
            {
                ((Column)columns[i]).UpdatedOrdinal(i - 1);
            }
            columns.RemoveAt(ColumnOrdinal);
            log.Info($"Removed Column: {ColumnOrdinal}");
        }
        
        /// <summary>
        /// move right a column in board 
        /// </summary>
        /// <param name="ColumnOrdinal"></param>
        internal Column MoveColumnRight(string email,int ColumnOrdinal)
        {
            VerifyCreator(email);
            VerifyNotLastColumn(ColumnOrdinal);
            VerifyValidColumnID(ColumnOrdinal);
            
            DB_SwapColumnID(ColumnOrdinal, ColumnOrdinal+1);
            BL_SwapColumnID(ColumnOrdinal, ColumnOrdinal + 1);          

            return (Column)columns[ColumnOrdinal + 1];
        }
       
        /// <summary>
        /// Swap the column id of two columns in the Data Base 
        /// </summary>
        /// <param name="firstID"></param>
        /// <param name="secondID"></param>
        private void DB_SwapColumnID(int firstID, int secondID)
        {
            ((Column)columns[firstID]).UpdatedOrdinal(-1);
            ((Column)columns[secondID]).UpdatedOrdinal(firstID);
            ((Column)columns[firstID]).UpdatedOrdinal(secondID);

            log.Info($"Swapped the ColumnsID of {firstID} and {secondID} in the DB");
        }
        
        /// <summary>
        /// Swap the column id of two columns in the Business layer
        /// </summary>
        /// <param name="firstID"></param>
        /// <param name="secondID"></param>
        private void BL_SwapColumnID(int firstID, int secondID)
        {
            Column temp = (Column)columns[firstID];
            columns[firstID] = columns[secondID];
            columns[secondID] = temp;
            log.Info($"Swapped the ColumnsID of {firstID} and {secondID} in the BL");
        }
        
        /// <summary>
        /// move left a column in board
        /// </summary>
        /// <param name="ColumnOrdinal"></param>
        internal Column MoveColumnLeft(string email, int ColumnOrdinal)
        {
            VerifyCreator(email);
            VerifyValidColumnID(ColumnOrdinal);
            if (ColumnOrdinal == FIRST_COLUMN_DEFAULT)
            {
                log.Debug("Tried to move left the first column");
                throw new Exception("cannot move the first column to the left");
            }
            DB_SwapColumnID(ColumnOrdinal, ColumnOrdinal - 1);
            BL_SwapColumnID(ColumnOrdinal, ColumnOrdinal - 1);

            return (Column)columns[ColumnOrdinal - 1];
        }

        /// <summary>
        /// Verify if the loggedin user is the creator of the board
        /// </summary>
        /// <param name="email"></param>
        internal void VerifyCreator(string email)
        {
            if (!(email.Equals(creator))) {
                throw new Exception($"Only the board creator {creator} can change columns");
            }
           
        }

        // ===================================TO_DAL_OBJECT=====================================================
        /// <summary>
        /// convert board to boardDTO
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public BoardDTO ToDalObject()
        {
            return thisDTO;
        }

    }
}
