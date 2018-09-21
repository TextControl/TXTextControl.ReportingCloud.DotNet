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
        /// The datasource for the merge process as a JSON array.
        /// </summary>
        public List<AppendDocument> Documents { get; set; } = new List<AppendDocument>();
        /// <summary>
        /// Optional. Optional merge settings to specify merge properties and document properties such as title and author.
        /// </summary>
        public DocumentSettings DocumentSettings { get; set; }
    }

    public class AppendDocument
    {
        public byte[] Document { get; set; }
        public DocumentDivider DocumentDivider { get; set; }
    }

    public enum DocumentDivider
    {
        None = 1,
        NewParagraph = 2,
        NewSection = 3,
    }
}
