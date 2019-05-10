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
    /// This class provides the structure of the ReportingCloud MergeSettings object used in the Merge method.
    /// </summary>
    public class MergeSettings : DocumentSettings
    {
        /// <summary>
        /// Optional. Specifies whether empty fields should be removed from the template or not. The default value is true.
        /// </summary>
        public bool? RemoveEmptyFields { get; set; }
        /// <summary>
        /// Optional. Specifies whether field data can contain formatted Html content or not. The default value is false.
        /// </summary>
        public bool? MergeHtml { get; set; }
        /// <summary>
        /// Optional. Specifies whether the content of empty merge blocks should be removed from the template or not. The default value is true.
        /// </summary>
        public bool? RemoveEmptyBlocks { get; set; }
        /// <summary>
        /// Optional. Specifies whether images which don't have merge data should be removed from the template or not. The default value is false.
        /// </summary>
        public bool? RemoveEmptyImages { get; set; }
        /// <summary>
        /// Optional. Specifies whether trailing whitespace should be removed before saving a document. The default value is true.
        /// </summary>
        public bool? RemoveTrailingWhitespace { get; set; }
        /// <summary>
        /// Optional. Specifies whether lines should be removed that contain only empty fields and no other content. The default value is false.
        /// </summary>
        public bool? RemoveEmptyLines { get; set; }
        /// <summary>
        /// Optional. Sets the culture for the merge process for date and currency values.
        /// </summary>
#if NET45
        public System.Globalization.CultureInfo Culture { get; set; }
#else
        public string Culture { get; set; }
#endif
    }

    /// <summary>
    /// This class provides the structure of the ReportingCloud DocumentSettings object used in the Merge and Append methods.
    /// </summary>
    public class DocumentSettings
    {
        /// <summary>
        /// Optional. Sets the document's author which will be saved in the document.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Optional. Sets the document's creation date which will be saved in the document.
        /// </summary>
        public DateTime? CreationDate { get; set; }
        /// <summary>
        /// Optional. Sets the application, which has created the document.
        /// </summary>
        public string CreatorApplication { get; set; }
        /// <summary>
        /// Optional. Sets the document's subject string which will be saved in the document.
        /// </summary>
        public string DocumentSubject { get; set; }
        /// <summary>
        /// Optional. Sets the document's title that will be saved in the document.
        /// </summary>
        public string DocumentTitle { get; set; }
        /// <summary>
        /// Optional. Sets the date the document is last modified.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }
        /// <summary>
        /// Optional. Sets the date the document is last modified.
        /// </summary>
        public string UserPassword { get; set; }
    }
}
