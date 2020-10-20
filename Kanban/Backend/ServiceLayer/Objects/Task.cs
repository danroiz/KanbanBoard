using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {

        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly DateTime DueDate;
        public readonly string Title;
        public readonly string Description;
        public readonly string emailAssignee;

        /// <summary>
        /// classic constructor
        /// </summary>
        /// <param name="id">task's id</param>
        /// <param name="creationTime">task's creation time</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="DueDate">task's duedate</param>
        internal Task(int id, DateTime creationTime, DateTime dueDate, string title, string description, string emailAssignee)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.emailAssignee = emailAssignee;
        }

        /// <summary>
        /// constructor to convert from business layer task to service layer task
        /// </summary>
        /// <param name="task">business layer task</param>
        internal Task(BusinessLayer.Task task)
        {
            this.Id = task.Id;
            this.CreationTime = task.CreationTime;
            this.Title = task.Title;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
            this.emailAssignee = task.EmailAssignee;
        }

        /// <summary>
        /// overriding tostring object
        /// </summary>
        /// <returns>new format tostring</returns>
        override
        public string ToString()
        {
            string str = $"id: {Id} CreationTime: {CreationTime} title: {Title} description: {Description} dueDate: {DueDate} emailAssignee: {emailAssignee}";
            return str;
        }
    }
}
