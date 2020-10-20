using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Model
{

    public class BoardModel : NotifiableObject
    {
        // ===================================FIELDS========================================================
        private ObservableCollection<ColumnModel> columns;
        private string emailCreator;
        private string loggedUser;
        public TaskModel selectedTask;

        // ===================================CONSTRUCTORS=====================================================

        public BoardModel(string emailCreator, ObservableCollection<ColumnModel> columns, string loggedUser)
        {
            this.loggedUser = loggedUser;
            this.emailCreator = emailCreator;
            this.columns = columns;
            columns.CollectionChanged += HandleChange;
            
        }

        // ===================================PROPERTIES=======================================================

        public ObservableCollection<ColumnModel> Columns
        {
            get { return this.columns; }
            set { columns = value; }
        }
        public string EmailCreator
        {
            get
            {
                return emailCreator;
            }
        }

        // ===================================METHODS==========================================================

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int count = 0;
                foreach (ColumnModel col in columns)
                {
                    col.ColumnOrdinal = count;

                    count = count + 1;
                }

            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int count = 0;

                foreach (ColumnModel col in columns)
                {

                    col.ColumnOrdinal = count;
                    count++;
                }
            }
        }

        /// <summary>
        /// return the column by name
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ColumnModel GetColumnModel(String columnName)
        {
            foreach (ColumnModel columnModel in columns)
            {
                if (columnModel.Name.Equals(columnName))
                {
                    return columnModel;
                }
            }
            return null;
        }

        /// <summary>
        /// delete selected task
        /// </summary>
        /// <param name="taskToDelete"></param>
        public void DeleteTask(TaskModel taskToDelete)
        {
            columns[taskToDelete.ColumnOrdinal].Tasks.Remove(taskToDelete);
           
        }

        /// <summary>
        /// move selected column left
        /// </summary>
        /// <param name="columnOrdinal"></param>
        public void MoveColumnLeft(int columnOrdinal)
        {
            int temp = columnOrdinal - 1;
            ColumnModel tempCol = Columns[temp];
            Columns[temp] = columns[columnOrdinal];
            Columns[columnOrdinal] = tempCol;

            // update the columns new ordinal
            Columns[temp].ColumnOrdinal = temp; ;
            Columns[columnOrdinal].ColumnOrdinal = columnOrdinal;

           
        }

        /// <summary>
        /// advance task
        /// </summary>
        /// <param name="selectedTask"></param>
        public void AdvanceTask(TaskModel selectedTask)
        {
            Columns[selectedTask.ColumnOrdinal].Tasks.Remove(selectedTask);
            selectedTask.ColumnOrdinal = selectedTask.ColumnOrdinal + 1;
            Columns[selectedTask.ColumnOrdinal].Tasks.Add(selectedTask);
        }

        /// <summary>
        /// delete selected column
        /// </summary>
        /// <param name="selectedColumn"></param>
        public void DeleteColumn(ColumnModel selectedColumn)
        {
            int deletedColumnID = selectedColumn.ColumnOrdinal;         
            columns.Remove(selectedColumn);
        
            if (deletedColumnID == 0) // first column removed
            {
                foreach (TaskModel tsk in selectedColumn.Tasks)
                {
                    columns[0].Tasks.Add(tsk);
                    tsk.ColumnOrdinal = 0;
                }
            }
            else // not first column removed
            {
                foreach (TaskModel tsk in selectedColumn.Tasks)
                {
                    columns[deletedColumnID-1].Tasks.Add(tsk);
                    tsk.ColumnOrdinal = deletedColumnID - 1;
                }
            }
        }

        /// <summary>
        /// sort all columns
        /// </summary>
        /// <param name="comp"></param>
        public void Sort(Comparison<TaskModel> comp) // gineric sort for all the sorts.
        {
            foreach (ColumnModel col in columns)
            {
                col.Sort(comp);
            }
        }

        /// <summary>
        /// move selected column right
        /// </summary>
        /// <param name="columnOrdinal"></param>
        public void MoveColumnRight(int columnOrdinal)
        {
            int temp = columnOrdinal + 1;
            ColumnModel tempCol = Columns[temp];
            Columns[temp] = columns[columnOrdinal];
            Columns[columnOrdinal] = tempCol;
            Columns[temp].ColumnOrdinal = temp; ;
            Columns[columnOrdinal].ColumnOrdinal = columnOrdinal;    
        }

        /// <summary>
        /// filter the tasks
        /// </summary>
        /// <param name="filter"></param>
        public void FilterBoard(string filter)
        {

            foreach (ColumnModel column in columns)
            {
                column.FilterColumn(filter);
            }
        }
    }
}
