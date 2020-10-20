using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task : IPersistedObject<TaskDTO>
    {
        // ===================================FIELDS=========================================================
        private int id;
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        private string emailAssignee;

        //DTO
        private TaskDTO thisDTO;

        // const 
        private const int MAX_TITLE_LENGTH = 50;
        private const int MIN_TITLE_LENGTH = 1;
        private const int MAX_DESCRIPTION_LENGTH = 300;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ===================================CONSTRUCTORS=====================================================
        /// <summary>
        /// Task constructor. checks legal inputs
        /// </summary>
        /// <param name="id">The id of the Task to create</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>      
        /// <returns> new object of Task </returns>
        internal Task(string hostEmail, int boardId, int columnOrdinal, int id, string title, string description, DateTime dueDate, string emailAssignee)
        {
            this.creationTime = DateTime.Now;
            IsValidDueDate(dueDate);
            IsValidTitle(title);
            IsValidDescription(description);
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;
            this.id = id;
            this.emailAssignee = emailAssignee;
            thisDTO = new TaskDTO(hostEmail, columnOrdinal, id, title, description, dueDate, creationTime, boardId, emailAssignee);
        }
        
        /// <summary>
        /// convert DTOtask to task
        /// </summary>
        /// <param name="DTOtask"></param>
        internal Task(TaskDTO DTOtask)
        {
            this.id = (int)DTOtask.id;
            this.title = DTOtask.title;
            this.description = DTOtask.description;
            this.creationTime = DTOtask.creationTime;
            this.dueDate = DTOtask.dueDate;
            this.emailAssignee = DTOtask.emailAssignee;
            thisDTO = DTOtask;
        }

        // ===================================PROPERTIES=====================================================    
        /// <summary>
        /// Id properties
        /// </summary>
        internal virtual int Id
        {
            get { return id; }
        }

        internal virtual string EmailAssignee
        {
            get { return emailAssignee; }
            set
            {
                thisDTO.EmailAssignee = value;
                emailAssignee = value;
                log.Info($"task {id} now has new assignee {value}");
            }
        }
        
        
        /// <summary>
        /// Title properties
        /// </summary>
        internal virtual string Title
        {
            get { return title; }
            set
            {

                IsValidTitle(value);
                thisDTO.Title = value; // save
                title = value;
                log.Info($"Title {id} now has new title {value}");
            }
        }
        
        /// <summary>
        /// Description properites
        /// </summary>
        internal virtual string Description
        {
            get { return description; }
            set
            {
                IsValidDescription(value);
                thisDTO.Description = value; // save
                description = value;
                log.Info($"Title {id} now has new description {value}");
            }
        }
        
        /// <summary>
        /// Due-date properties
        /// </summary>
        internal virtual DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                IsValidDueDate(value);
                // Save
                thisDTO.DueDate = value;
                dueDate = value;
                log.Info($"Title {id} now has new dueDate {value}");
            }
        }

        /// <summary>
        /// Creation-time properties
        /// </summary>
        internal virtual DateTime CreationTime
        {
            get { return creationTime; }
        }
        
        // ===================================VERIFICATIONS=====================================================
        /// <summary>
        /// check if a title is valid
        /// </summary>
        /// <param name="title"></param>
        private void IsValidTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_TITLE_LENGTH | title.Length < MIN_TITLE_LENGTH)
            {
                log.Debug("tried to set title with illegal length or null");
                throw new Exception("title length must be between 1 to 50 charecter");
            }
        }
        
        /// <summary>
        /// check if a Due date is valid
        /// </summary>
        /// <param name="dateTime"></param>
        private void IsValidDueDate (DateTime dateTime)
        {
            if (dateTime == null || (DateTime.Now).CompareTo(dateTime) > 0) // checks legal dueDate
            { 
                log.Debug("tried to set illegal due date");
                throw new Exception("Unless you have time machince, due date can't be earlier than present time");
            }
        }

        internal void VerifyAssignee(string email)
        {
            if (!email.Equals(this.emailAssignee))
            {
                log.Debug("Only the task assignee can modify the task");
                throw new Exception("Only the task assignee can modify the task");
            }
        }
        internal void AssignTask(string email, string emailAssignee)
        {
            VerifyAssignee(email);
            EmailAssignee = emailAssignee;
        }

        /// <summary>
        /// check if a Description is valid
        /// </summary>
        /// <param name="value"></param>
        private void IsValidDescription(string value)
        {
            if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
            {
                log.Debug("tried to change description longer then 300 char");
                throw new Exception("Description max. Length is 300 charcter");
            }
        }

        internal void DeleteTask(string email)
        {
            VerifyAssignee(email);
            thisDTO.Delete();
        }

        internal void UpdateDTOOrdinal(int Ordinal)
        {
            thisDTO.ColumnOrdinal = Ordinal;
        }



        // ===================================TO_DAL_OBJECT=====================================================
        /// <summary>
        /// convert task to taskDTO
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public virtual TaskDTO ToDalObject()
        {
            return thisDTO;
        }

        
    }
}
