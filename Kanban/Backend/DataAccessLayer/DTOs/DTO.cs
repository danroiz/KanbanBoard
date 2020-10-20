using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public abstract class DTO
    {

        public const string EmailColumnName = "Email";
       

        public string email;

        protected Controllers.DalController controller;

        /// <summary>
        /// Email properites
        /// </summary>
        public string Email {
            get { return email; }
            set { email = value; }
            
        } 

        protected DTO(Controllers.DalController controller)
        {
            this.controller = controller;
        }
        /// <summary>
        /// creating an array of the dto unique keys
        /// </summary>
        /// <returns>A string array of the DTO keys</returns>
        protected abstract string[] KeysToArray();

        /// <summary>
        /// creating an array of the names of the column's that store the keys
        /// </summary>
        /// <returns>A string array of the key's column names</returns>
        protected abstract string[] KeyColumnsToArray();

        /// <summary>
        /// Insert the DTO to its relavant table
        /// </summary>
        public abstract void Insert();

        /// <summary>
        /// Delete this element from sql
        /// </summary>
        public void Delete()
        {
            controller.Delete(KeysToArray(), KeyColumnsToArray());
        }

    }
}