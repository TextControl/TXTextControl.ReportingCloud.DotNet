using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** Template **
    // This class implements the structure of the returned Web Api Template object
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of a template in the template storage.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// The filename of the template in the template storage.
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// The last date on that this template has been modified. The date string includes the time zone of the server (UTC). Sample: 2016-06-06T11:25:44+00:00.
        /// </summary>
        public DateTime Modified { get; set; }
        /// <summary>
        /// The size of the template in bytes.
        /// </summary>
        public long Size { get; set; }
    }
}
