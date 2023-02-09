using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class BaseModel
    {
        ///<summary>
        /// Id of the model
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// Created id
        ///</summary>
        public Guid CreatedBy { get; set; }

        ///<summary>
        /// Updated id
        ///</summary>
        public Guid UpdatedBy { get; set; }

        ///<summary>
        /// Created Date 
        ///</summary>
        public DateTime CreatedDate { get; set; }

        ///<summary>
        /// Updated date
        ///</summary>
        public DateTime UpdateDate { get; set; }

        ///<summary>
        /// is the model is active or not
        ///</summary>
        public bool IsActive { get; set; }
    }
}
