using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace DataHandler.DataLayer.Models;

public class ErrorModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string ErrorMessage { get; set; }
    public int? LineNumber { get; set; }
    public int? ColumnNumber { get; set; }
    public string Line { get; set; }

    public FileUploadModel FileUpload { get; set; }
}