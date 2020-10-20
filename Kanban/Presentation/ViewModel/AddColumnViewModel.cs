using IntroSE.Kanban.Backend.BusinessLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddColumnViewModel: NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }
        private ObservableCollection<ColumnModel> columns;
        private string userEmail;
        private string columnName;
        private int columnOrdinal;
        private string message;

        // ===================================CONSTRUCTORS=====================================================

        public AddColumnViewModel(BackendController controller, string userEmail, ObservableCollection<ColumnModel> columns)
        { 
            this.Controller = controller;
            this.userEmail = userEmail;
            this.columns = columns;
        }

        // ===================================PROPERTIES=======================================================

        public string ColumnName
        {
            get => columnName;
            set
            {
                columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }
        public int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        public ObservableCollection<ColumnModel> Columns
        {
            get => columns;
            set
            {
                columns = value;
                RaisePropertyChanged("Columns");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// Add new Column to the Board
        /// </summary>
        public void AddColumn()
        {
            try
            {
                ColumnModel columnModel = Controller.AddColumn(userEmail, columnOrdinal, columnName);
                columns.Insert(columnOrdinal, columnModel);
                Message = "";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }
    }
}
