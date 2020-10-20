using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddTaskViewModel: NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }
        private ObservableCollection<TaskModel> tasks;
        private string title;
        private string description;
        private DateTime dueDate;
        private string userEmail;
        private string message;

        // ===================================CONSTRUCTORS=====================================================

        public AddTaskViewModel(BackendController controller, string userEmail, ObservableCollection<TaskModel> tasks)
        {
            this.Controller = controller;
            this.userEmail = userEmail;
            this.tasks = tasks;
            
        }

        // ===================================PROPERTIES=======================================================

        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        public DateTime CurrentTime
        {
            get => DateTime.Now;
        }
        public string Message
        {
            get { return message; }
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// Adding new Task To Board
        /// </summary>
        /// <returns>True if succeeded adding task or not</returns>
        public bool AddTask()
        {
            try
            {
                tasks.Add(Controller.AddTask(userEmail, title, description, dueDate));
                Message = "";
                return true;
            }
            catch(Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
    }
}
