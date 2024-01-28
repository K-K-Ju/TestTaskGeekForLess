using System.ComponentModel.DataAnnotations.Schema;

namespace TestTaskGeekForLess.Models
{
    [NotMapped]
    public class UploadFileWrapper
    {
        public IFormFile File { get; set; }
    }
}
