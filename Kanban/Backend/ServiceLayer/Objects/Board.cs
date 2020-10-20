using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly IReadOnlyCollection<string> ColumnsNames;
        public readonly string emailCreator;

        /// <summary>
        /// classic constructor
        /// </summary>
        /// <param name="columnsNames">board's column</param>
        internal Board(IReadOnlyCollection<string> columnsNames, string emailCreator)
        {
            this.ColumnsNames = columnsNames;
            this.emailCreator = emailCreator;
        }

        /// <summary>
        /// overriding tostring object
        /// </summary>
        /// <returns>new format tostring</returns>
        override
        public string ToString()
        {
            string output = "";
            foreach (string str in ColumnsNames)
                output = output + str + " ";
            return output;

        }
    }
}