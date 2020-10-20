using IntroSE.Kanban.Backend.BusinessLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }
        private BoardModel board;
        private string userEmail;
        private string contactHost;
        private string title;
        private ColumnModel selectedColumn;
        private TaskModel selectedTask;
        private bool isHost;
        private bool isColumnSelected;
        private string message;
        private bool isTaskSelected;
        private string filter;
        private bool enableModify;

        // ===================================CONSTRUCTORS=====================================================

        public BoardViewModel(BackendController controller, UserModel user)
        {
            this.Controller = controller;
            this.userEmail = user.Email;
            this.Title = user.Nickname;
            this.board = Controller.GetBoard(userEmail);
            //this.hostEmail = board.EmailCreator;
            this.ContactHost = board.EmailCreator;
            this.isHost = this.userEmail.Equals(board.EmailCreator);
            isTaskSelected = false;
            enableModify = false;
          
        }

        // ===================================PROPERTIES=======================================================

        public bool EnableModify
        {
            get => enableModify;
            set
            {
                enableModify = value;
                RaisePropertyChanged("EnableModify");
            }
        }
        public bool IsTaskSelected
        {
            get => isTaskSelected;
            set
            {
                isTaskSelected = value;
                EnableModify = isTaskSelected & IsAssignee();
                RaisePropertyChanged("IsTaskSelected");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                this.title = "Hello " + value;
                RaisePropertyChanged("Title");
            }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                this.filter = value ;
                if (value == null ||value.Length == 0) board.FilterBoard(value);
                RaisePropertyChanged("Filter");
            }
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
        public ColumnModel SelectedColumn
        {
            get
            {
                return selectedColumn;
            }
            set
            {
                HandleSelect();

                selectedColumn = value;
                IsColumnSelected = (value != null);                
                RaisePropertyChanged("SelectedColumn");
            }
        }
        public TaskModel SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {

                HandleSelect();
                selectedTask = value;
                IsTaskSelected = value != null;
                EnableModify = IsAssignee() & IsTaskSelected;
                RaisePropertyChanged("SelectedTask");
            }
        }
        public BoardModel Board
        {
            get { return board; }
            set { this.board = value; }
        }
        public string UserEmail
        {
            get { return userEmail; }
        }
        public string ContactHost
        {
            get { return contactHost; }
            set
            {
                this.contactHost = "To contact board's owner use this email address: " + value;
                RaisePropertyChanged("ContactHost");
            }
        }
        public bool IsHost
        {
            get { return isHost; }
        }
        public bool IsColumnSelected
        {
            get { return isColumnSelected; }
            private set
            {
                this.isColumnSelected = IsHost & value;
                RaisePropertyChanged("IsColumnSelected");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// checks if the logged in user is the task assignee
        /// </summary>
        /// <returns></returns>
        public bool IsAssignee()
        {
            if (selectedTask != null)
                return SelectedTask.IsAssignee();
            return false;
        }

        /// <summary>
        ///revert the Selected Task/ Column when closing TaskInfo 
        /// </summary>
        /// 
        public void OnTaskViewClose()
        {
            if (!IsAssignee()) {
                IsTaskSelected = false;
            }
            IsColumnSelected = false;
        }

        /// <summary>
        /// Move Column left
        /// </summary>
        /// 
        public void MoveLeftColumn()
        {
            try
            {
                int columnOrdinal = SelectedColumn.ColumnOrdinal;
                Controller.MoveColumnLeft(UserEmail, columnOrdinal);
                board.MoveColumnLeft(columnOrdinal);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// turn filter on
        /// </summary>
        /// 
        public void FilterOn()
        {
            Board.FilterBoard(filter);
        }

        /// <summary>
        /// advance selected task
        /// </summary>
        /// 
        public void AdvanceTask()
        {
            try
            {
                if (SelectedTask == null)
                    throw new Exception("Please select a task to advance");
                int columnOrdinal = SelectedTask.ColumnOrdinal;

                Controller.AdvanceTask(userEmail, columnOrdinal, SelectedTask.Id);
                board.AdvanceTask(SelectedTask);

            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// move column right
        /// </summary>
        /// 
        public void MoveRightColumn()
        {
            try
            {
                int columnOrdinal = SelectedColumn.ColumnOrdinal;
                Controller.MoveColumnRight(UserEmail, columnOrdinal);
                board.MoveColumnRight(columnOrdinal);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// change the selecteTask/Column according to the user
        /// </summary>
        private void HandleSelect()
        {
            Message = "";


            // unchoose prev task
            selectedTask = null;
            IsTaskSelected = false;
            RaisePropertyChanged("SelectedTask");

            // unchoose prev column
            selectedColumn = null;
            IsColumnSelected = false;
            RaisePropertyChanged("SelectedColumn");
        }

        /// <summary>
        /// delete selected column
        /// </summary>
        public void DeleteColumn()
        {
            try
            {
                Controller.RemoveColumn(userEmail, selectedColumn.ColumnOrdinal);
                board.DeleteColumn(SelectedColumn);
            }
            catch (Exception e)
            {
                this.Message = e.Message;
            }
        }

        /// <summary>
        /// logout
        /// </summary>
        public void Logout()
        {
            Controller.Logout(userEmail);

        }

        /// <summary>
        /// delete selected task
        /// </summary>
        public void DeleteTask()
        {
            try
            {
                int deletedTaskColumnOrdinal = SelectedTask.ColumnOrdinal;
                Controller.DeleteTask(userEmail, deletedTaskColumnOrdinal, SelectedTask.Id);
                board.DeleteTask(SelectedTask);
            }
            catch (Exception e)
            {
                this.Message = e.Message;
            }

        }

        /// <summary>
        /// sort all task by due date
        /// </summary>
        public void SortByDueDate()
        {
            board.Sort((x, y) => x.DueDate.CompareTo(y.DueDate)); // generic sort that gets Lambda exprtion
        }

        /// <summary>
        /// sort all task by creation time
        /// </summary>
        public void SortByCreationTime()
        {
            board.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime)); // generic sort that gets Lambda exprtion
        }

        /// <summary>
        /// sort all tasks by search
        /// </summary>
        public void SortByTitle()
        {
            board.Sort((x, y) => x.Title.CompareTo(y.Title)); // generic sort that gets Lambda exprtion
        }

    }
}
