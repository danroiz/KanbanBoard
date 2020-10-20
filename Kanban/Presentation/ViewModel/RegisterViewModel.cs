using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RegisterViewModel : NotifiableObject
    {
        // ===================================FIELDS========================================================

        public BackendController Controller { get; private set; }
        private string email;
        private string nickName;
        private string password;
        private string validPassword;
        private bool isChecked;
        private bool isJoin;
        private string boardCreator;
        private string message;

        // ===================================CONSTRUCTORS=====================================================

        public RegisterViewModel(BackendController controller)
        {
            this.Controller = controller;
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
        public string ValidPassword
        {
            get => validPassword;
            set
            {
                this.validPassword = value;
                RaisePropertyChanged("ValidPassword");
                }
        }
        public string NickName
        {
            get => nickName;
            set
            {
                this.nickName = value;
                RaisePropertyChanged("NickName");
            }
        }
        public bool IsChecked
        {
            get => isChecked;
            set 
            {
                isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
        public bool IsJoin
        {
            get => isJoin;
            set
            {
                isJoin = value;
                RaisePropertyChanged("IsJoin");
            }
        }
        public string BoardCreator
        {
            get => boardCreator;
            set
            {
                boardCreator = value;
                RaisePropertyChanged("BoardCreator");
            }
        }

        // ===================================METHODS==========================================================

        /// <summary>
        /// register a new user
        /// </summary>
        /// <returns>true if succeeded register a new user</returns>
        public bool register()
        {
            try
            {
                if (password != validPassword) throw new Exception("password and valid password are not equal");
                if (!isChecked) throw new Exception("Are you a robot?");
                if (isJoin) Controller.Register(email, password, nickName,boardCreator);
                else Controller.Register(email, password, nickName);
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
