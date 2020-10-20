
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class ColumnDTO : DTO
    {
        // ===================================FIELDS============================================================
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string NameColumnName = "Name";
        public const string ColumnOrdinalColumnName = "ColumnOrdinal";
        public const string LimitColumnName = "ColumnLimit";
        public const string BoardIDColumnName = "BoardID";

        public string name;
        public long limit;
        public long columnOrdinal;
        public long boardID;

        // ===================================CONSTRUCTOR========================================================
        public ColumnDTO(string email, string name, long columnOrdinal, long limit, long boardID) : base(new ColumnDalController())
        {
            Email = email;
            this.limit = limit;
            this.columnOrdinal = columnOrdinal;
            this.name = name;
            this.boardID = boardID;
        }

        // ===================================PROPERTIES=========================================================
        public virtual string Name
        {
            get { return name; }
            set
            {
               
                controller.Update(KeysToArray(), KeyColumnsToArray(), NameColumnName, value);
                name = value;
            }
        }
        public virtual long Limit
        {
            get { return limit; }
            set
            {
                
                controller.Update(KeysToArray(), KeyColumnsToArray(), LimitColumnName, value);
                limit = value;
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
        public virtual long BoardID
        {
            get { return boardID; }
        }

        // ===================================METHODS=============================================================
        protected override string[] KeysToArray()
        {
            string[] keysArr = new string[3];
            keysArr[0] = Email;
            keysArr[1] = columnOrdinal.ToString();
            keysArr[2] = boardID.ToString();
            return keysArr;
        }

        protected override string[] KeyColumnsToArray()
        {
            string[] keyColumnsArr = new string[3];
            keyColumnsArr[0] = EmailColumnName;
            keyColumnsArr[1] = ColumnOrdinalColumnName;
            keyColumnsArr[2] = BoardIDColumnName;
            return keyColumnsArr;
        }

        public override void Insert()
        {
            ColumnDalController CDC = (ColumnDalController)controller;
            CDC.Insert(this);
            log.Info($"Inserted new Column to the DB {name} {email} {boardID}");
        }

    }
}
