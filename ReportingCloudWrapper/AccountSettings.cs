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
        public string SerialNumber { get; set; }
        public int CreatedDocuments { get; set; }
        public int UploadedTemplates { get; set; }
        public int MaxDocuments { get; set; }
        public int MaxTemplates { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
