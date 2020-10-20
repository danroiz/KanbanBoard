using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;

        /// <summary>
        /// classic constructor
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="nickname">user's nickname</param>
        internal User(string email, string nickname)
        {
            this.Email = email;
            this.Nickname = nickname;
        }
        // You can add code here
    }
}
