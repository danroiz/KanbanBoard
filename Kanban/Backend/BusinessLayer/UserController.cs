using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        // ===================================FIELDS=====================================================
        private Dictionary<string, User> users;
        

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ===================================CONSTRUCTOR=================================================
        /// <summary>
        /// Initiallize dictionary of Users
        /// </summary>   
        /// <returns> new object of UserController </returns>
        internal UserController()
        {
            users = new Dictionary<string, User>();
           
        }



        // ===================================METHODS=====================================================

        /// <summary>
        /// Initiallize the relevant tables in the data base.
        /// </summary>
        internal void InitDataBase()
        {
            UserDalController userDC = new UserDalController();
            userDC.InitiallizeDB();
            log.Info("Initiallized Database");
        }

        /// <summary>
        /// Load all persistance data from db to the bl. i
        /// providing the user controller the information of all the users
        /// </summary>
        internal void LoadData()
        {
            UserDalController userDC = new UserDalController();
            BoardDalController boardDC = new BoardDalController();
            ColumnDalController columnDC = new ColumnDalController();
            TaskDalController taskDC = new TaskDalController();

            Dictionary<string, Board> boards = new Dictionary<string, Board>();
            // load all boards from DB
            foreach (BoardDTO boardDTO in boardDC.SelectAllBoards())
            {
                string boardEmail = boardDTO.email;
                boards.Add(boardEmail, new Board(boardDTO, columnDC.SelectAllColumns(boardEmail), taskDC.SelectAllTasks(boardEmail)));
            }
            // load all users, assign each user to its board
            foreach (UserDTO DTOuser in userDC.SelectAllUsers())
            {
                string email = DTOuser.Email.ToLower();
                string hostEmail = DTOuser.hostEmail;
                users.Add(email, new User(DTOuser, boards[DTOuser.hostEmail]));
            }
            log.Info("Loaded all data from DB");
        }

        internal string AssignTask(string email, string emailAssignee)
        {
            email = VerifyContainUser(email);
            emailAssignee = VerifyContainUser(emailAssignee);
            // check if email and emailAssignee are members of same board
            if (!users[email].Board.Creator.Equals(users[emailAssignee].Board.Creator)){
                log.Debug("Can't change the assignee to a not board member user");
                throw new Exception("Can't change the assignee to a not board member user");
            }

            return emailAssignee;
        }

        /// <summary>
        /// Remove all persistent data.
        /// </summary>
        internal void DeleteData()
        {
            
            bool deleted = new UserDalController().DeleteAllData();
            if (!deleted)
                throw new Exception("Failed to delete all data from DB"); // written in the log already
            try
            {
                users.Clear();

            }
            catch (Exception e)
            {
                log.Error("Failed to clear users dictionary");
                throw e;
            }
            log.Info("Deleted all data in the DB");
        }
        internal string VerifyContainUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Debug("email is null or whitespace email");
                throw new Exception("email is invalid darling");
            }
            email = email.ToLower(); // our convention - save email with lowerCase chars only.
            if (!users.ContainsKey(email))
            {
                log.Debug("email is not exist");
                throw new Exception("email is not exist");
            }
            return email;

        }

        internal string VerifyNotContainUser(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                log.Debug("email is null or whitespace email");
                throw new Exception("email is invalid darling");
            }
            email = email.ToLower(); // our convention - save email with lowerCase chars only.
            if (users.ContainsKey(email))
            {
                log.Debug("email is already registred");
                throw new Exception("email already registered");
            }
            return email;

        }
        /// <summary>
        /// Adds a new User to the users dictionary. saves the new user in the database.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="pass">The password of the user to register</param>
        /// <param name="nick">The nickname of the user to register</param>
        internal void Register(string email, string pass, string nick)
        {
            email = VerifyNotContainUser(email);
            User us = new User(email, pass, nick); // throws exception if password/nick not valid       
            users.Add(email, us);
            
            log.Info("Registered a new user");
        }
        /// <summary>
        /// Adds a new User to the users dictionary. saves the new user in the database.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="pass">The password of the user to register</param>
        /// <param name="nick">The nickname of the user to register</param>
        internal void Register(string email, string pass, string nick, string emailHost)
        {
            email = VerifyNotContainUser(email);
            emailHost = VerifyContainUser(emailHost);         

            Board hostUserBoard = users[emailHost].Board;
            if (!hostUserBoard.Creator.Equals(emailHost))
            {
                log.Debug($"tried to join to a board not own by {emailHost}");
                throw new Exception($"tried to join to a board not own by {emailHost}");
            }
         
            User us = new User(email, pass, nick, hostUserBoard); // throws exception if password/nick not valid
            users.Add(email, us);

            log.Info("Registered a new user");
        }

        /// <summary>
        /// Checks that email exist and password match.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="pass">The password of the user to login</param>
        /// <returns>A User that match the input email and password</returns>
        internal User Login(string email, string pass)
        {
            

            if (string.IsNullOrWhiteSpace(email)) {
                log.Debug("tried to log in with null email");
                throw new Exception("don't mess around with null input. tried to login with invalid email");
            }
            email = email.ToLower();
            if (!users.ContainsKey(email)){
                log.Debug("tried to log in with unregistered email");
                throw new Exception("Email is not registered.");
            }
            if (!users[email].IsPass(pass)){
                log.Debug("tried to log in with not match password");
                throw new Exception("Password is not correct.");
            }

            log.Info($"login user by email: {email}");
            return users[email];
            
        }
    }
}