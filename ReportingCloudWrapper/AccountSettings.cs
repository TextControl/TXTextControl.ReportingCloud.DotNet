using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** AccountSettings **
    // This class implements the structure of the returned Web Api AccountSettings object
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud account settings.
    /// </summary>
    public class AccountSettings
    {
        /// <summary>
        /// The serial number that is attached to the account. Possible values are FREE, TRIAL and a 13 character long serial number.
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// The number of created documents in the current month.
        /// </summary>
        public int CreatedDocuments { get; set; }
        /// <summary>
        /// The number of uploaded templates to the template storage.
        /// </summary>
        public int UploadedTemplates { get; set; }
        /// <summary>
        /// The maximum number of documents that can be created per month.
        /// </summary>
        public int MaxDocuments { get; set; }
        /// <summary>
        /// The maximum number of templates that can be uploaded to the template storage.
        /// </summary>
        public int MaxTemplates { get; set; }
        /// <summary>
        /// Returns the date until the current subscription is valid. The date string includes the time zone of the server (UTC). Sample: 2016-06-06T11:25:44+00:00.
        /// </summary>
        public DateTime? ValidUntil { get; set; }
    }
}
