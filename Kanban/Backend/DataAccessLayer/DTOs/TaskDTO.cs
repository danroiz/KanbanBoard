using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class TaskDTO : DTO
    {
        // ===================================fIELDS=============================================================
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string IDColumnName = "TaskID";
        public const string TitleColumnName = "Title";
        public const string ColumnOrdinalColumnName = "ColumnOrdinal";
        public const string DescriptionColumnName = "Description";
        public const string DueDateColumnName = "DueDate";
        public const string CreationTimeColumnName = "CreationTime";
        public const string BoardIDColumnName = "BoardID";
        public const string EmailAssigneeColumnName = "EmailAssignee";

        public long id;
        public string title;
        public string description;
        public long columnOrdinal;
        public DateTime creationTime;
        public DateTime dueDate;
        public long boardID;
        public string emailAssignee;

        // ===================================CONSTRUCTOR=========================================================
        public TaskDTO(string email, long columnOrdinal, long id, string title, string description, DateTime dueDate, DateTime creationTime, long boardID, string emailAssignee) : base(new TaskDalController())
        {
            Email = email;
            this.id = id;
            this.columnOrdinal = columnOrdinal;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            this.boardID = boardID;
            this.emailAssignee = emailAssignee;
        }

        // ===================================PROPERTIES==========================================================
        public virtual string EmailAssignee
        {
            get { return emailAssignee; }
            set
            {
                controller.Update(KeysToArray(), KeyColumnsToArray(), EmailAssigneeColumnName, value);
                emailAssignee = value;
            }
        }
        public virtual long Id
        {
            get { return id; }
            set
            {               
                controller.Update(KeysToArray(), KeyColumnsToArray(), IDColumnName, value);
                id = value;
            }
        }
        public virtual string Title
        {
            get { return title; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), TitleColumnName, value);
                title = value;
            }
        }
        public virtual long ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), ColumnOrdinalColumnName, value);
                columnOrdinal = value;
            }
        }
        public virtual string Description
        {
            get { return description; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), DescriptionColumnName, value);
                description = value;
            }
        }
        public virtual DateTime CreationTime
        {
            get { return creationTime; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), CreationTimeColumnName, value.ToString());
                creationTime = value;
            }
        }
        public virtual DateTime DueDate
        {
            get { return dueDate; }
                set
                {
                    
                    controller.Update(KeysToArray(), KeyColumnsToArray(), DueDateColumnName, value.ToString());
                    dueDate = value;
            }
        }
        public virtual long BoardID
        {
            get { return boardID; }
        }

        // ===================================METHODS==============================================================
        protected override string[] KeysToArray()
        {
            string[] keysArr = new string[4];
            keysArr[0] = Email;
            keysArr[1] = columnOrdinal.ToString();
            keysArr[2] = id.ToString();
            keysArr[3] = boardID.ToString();
            return keysArr;
        }

        protected override string[] KeyColumnsToArray()
        {
            string[] keyColumnsArr = new string[4];
            keyColumnsArr[0] = EmailColumnName;
            keyColumnsArr[1] = ColumnOrdinalColumnName;
            keyColumnsArr[2] = IDColumnName;
            keyColumnsArr[3] = BoardIDColumnName;
            return keyColumnsArr;
        }

        public override void Insert()
        {
            TaskDalController TDC = (TaskDalController)controller;
            TDC.Insert(this);
            log.Info($"Inserted new Task to the DB. {id} {email} {boardID}");
        }
    }
}
