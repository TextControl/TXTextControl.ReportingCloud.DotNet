using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** MergeBody **
    // This class implements the structure request object MergeBody
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud MergeBody object used in the Merge method.
    /// </summary>
    public class MergeBody
    {
        private string sTemplate = String.Empty;

        /// <summary>
        /// The datasource for the merge process as a JSON array.
        /// </summary>
        public object MergeData { get; set; }
        /// <summary>
        /// Optional. The template as a Byte array. Supported formats are RTF, DOC, DOCX and TX.
        /// </summary>
        public byte[] Template { get; set; }
        /// <summary>
        /// Optional. Optional merge settings to specify merge properties and document properties such as title and author.
        /// </summary>
        public MergeSettings MergeSettings { get; set; }
    }
}
