using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    public class TaskModel : NotifiableObject
    {
        // ===================================FIELDS========================================================
        private int id;
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        private string emailAssignee;
        private string loggedEmail; // hack
        private int columnOrdinal; // hack
        private const int FIRST_COLUMN_ID = 0;
        private bool isVisible = true;
        private Visibility visibility;
        private SolidColorBrush backgroundcolor;
        private SolidColorBrush borderColor;

        // ===================================CONSTRUCTORS=====================================================

        public TaskModel(int id, string title, string description, DateTime creationTime, DateTime dueDate, string emailAssignee, string loggedEmail, int columnOrdinal)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            this.emailAssignee = emailAssignee;
            //hacks
            this.loggedEmail = loggedEmail;
            this.columnOrdinal = columnOrdinal;
            UpdateBackgroundColor();
            visibility = Visibility.Visible;

        }


        // ===================================PROPERTIES=======================================================

        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                RaisePropertyChanged("Visibility");
            }
        }
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }
        public string LoggedEmail
        {
            get => loggedEmail;
        }
        public string EmailAssignee
        {
            get => emailAssignee;
            set
            {
                   this.emailAssignee = value.ToLower(); // more aesthetic
                   RaisePropertyChanged("EmailAssignee");           
            }
        }
        public int Id
        {
            get => id;
        }
        public string Title
        {
            get => title;
            set
            {
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Description
        {
            get => description;
            set
            {
                this.description = value;
                RaisePropertyChanged("Description");
            }
        }
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                this.dueDate = value;
                RaisePropertyChanged("DueDate");
            }

        }
        public DateTime CreationTime
        {
            get => creationTime;
            

        }       
        public SolidColorBrush BorderColor
        {
            get
            {
                borderColor = new SolidColorBrush(emailAssignee.Equals(loggedEmail) ? Colors.Blue : Colors.Gray);
                return borderColor;
            }

            set
            {
                borderColor = value;
                RaisePropertyChanged("BorderColor");
            }
        }
        public SolidColorBrush BackgroundColor
        {
            get => backgroundcolor;

            set
            {
                backgroundcolor = value;
                RaisePropertyChanged("BackgroundColor");
            }
        }
        public int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                //advance Task
                columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// filter the tasks according to the user input
        /// </summary>
        /// <param name="filter"></param>
        public void FilterTask(string filter)
        {
            if (filter == null || filter.Equals("")) Visibility = Visibility.Visible;            
            else if (((description!=null)&&(description.ToLower().Contains(filter.ToLower())) | title.ToLower().Contains(filter.ToLower()))) Visibility = Visibility.Visible;
            else Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// change the background color of the task in the gui
        /// </summary>
        /// 
        public void UpdateBackgroundColor()
        {
            long totalTime = (dueDate - creationTime).Ticks;
            double timeLimitToOrange = totalTime * 0.75;
            long timePassed = (DateTime.Now - creationTime).Ticks;
            if (DateTime.Now.CompareTo(dueDate) > 0) BackgroundColor = new SolidColorBrush(Colors.Red);
            else if (timePassed >= timeLimitToOrange) BackgroundColor = new SolidColorBrush(Colors.Orange);
            else BackgroundColor = new SolidColorBrush(Colors.Transparent);
        }        

        /// <summary>
        /// check if the logged in user is the task's assignee
        /// </summary>
        /// <returns></returns>
        /// 
        public bool IsAssignee()
        {
            return emailAssignee.Equals(loggedEmail);
        }

        /// <summary>
        /// change the border color of the task in the gui
        /// </summary>
        /// 
        public void UpdateBorderColor()
        {
            BorderColor = new SolidColorBrush(emailAssignee.Equals(loggedEmail) ? Colors.Blue : Colors.Gray);
        }
    }
}
