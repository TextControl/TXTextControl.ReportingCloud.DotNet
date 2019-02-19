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
        /// <summary>
        /// Adobe PDF
        /// </summary>
        PDF,
        /// <summary>
        /// Adobe PDF/A
        /// </summary>
        PDFA,
        /// <summary>
        /// Microsoft Word 97-2003
        /// </summary>
        DOC,
        /// <summary>
        /// Microsoft Office Open XML
        /// </summary>
        DOCX,
        /// <summary>
        /// Rich Text Format
        /// </summary>
        RTF,
        /// <summary>
        /// TX Text Control Internal Format
        /// </summary>
        TX,
        /// <summary>
        /// Hypertext Markup Language
        /// </summary>
        HTML,
        /// <summary>
        /// Plain Text
        /// </summary>
        TXT
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
        /// <summary>
        /// Joint Photographic Experts Group
        /// </summary>
        JPG,
        /// <summary>
        /// Portable Network Graphics
        /// </summary>
        PNG,
        /// <summary>
        /// Windows Bitmap
        /// </summary>
        BMP,
        /// <summary>
        /// Graphics Interchange Format
        /// </summary>
        GIF
    }
}
