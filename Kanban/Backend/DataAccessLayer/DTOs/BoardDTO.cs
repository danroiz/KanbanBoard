using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class BoardDTO : DTO
    {
        // ===================================fIELDS=============================================================
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
      (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string BoardIDColumnName = "BoardID";
        public const string TaskIdCountColumnName = "TaskIdCount";

        public long taskIdCount;
        public long boardID;

        // ===================================CONSTRUCTOR========================================================
        public BoardDTO(string email, long boardID, long taskIdCount) : base(new BoardDalController())
        {
            Email = email;
            this.boardID = boardID;
            this.taskIdCount = taskIdCount;
        }

        // ===================================PROPERTIES=========================================================
        public long TaskIdCount
        {
            get
            {
                return taskIdCount;
            }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), TaskIdCountColumnName, value);
                taskIdCount = value;
            }
        }

        public long BoardID
        {
            get { return boardID; }
        }

        // ===================================METHODS============================================================
        public override void Insert()
        {
            BoardDalController BDC = (BoardDalController)controller;
            BDC.Insert(this);
            log.Info($"Inserted new Board to the DB {email} {boardID}");
        }

        protected override string[] KeysToArray()
        {
            string[] keysArr = new string[2];
            keysArr[0] = Email;
            keysArr[1] = boardID.ToString();
            return keysArr;
        }

        protected override string[] KeyColumnsToArray()
        {
            string[] keyColumnsArr = new string[2];
            keyColumnsArr[0] = EmailColumnName;
            keyColumnsArr[1] = BoardIDColumnName;
            return keyColumnsArr;
        }

    }
}
