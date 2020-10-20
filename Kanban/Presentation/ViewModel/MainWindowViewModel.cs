using IntroSE.Kanban.Backend.BusinessLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class MainWindowViewModel : NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }

        private string email;
        private string password;
        private string message;

        // ===================================CONSTRUCTORS=====================================================

        public MainWindowViewModel(BackendController controller)
        {
            this.Controller = controller;
        }

        // ===================================PROPERTIES=======================================================

        public string Email
        {
            get => email;
            set
            {
                this.email = value;
                RaisePropertyChanged("Email");
            }
        }
        public string Password
        {
            get => password;
            set
            {
                this.password = value;
                RaisePropertyChanged("Password");
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
        /// login 
        /// </summary>
        /// <returns>true if succeeded login</returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(email, password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
