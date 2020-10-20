using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private BusinessLayer.UserController userController;

        /// <summary>
        /// parameterless constructor for the startup of the system
        /// </summary>
        internal UserService()
        {
            userController = new BusinessLayer.UserController();
        }

        /// <summary>
        /// first initilliaztion of db when program starts
        /// </summary>
        public void InitDataBase()
        {
            userController.InitDataBase();
        }

        /// <summary>
        /// loading the users data files 
        /// </summary>
        public void LoadData()
        {
            userController.LoadData();
        }

        /// <summary>
        /// Remove all persistent data.
        /// </summary>
        public void DeleteData()
        {           
            userController.DeleteData();
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">he email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>returns the User Object</returns>
        public BusinessLayer.User Login(string email, string password)
        {
                return userController.Login(email, password);
        }

      

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="email">he email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        public void Register(string email, string password, string nickname)
        {
             userController.Register(email, password, nickname);
        }

        internal void Register(string email, string password, string nickname, string emailHost)
        {
            userController.Register(email, password, nickname,emailHost);
        }

        internal string AssignTask(string email, string emailAssignee)
        {
            return userController.AssignTask(email, emailAssignee);
        }
    }
}
