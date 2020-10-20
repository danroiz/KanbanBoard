using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;

        /// <summary>
        /// calssic constructor
        /// </summary>
        /// <param name="tasks">column's tasks</param>
        /// <param name="name">column's name</param>
        /// <param name="limit">column's limit</param>
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
        }

        /// <summary>
        /// constructor to convert from business layer column to service layer column
        /// </summary>
        /// <param name="column">business layer column</param>
        internal Column(BusinessLayer.Column column)
        {
            this.Limit = column.Limit; this.Name = column.Name;
            LinkedList<Task> TaskList = new LinkedList<Task>();
            foreach (BusinessLayer.Task tsk in column.Tasks)
            {
                TaskList.AddLast(new Task(tsk));
            }          
            this.Tasks = TaskList;
        }

        /// <summary>
        /// overriding tostring object
        /// </summary>
        /// <returns>new format tostring</returns>
        override
        public string ToString()
        {
            string output = "";
            output += "Column name: " + this.Name + " Column Limit: " + this.Limit + " Column Tasks: ";
            foreach (Task tsk in Tasks)
                output += tsk;
            
            return output;
        }
    }
}
