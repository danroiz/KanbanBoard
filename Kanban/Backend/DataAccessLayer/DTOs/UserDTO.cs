using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;


namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class UserDTO : DTO
    {
        // ===================================fIELDS=============================================================
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string PasswordColumnName = "Password";
        public const string NicknameColumnName = "Nickname";
        public const string HostEmailColumnName = "HostEmail";
     
        public string nickname;
        public string password;
        public string hostEmail;

        // ===================================CONSTRUCTOR========================================================
        public UserDTO(string email, string password, string nickname, string hostEmail) : base(new UserDalController())
        {
            Email = email;
            this.password = password;
            this.nickname = nickname;
            this.hostEmail = hostEmail;
        }

        // ===================================PROPERTIES=========================================================
        public string HostEmail
        {
            get { return hostEmail; }
            set
            {
                controller.Update(KeysToArray(), KeyColumnsToArray(), HostEmailColumnName, value);
                hostEmail = value;
            }
        }

        public string Nickname
        {
            get { return nickname; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), NicknameColumnName, value);
                nickname = value;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), PasswordColumnName, value);
                password = value;
            }
        }

        // ===================================METHODS============================================================
        public override void Insert()
        {
            UserDalController UDC = (UserDalController)controller;
            UDC.Insert(this);
            log.Info($"Inserted new User {email} to the DB");
        }

        protected override string[] KeysToArray()
        {
            string[] keysArr = new string[1];
            keysArr[0] = Email;
            return keysArr;
        }

        protected override string[] KeyColumnsToArray()
        {
            string[] keyColumnsArr = new string[1];
            keyColumnsArr[0] = EmailColumnName;
            return keyColumnsArr;
        }
    }
}
