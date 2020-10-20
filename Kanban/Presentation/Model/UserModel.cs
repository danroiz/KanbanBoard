using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    public class UserModel : NotifiableObject
    {
        // ===================================FIELDS========================================================
        private string email;
        private string nickname;

        // ===================================CONSTRUCTORS=====================================================
       
        public UserModel(string email, string nickname)
        {
            Email = email;
            Nickname = nickname;
        }
    
        // ===================================PROPERTIES=======================================================
        public string Email
        {
            get
            {
                return email;
            }
            
            set
            {
                email = value.ToLower();
                RaisePropertyChanged("Email");
            }
            
        }
        public string Nickname
        {
            get
            {
                return nickname;
            }
            
            set
            {
                nickname = value;
                RaisePropertyChanged("Nickname");
            }
            
        }

    }
}
