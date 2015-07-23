namespace StudentSystem.Models
{
    using System.ComponentModel;

    public enum ContentType
    {
        Application,
        [Description("PDF or application")]
        PdfOrApplication,
        Zip
    }
}
