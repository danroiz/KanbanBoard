using System;
using Presentation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class InfoTaskViewModel : NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }

        private TaskModel task;
        private string message;
        private bool enable;
        private string demoTitle;
        private string demoDescription;
        private DateTime demoDueDate;
        private string demoEmailAssignee;

        // ===================================CONSTRUCTORS=====================================================

        public InfoTaskViewModel(BackendController controller, TaskModel task)
        {
            this.Controller = controller;
            Task = task;

            DemoEmailAssignee = task.EmailAssignee;
            DemoTitle = task.Title;
            DemoDescription = task.Description;
            DemoDueDate = task.DueDate;

            enable = this.task.IsAssignee();
        }

        // ===================================PROPERTIES=======================================================

        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }
        public string DemoTitle
        {
            get => demoTitle;
            set
            {
                demoTitle = value;
                RaisePropertyChanged("DemoTitle");
            }
        }
        public DateTime DemoDueDate
        {
            get => demoDueDate;
            set
            {
                demoDueDate = value;
                RaisePropertyChanged("DemoDueDate");
            }
        }
        public string DemoDescription
        {
            get => demoDescription;
            set
            {
                demoDescription = value;
                RaisePropertyChanged("DemoDescription");
            }
        }
        public string DemoEmailAssignee
        {
            get => demoEmailAssignee;
            set
            {
                demoEmailAssignee = value;
                RaisePropertyChanged("DemoEmailAssigne");      
            }
        }
        public TaskModel Task
        {
            get
            {
                return task;
            }
            set
            {
                task = value;
                RaisePropertyChanged("Task");
            }
        }
        public bool Enable
        {
            get => enable;
            set
            {
                enable = value;
                RaisePropertyChanged("Enable");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// edit the task
        /// </summary>
        public void Save()
        {
            try
            {

                if (!DemoTitle.Equals(task.Title))
                {
                    Controller.UpdateTaskTitle(task.LoggedEmail, task.ColumnOrdinal, task.Id, demoTitle);
                    task.Title = demoTitle;
                }
                if (!DemoDescription.Equals(task.Description))
                {
                    Controller.UpdateTaskDescription(task.LoggedEmail, task.ColumnOrdinal, task.Id, demoDescription);
                    task.Description = demoDescription;
                }
                if (!DemoDueDate.Equals(task.DueDate))
                {
                    Controller.UpdateTaskDueDate(task.LoggedEmail, task.ColumnOrdinal, task.Id, demoDueDate);
                    task.DueDate = demoDueDate;
                    task.UpdateBackgroundColor();
                }
                if (!DemoEmailAssignee.Equals(task.EmailAssignee))
                {
                    Controller.AssignTask(task.LoggedEmail, task.ColumnOrdinal, task.Id, demoEmailAssignee);
                    task.EmailAssignee = demoEmailAssignee;
                    task.UpdateBorderColor();
                }

                Enable = task.LoggedEmail.Equals(task.EmailAssignee);
                Message = "";
            }
            catch(Exception e)
            {
                Message = e.Message;
            }
        }
    }
}
