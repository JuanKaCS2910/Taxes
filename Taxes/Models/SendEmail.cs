using System.ComponentModel.DataAnnotations.Schema;

namespace Taxes.Models
{
    [NotMapped]
    public class SendEmail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}