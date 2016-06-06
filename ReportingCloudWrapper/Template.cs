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
    /// <member name="TemplateName">The name of the template in the template storage.</member>
    /// <param name="Modified">The name of the template in the template storage.</param>
    /// <param name="Size">The name of the template in the template storage.</param>
    public class Template
    {
        public string TemplateName { get; set; }
        public DateTime Modified { get; set; }
        public long Size { get; set; }
    }
}
