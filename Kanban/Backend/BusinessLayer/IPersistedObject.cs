using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface IPersistedObject<T> where T : DTO
    {
        /// <summary>
        /// Create a DataAccsessLayer Object
        /// </summary>
        ///  <returns> returns a DataAccessLayer from type T with current T(this) fields data</returns>

      T ToDalObject();
    }
}
