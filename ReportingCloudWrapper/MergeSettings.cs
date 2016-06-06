using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** MergeSettings **
    // This class implements the structure request object MergeSettings
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud MergeSettings used in the Merge method.
    /// </summary>
    public class MergeSettings
    {
        public bool? RemoveEmptyFields { get; set; }
        public bool? RemoveEmptyBlocks { get; set; }
        public bool? RemoveEmptyImages { get; set; }
        public bool? RemoveTrailingWhitespace { get; set; }

        public string Author { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatorApplication { get; set; }
        public string DocumentSubject { get; set; }
        public string DocumentTitle { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string UserPassword { get; set; }
    }
}
