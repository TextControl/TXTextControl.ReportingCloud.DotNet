using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** ReturnFormat **
    // This enum lists all possible return formats for the Merge method
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This enumeration provides the supported document return formats.
    /// </summary>
    public enum ReturnFormat
    {
        PDF,
        PDFA,
        DOC,
        DOCX,
        RTF,
        TX,
        HTML
    }

    /*-------------------------------------------------------------------------------------------------------
   // ** ImageFormat **
   // This enum lists all possible image formats for the thumbnails
   *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This enumeration provides the supported image formats.
    /// </summary>
    public enum ImageFormat
    {
        JPG,
        PNG,
        BMP,
        GIF
    }
}
