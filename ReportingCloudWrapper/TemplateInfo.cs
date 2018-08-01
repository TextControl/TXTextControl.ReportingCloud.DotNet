using System.Collections.Generic;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** TemplateInfo **
    // This class implements the structure request object TemplateInfo
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud TemplateInfo object.
    /// </summary>
    public class TemplateInfo
    {
        /// <summary>
        /// The name of the template.
        /// </summary>
        public string TemplateName;
        /// <summary>
        /// The list of the contained merge blocks.
        /// </summary>
        public List<MergeBlock> MergeBlocks { get; set; }
        /// <summary>
        /// The list of the contained merge fields.
        /// </summary>
        public List<MergeField> MergeFields { get; set; }
        /// <summary>
        /// Dictionary with user defined document properties
        /// </summary>
        public Dictionary<string, object> UserDocumentProperties { get; set; }

        public TemplateInfo()
        {
            this.MergeBlocks = new List<MergeBlock>();
            this.MergeFields = new List<MergeField>();
            this.UserDocumentProperties = new Dictionary<string, object>();
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** MergeField **
    // This class implements the structure request object MergeField
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud MergeField object.
    /// </summary>
    public class MergeField
    {
        /// <summary>
        /// Specifies a string format which is applied to date / time values.
        /// </summary>
        public string DateTimeFormat { get; set; }
        /// <summary>
        /// Gets and sets the name of the field.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Specifies a string format which is applied to numeric values.
        /// </summary>
        public string NumericFormat { get; set; }
        /// <summary>
        /// Specifies whether the formatting is perserved.
        /// </summary>
        public bool PreserveFormatting { get; set; }
        /// <summary>
        /// Gets and sets the text of the field.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets and sets the text after the field.
        /// </summary>
        public string TextAfter { get; set; }
        /// <summary>
        /// Gets and sets the text before the field.
        /// </summary>
        public string TextBefore { get; set; }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** MergeBlock **
    // This class implements the structure request object MergeBlock
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud MergeBlock object.
    /// </summary>
    public class MergeBlock
    {
        public MergeBlock()
        {
            this.MergeBlocks = new List<MergeBlock>();
            this.MergeFields = new List<MergeField>();
        }

        /// <summary>
        /// Gets and sets the name of the merge block.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The list of the contained merge fields.
        /// </summary>
        public List<MergeField> MergeFields { get; set; }
        /// <summary>
        /// The list of the contained merge blocks.
        /// </summary>
        public List<MergeBlock> MergeBlocks { get; set; }
    }
}
