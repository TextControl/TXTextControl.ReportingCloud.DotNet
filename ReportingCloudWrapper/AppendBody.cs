using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** AppendBody **
    // This class implements the structure request object AppendBody
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud AppendBody object used in the Append method.
    /// </summary>
    public class AppendBody
    {
        /// <summary>
        /// The documents that will be combined into one document.
        /// </summary>
        public List<AppendDocument> Documents { get; set; } = new List<AppendDocument>();
        /// <summary>
        /// Optional. Optional document settings to specify document properties such as title and author.
        /// </summary>
        public DocumentSettings DocumentSettings { get; set; }
    }

    /// <summary>
    /// This class provides the structure of a AppendDocument object used in the Append method.
    /// </summary>
    public class AppendDocument
    {
        /// <summary>
        /// Specifies the document to be appended.
        /// </summary>
        public byte[] Document { get; set; }
        /// <summary>
        /// Specified the divider between this document and the previous document.
        /// </summary>
        public DocumentDivider DocumentDivider { get; set; }
    }

    /// <summary>
    /// This enum defines the document divider.
    /// </summary>
    public enum DocumentDivider
    {
        None = 1,
        NewParagraph = 2,
        NewSection = 3,
    }
}
