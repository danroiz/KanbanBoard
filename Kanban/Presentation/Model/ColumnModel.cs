using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{

    public class ColumnModel : NotifiableObject
    {
        // ===================================FIELDS========================================================
        private ObservableCollection<TaskModel> tasks;
        private string name;
        private int limit;
        private string limitTitle;
        private string loggedUser; // hack 
        private int columnOrdinal; // hack
        // ===================================CONSTRUCTORS=====================================================
       
        public ColumnModel(ObservableCollection<TaskModel> tasks, string name, int limit, int columnOrdinal, string loggedUser)
        {
            this.tasks = tasks;
            this.name = name;
            this.limit = limit;
            this.columnOrdinal = columnOrdinal;
            this.loggedUser = loggedUser;
            tasks.CollectionChanged += HandleChange;
            UpdateLimitTitle();

        }

        // ===================================PROPERTIES=======================================================

        public int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                columnOrdinal = value;
                foreach (TaskModel tsk in tasks)
                    tsk.ColumnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        public string Name
        {
            get { return name; }
            set {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public int Limit
        {
            get { return limit; }
            set
            {
                limit = value;
                UpdateLimitTitle();
                RaisePropertyChanged("Limit");
            }
        }
        public string LimitTitle
        {
            get
            {
                return limitTitle;
            }
            set
            {
                limitTitle = value;
                RaisePropertyChanged("LimitTitle");

            }
        }
        public ObservableCollection<TaskModel> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }

        // ===================================METHODS==========================================================

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274

            UpdateLimitTitle();
        }

        /// <summary>
        /// change the label of number of task / limit in the gui
        /// </summary>
        private void UpdateLimitTitle()
        {
            if (limit != -1) LimitTitle = tasks.Count + "/" + Limit + " tasks";
            else LimitTitle = "limitless column";
        }        

        /// <summary>
        /// sort the tasks in the column
        /// </summary>
        /// <param name="comp"></param>
        public void Sort(Comparison<TaskModel> comp)
        {
            List<TaskModel> taskslist = tasks.ToList();
            taskslist.Sort(comp);
            tasks.Clear();
            foreach (TaskModel task in taskslist)
            {
                tasks.Add(task);
            }
        }

        /// <summary>
        /// filter the tasks in the column according to the input from the user
        /// </summary>
        /// <param name="filter"></param>
        public void FilterColumn(string filter)
        {
            foreach (TaskModel task in tasks)
            {
                task.FilterTask(filter);
            }
        }
    }
}
