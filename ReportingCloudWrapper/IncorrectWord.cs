using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** IncorrectWord **
    // This class implements the structure of an IncorrectWord
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class implements the structure of an IncorrectWord.
    /// </summary>
    public class IncorrectWord
    {
        /// <summary>
        /// Gets the length of the spelled word.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Gets the starting position of a spelled word.
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// Gets the text of the spelled word.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets a value indicating whether the spelled word is declared as incorrect, because the previous word has the same text.
        /// </summary>
        public bool IsDuplicate { get; set; }
        /// <summary>
        /// Gets a value indicating the language the incorrect word was spelled.
        /// </summary>
        public System.Globalization.CultureInfo Language { get; set; }
    }
}
