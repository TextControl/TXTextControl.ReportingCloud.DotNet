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
    /// This class provides the structure of the ReportingCloud merge body used in the Merge method.
    /// </summary>
    public class MergeBody
    {
        public object MergeData { get; set; }
        public string Template { get; set; }
        public MergeSettings MergeSettings { get; set; }
    }
}
