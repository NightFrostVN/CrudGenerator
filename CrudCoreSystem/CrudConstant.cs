using System.Collections.Generic;

namespace CrudCoreSystem
{
    public class CrudConstant
    {
        /// <summary>
        /// List các tên cột không cho vào param khi read
        /// </summary>
        protected List<string> LIST_EXCLUDE_READ_PARAM_COLUMN_NAME = new List<string>()
        {
            "CreatedBy",
            "ModifiedBy",
            "CreatedDate",
            "ModifiedDate"
        };

        /// <summary>
        /// Connection string
        /// </summary>
        protected string CONNECTION_STRING = "";
    }
}
