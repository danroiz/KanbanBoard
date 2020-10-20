using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class EditColumnViewModel : NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }
        private ColumnModel columnModel;
        private string message;
        private string userEmail;
        private string demoColumnName;
        private int demoLimit;

        // ===================================CONSTRUCTORS=====================================================

        public EditColumnViewModel(BackendController controller, string userEmail, ColumnModel column)
        {
            this.Controller = controller;
            UserEmail = userEmail;
            ColumnModel = column;
            DemoColumnName = column.Name;
            DemoLimit = column.Limit;
        }

        // ===================================PROPERTIES=======================================================

        public string UserEmail
        {
            get => userEmail;
            set
            {
                userEmail = value;
                RaisePropertyChanged("UserEmail");
            }
        }
        public string DemoColumnName
        {
            get => demoColumnName;
            set
            {
                demoColumnName = value;
                RaisePropertyChanged("DemoColumnName");
            }
        }
        public int DemoLimit
        {
            get => demoLimit;
            set
            {
                demoLimit = value;
                RaisePropertyChanged("DemoLimit");
            }
        }
        public ColumnModel ColumnModel
        {
            get => columnModel;
            set
            {
                columnModel = value;
                RaisePropertyChanged("ColumnModel");
            }
        }
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// edit the column
        /// </summary>
        public void EditColumn()
        {
            
            try
            {
                if (!DemoColumnName.Equals(columnModel.Name))
                {
                    Controller.ChangeColumnName(userEmail, columnModel.ColumnOrdinal, DemoColumnName);
                    ColumnModel.Name = demoColumnName;
                }
                if (DemoLimit != columnModel.Limit)
                {
                    Controller.LimitColumnTasks(userEmail, columnModel.ColumnOrdinal, DemoLimit);
                    ColumnModel.Limit = demoLimit;
                }
                Message = "";
            }

            catch (Exception e)
            {
                Message = e.Message;
            }

            
            
        }

        
        
    }
}
