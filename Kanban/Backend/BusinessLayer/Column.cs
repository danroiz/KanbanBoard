using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class Column : IPersistedObject<ColumnDTO>
    {
        private const int DEFAULT_LIMIT = 100;
        // ===================================FIELDS=====================================================
        private Dictionary<int, Task> tasks;
        private string name;
        private int limit;

        //DTO
        private ColumnDTO thisDTO;

        //const
        private const int MAX_COLUMN_NAME = 15; 

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ===================================CONSTRUCTORS=====================================================
        /// <summary>
        /// classic constructor
        /// </summary>
        /// <param name="name"></param>
        internal Column(string email, string name, int boardId, int columnOrdinal)

        {
            IsValidColumnName(name);
            this.name = name;
            tasks = new Dictionary<int, Task>();
            limit = DEFAULT_LIMIT;

            thisDTO = new ColumnDTO(email, name, columnOrdinal, limit, boardId);

            thisDTO.Insert();


        }
        /// <summary>
        /// convert columnDTO to column
        /// </summary>
        /// <param name="DTOcolumn"></param>
        internal Column(ColumnDTO DTOcolumn)
        {

            thisDTO = DTOcolumn;
         
            this.limit = (int)DTOcolumn.Limit;
            this.name = DTOcolumn.Name;
            this.tasks = new Dictionary<int, Task>();
        }

        // ===================================PROPERTIES=======================================================
        /// <summary>
        /// Name properites
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                IsValidColumnName(value);
                thisDTO.Name = value; //SAVE
                name = value;
                
            }
        }    

        /// <summary>
        /// Limit properties
        /// </summary>
        public int Limit
        {
            get { return limit; }
            set
            {
                IsValidLimit(value);

                thisDTO.Limit = value; //SAVE
                limit = value;
                log.Info($"Column {name} has new limit = {value}");
            }
        }

        /// <summary>
        /// convert dictionary to linkedlist
        /// </summary>
        public LinkedList<Task> Tasks
        {
            get
            {
                LinkedList<Task> ll = new LinkedList<Task>();
                foreach (KeyValuePair<int, Task> kvp in tasks)
                {
                    ll.AddLast(kvp.Value);

                }
                return ll;
            }
        }

        // ===================================VERIFICATIONS=====================================================
        /// <summary>
        /// check the capacity of tasks in the column
        /// </summary>
        internal void IsValidCapacity()
        {
            if (tasks.Count >= limit & limit != -1)
            {
                log.Debug("tried to add task to a column with already max tasks");
                throw new Exception("Reached limit of tasks");
            }
        }

        /// <summary>
        /// check the if given limit is legal
        /// </summary>
        internal void IsValidLimit(int limit)
        {
            if (tasks.Count > limit & limit != -1)
            {
                log.Debug("tried to change column limit lower then number of tasks in the column");
                throw new Exception("current task amount bigger than limit");
            }

        }
        /// <summary>
        /// Check if a column name is valid
        /// </summary>
        /// <param name="value"></param>
        internal void IsValidColumnName(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_COLUMN_NAME || value.Length == 0)
            {
                log.Debug("tried to set nickname that is empty/null/illegal length");
                throw new Exception("column name is invalid. length should be between 1 to 15");

            }
        }

        // ===================================METHODS===========================================================
        /// <summary>
        /// add a given task to tasks dictionary.
        /// </summary>
        /// <param name="tsk">the task to add</param>
        /// for the ADDTASK into column and AdvanceTask
        internal void AddTask(Task tsk)
        {
            if (tsk == null)
            {
                log.Error("tried to add null task");
                throw new Exception("tried to add null task");

            }
            IsValidCapacity();

            tsk.ToDalObject().Insert() ;
            tasks.Add(tsk.Id, tsk);
          
        }
        
        /// <summary>
        /// adding task to column without changing the DTO
        /// </summary>
        /// <param name="tsk"></param>
        internal void AddExistingTask(Task tsk)
        {
            IsValidCapacity();
            tasks.Add(tsk.Id, tsk);
            
        }
        
        /// <summary>
        /// Return task with ID equles to taskID parameter.
        /// </summary>
        /// <param name="taskID">the ID of the task to return</param>
        /// <returns></returns>
        internal Task GetTask(int taskID)
        {
            if (!tasks.ContainsKey(taskID))
            {
                log.Debug("tried to get a non-existing task");
                throw new Exception($"Task ID not found in {name} column");
            }
            return tasks[taskID]; 
        }
        
        /// <summary>
        /// Remove a given task from tasks if exsist.
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        internal void RemoveTask(int taskID)
        {
           
            tasks.Remove(taskID);
            log.Info($"deleted task id: {taskID}");
        }

        /// <summary>
        /// merging 2 columns: this column gets all the tasks from othercoulmn
        /// </summary>
        /// <param name="otherColumn"></param>
        internal void Merge (Column deletedColumn, int insertToColumnID)
        {
            if (deletedColumn == null)
            {
                log.Error("how dafuq did u manage to merge null column??");
                throw new Exception("tried to merge a null column");
            }

            if (this.tasks.Count + deletedColumn.tasks.Count > limit && limit != -1)
            {
                log.Info($"failed to merge to Column {name} because of limit overflow");
                throw new Exception("column will reach its limit by this merge");
            }
            foreach(KeyValuePair<int,Task> kvp in deletedColumn.tasks)
            {
                kvp.Value.UpdateDTOOrdinal(insertToColumnID); // update each task's new column id.
                this.tasks.Add(kvp.Key, kvp.Value);
            }
            log.Info($"Merged {deletedColumn.name} to {this.name}");
        }

        internal void DeleteTask(string email, int taskId)
        {
            
            GetTask(taskId).DeleteTask(email); // delete from DB
            RemoveTask(taskId); // delete from BL
                      
        }


        internal void UpdatedOrdinal(int ColumnOrdinal)
        {
            thisDTO.ColumnOrdinal = ColumnOrdinal;
            foreach (Task task in Tasks)
                task.UpdateDTOOrdinal(ColumnOrdinal);
        }

        // ===================================TO_DAL_OBJECT=====================================================
        /// <summary>
        /// convert column to columnDTO
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public ColumnDTO ToDalObject()
        {
            return thisDTO;
        }

    }

}
