using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User : IPersistedObject<UserDTO>
    {
        // ===================================FIELDS==========================================================
        private string email;
        private string password;
        private string nickname;
        private Board board;

        //DTO
        private UserDTO thisDTO;
        
        //const
        private const int MAX_PASS_LENGTH = 25;
        private const int MIN_PASS_LENGTH = 5;
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ===================================CONSTRUCTORS=====================================================
        /// <summary>
        /// creates a new user
        /// </summary>
        /// <param name="email">The email address of the user to create</param>
        /// <param name="pass">The password of the user to create</param>
        /// <param name="nick">The nickname of the user to create</param>
        /// <returns> new object of User </returns>
        internal User(string email, string pass, string nick)
        {
            Email = email;
            Password = pass;
            Nickname = nick;
            board = new Board(email);
            thisDTO = new UserDTO(email, password, nickname, email);
            thisDTO.Insert();

        }

        internal User(string email, string pass, string nick, Board _board)
        {
            Email = email;
            Password = pass;
            Board = _board;
            Nickname = nick;
            thisDTO = new UserDTO(email, password, nickname, board.Creator);
            thisDTO.Insert();

        }

        /// <summary>
        /// convert UserDTO to User
        /// </summary>
        /// <param name="DTOuser"></param>
        /// <param name="boardDTO"></param>
        /// <param name="columnDTOs"></param>
        /// <param name="taskDTOs"></param>
        internal User(UserDTO DTOuser, Board board)
        {
            thisDTO = DTOuser;
            Email = DTOuser.Email;
            Password = DTOuser.password;
            Nickname = DTOuser.nickname;
            Board = board;
            

        }

        // ===================================PROPERTIES=======================================================
        /// <summary>
        /// Board properties
        /// </summary>
        internal Board Board
        {
            get
            {
                return board;
            }
            set
            {
                if (value == null)
                    throw new Exception("cant set null board to a user");
                board = value;
            }
        }
        
        /// <summary>
        /// Email properties
        /// </summary>
        internal string Email
        {
            get
            {
                return email;
            }
            private set
            {
                if (!IsValidEmail(value))
                {
                    log.Debug("tried to enter invalid email");
                    throw new Exception("not valid email");
                }
                email = value;
            }
        }
        
        /// <summary>
        /// Nickname properties
        /// </summary>
        internal string Nickname
        {
            get { return nickname; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length == 0)
                {
                    log.Debug("tried to enter invalid email");
                    throw new Exception("not valid nickname");
                }
                nickname = value;
            }
        }
        
        /// <summary>
        /// Password properties
        /// </summary>
        internal string Password
        {
            set
            {
                if (!ValidPassword(value))
                {
                    log.Debug("tried to enter invalid password");
                    throw new Exception("not a valid password");
                }
                password = value;
            }
        }

        // ===================================VERIFICATIONS=====================================================
        /// <summary>
        /// Checks if password string compile with the demands of a password (length and char types)
        /// </summary>
        /// <param name="pass">string to compare if its the current pass</param>          
        /// <returns> True if pass valid </returns>
        private bool ValidPassword(string pass)
        {
            bool upper = false; bool lower = false; bool number = false;
            if (string.IsNullOrWhiteSpace(pass) || pass.Length < MIN_PASS_LENGTH | pass.Length > MAX_PASS_LENGTH) // valid length
                return false;
            for (int i = 0; i < pass.Length | !(upper & lower & number); i++)
            {
                char curr = pass[i];
                if (curr >= '0' & curr <= '9') // contain a number
                    number = true;
                else if (curr >= 'A' & curr <= 'Z')// contain UpperCase char
                    upper = true;
                else if (curr >= 'a' & curr <= 'z')// contain LowerCase char
                    lower = true;
            }
            return upper & lower & number;
        }
  
        // .NET Code. "don't invent the wheel" e.ahia
        // ref: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                new System.Net.Mail.MailAddress(email);
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Compare a string to a actual user password field.
        /// </summary>
        /// <param name="pass">string to compare if its the current pass</param>          
        /// <returns> True if pass matches to actual User password </returns>
        internal bool IsPass(string pass)
        {
            if (string.IsNullOrWhiteSpace(pass))
                return false;
            return pass.Equals(password);
        }

        // ===================================METHODS===========================================================
        /// <summary>
        /// Set a new password to the User - **NOT A REQUIERMENT**
        /// </summary>
        /// <param name="oldP">The old password to be change</param>
        /// <param name="newP">The new password that will be</param>    
        /// <returns> True if changed </returns>
        internal void ChangePass(string oldP, string newP)
        {
            if (!IsPass(oldP))
            {
                log.Debug("try to change pass with incorrect old pass");
                throw new Exception("old password is incorrect");
            }
            else if (!ValidPassword(newP))
            {
                log.Debug("tried to set a weak new password");
                throw new Exception("new password is not strong enough");
            }
            password = newP;
        }

        // ===================================TO_DAL_OBJECT=====================================================
        /// <summary>
        /// convert user to UserDTO
        /// </summary>
        /// <returns></returns>
        public UserDTO ToDalObject()
        {
            return thisDTO;
        }


    }
}
