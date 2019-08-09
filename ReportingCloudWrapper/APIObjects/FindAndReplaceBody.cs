using System;
using System.Collections.Generic;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** FindAndReplaceBody **
    // This class implements the structure request object FindAndReplaceBody
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud FindAndReplaceBody object used
    /// in the FindAndReplace method.
    /// </summary>
    public class FindAndReplaceBody
    {
        private string sTemplate = String.Empty;

        /// <summary>
        /// The replacement list array for the find and replace process as an array of string arrays.
        /// </summary>
        public List<string[]> FindAndReplaceData { get; set; }
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
